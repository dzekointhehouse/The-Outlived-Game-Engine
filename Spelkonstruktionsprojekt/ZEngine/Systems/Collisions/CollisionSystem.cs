using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Wrappers;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Runtime.Remoting.Messaging;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using Color = Microsoft.Xna.Framework.Color;
using Debug = System.Diagnostics.Debug;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using ZEngine.Helpers;
using ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;

namespace ZEngine.Systems
{
    class CollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        private Dictionary<Tuple<string, Point>, Color[]> cache = new Dictionary<Tuple<string, Point>, Color[]>();
        private Dictionary<uint, bool> IsAI = new Dictionary<uint, bool>();
        private Dictionary<uint, bool> IsBullet = new Dictionary<uint, bool>();

        private Dictionary<uint, int> CallCount = new Dictionary<uint, int>();
        
        private int cacheMisses = 0;
        
        private QuadTree QuadTree { get; set; } = new QuadTree(ComponentManager.Instance);
        
        //TODO Should do this on game load
        public Color[] TextureCache(SpriteComponent spriteComponent, uint entityId)
        {
            Color[] data;
            var key = new Tuple<string, Point>(spriteComponent.SpriteName, spriteComponent.Position);
            var status = cache.TryGetValue(key, out data);
            if (!status)
            {
                int x, y;

                var animationComponent =
                    ComponentManager.GetEntityComponentOrDefault<SpriteAnimationComponent>(entityId);
                if (animationComponent == null || (animationComponent.CurrentAnimatedState == null &&
                                                   animationComponent.NextAnimatedState == null))
                {
                    x = spriteComponent.Position.X;
                    y = spriteComponent.Position.Y;
                }
                else
                {
                    SpriteAnimationBinding animationState;
                    if (animationComponent.CurrentAnimatedState != null)
                    {
                        animationState = animationComponent.CurrentAnimatedState;
                    }
                    else
                    {
                        animationState = animationComponent.NextAnimatedState;
                    }

                    x = animationState.StartPosition.X;
                    y = animationState.StartPosition.Y;
                }
                data =
                    new Color[spriteComponent.TileWidth * spriteComponent.TileHeight];
                spriteComponent.Sprite.GetData(0,
                    new Rectangle(
                        x,
                        y,
                        spriteComponent.TileWidth,
                        spriteComponent.TileHeight),
                    data, 0, data.Length);
                cache[key] = data;
                if (PROFILING_CACHE)
                {
                    cacheMisses++;
                }
            }
            return data;
        }

        private const bool PROFILING_COLLISIONS = false;
        private const bool PROFILING_COLLISIONS_DEEP = false;
        private const bool PROFILING_CACHE = false;
        private const bool PROFILING_CALL_COUNT = false;
        
        private int movingEntity = 0;
        private int stillEntity = 0;
        private int detectCollision = 0;
        private int entitiesCollide = 0;

        public async Task DetectCollisions()
        {
            Stopwatch timer;
            if (PROFILING_COLLISIONS)
            {
                timer = Stopwatch.StartNew();
            }
            //
            QuadTree.CreateTree(
                ComponentManager.GetEntitiesWithComponent(typeof(CollisionComponent)).Keys,
                new Rectangle(0, 0, 50000, 50000)
            );

            if (PROFILING_COLLISIONS)
            {
                Debug.WriteLine("QUAD TREE CONSTR." + timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }

            var tasks = new List<Task>();
            foreach (var movingEntity in QuadTree.Entities())
            {
                this.movingEntity++;
                if (CallCount.ContainsKey(movingEntity.Key))
                {
                    CallCount[movingEntity.Key]++;
                }
                else
                {
                    CallCount[movingEntity.Key] = 1;
                }
                
                foreach (var stillEntity in movingEntity.Value)
                {
                    this.stillEntity++;
                    if (CallCount.ContainsKey(stillEntity))
                    {
                        CallCount[stillEntity]++;
                    }
                    else
                    {
                        CallCount[stillEntity] = 1;
                    }
                
                    if (IsBothAI(movingEntity.Key, stillEntity)) continue;
                    if (IsStillBullet(stillEntity)) continue;
//                    await CollisionDetection(movingEntity, stillEntity);
                    tasks.Add(CollisionDetection(movingEntity.Key, stillEntity));
                }
            }
//            if (PROFILING_COLLISIONS)
//            {
//                Debug.WriteLine("QUAD TRAVERSE TOTAL " + timer.ElapsedTicks);
//                timer = Stopwatch.StartNew();
//            }
            await Task.WhenAll(tasks);
            if (PROFILING_COLLISIONS)
            {
                Debug.WriteLine("COL. TOTAL " + timer.ElapsedTicks);
            }
            if (PROFILING_CACHE)
            {
                Debug.WriteLine("END OF COL.DET. MISSES: " + cacheMisses);
                cacheMisses = 0;
            }
            if (PROFILING_CALL_COUNT)
            {
                Debug.WriteLine("MOVING: " + this.movingEntity + ", STILL: " + this.stillEntity + ", DETECT: " + this.detectCollision + ", COL: " + this.entitiesCollide);
                int count = 0;
                foreach (var e in CallCount)
                {
                    Debug.WriteLine("ID: " + e.Key + " COUNT: " + e.Value);
                    count += e.Value;
                }
                Debug.WriteLine("REPETITION AVERAGE: " + count / CallCount.Count);
                Debug.WriteLine("");
            }
            this.movingEntity = 0;
            this.stillEntity = 0;
            this.detectCollision = 0;
            this.entitiesCollide = 0;
        }

        private bool IsBothAI(uint moving, uint still)
        {
            bool movingIsAI;
            bool stillIsAI;
            if (IsAI.TryGetValue(moving, out movingIsAI) &&
                IsAI.TryGetValue(still, out stillIsAI))
            {
                if (movingIsAI && stillIsAI) return true;
            }
            else
            {
                var aiComponent = ComponentManager.GetEntityComponentOrDefault<AIComponent>(moving);
                var stillAiComponent = ComponentManager.GetEntityComponentOrDefault<AIComponent>(still);
                IsAI[moving] = aiComponent != null;
                IsAI[still] = stillAiComponent != null;
                if (aiComponent != null && stillAiComponent != null) return true;
            }
            return false;
        }

        private bool IsStillBullet(uint still)
        {
            bool stillIsBullet;
            if (IsBullet.TryGetValue(still, out stillIsBullet))
            {
                if (stillIsBullet) return true;
            }
            else
            {
                var stillBulletComponent = ComponentManager.GetEntityComponentOrDefault<BulletComponent>(still);
                IsBullet[still] = stillBulletComponent != null;
                if (stillBulletComponent != null) return true;
            }
            return false;
        }

        private async Task CollisionDetection(uint movingEntity, uint stillEntity)
        {
            this.detectCollision++;
            Stopwatch timer;
            var movingCollision =
                ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(movingEntity);
            var areaComponent =
                ComponentManager.GetEntityComponentOrDefault<EventZoneComponent>(stillEntity);

            if (areaComponent != null)
            {
                if (PROFILING_COLLISIONS_DEEP)
                {
                    timer = Stopwatch.StartNew();
                }
                var isPlayer = ComponentManager.EntityHasComponent<PlayerComponent>(movingEntity);
                if (isPlayer)
                {
                    if (EntityIsContained(movingEntity, stillEntity))
                    {
                        if (!areaComponent.Inhabitants.Contains(movingEntity))
                        {
                            areaComponent.NewInhabitants.Add(movingEntity);
                            areaComponent.Inhabitants.Add(movingEntity);
                        }
                    }
                    else
                    {
                        areaComponent.Inhabitants.Remove(movingEntity);
                    }
                }
                if (PROFILING_COLLISIONS_DEEP)
                {
                    var time = timer.ElapsedTicks;
                    if (time > 5000)
                    {
                        Debug.WriteLine("INSIDE MOVING " + movingEntity + "/STILL " + stillEntity +
                                        " Time: " + time);
                        ComponentManager.Debug_ListComponentsForEntity(movingEntity);
                        ComponentManager.Debug_ListComponentsForEntity(stillEntity);
                    }
                }
            }
            else
            {
                if (PROFILING_COLLISIONS_DEEP)
                {
                    timer = Stopwatch.StartNew();
                }
                if (EntitiesCollide(movingEntity, stillEntity))
                {
                    this.entitiesCollide++;
                    movingCollision.Collisions.Add(stillEntity);
                    //TODO might be that we need to add collision id to stillEntity as well
                }
                if (PROFILING_COLLISIONS_DEEP)
                {
                    var time = timer.ElapsedTicks;
                    if (time > 0)
                    {
                        Debug.WriteLine("COLLIDED MOVING " + movingEntity + "/STILL " + stillEntity +
                                        " Time: " + time);
//                        Debug.WriteLine("QUAD NODE SIZE " + movingEntity.Item2.Bounds + " PERM.ELEMENTS: " +
//                                        movingEntity.Item2.PermanentStillEntities.Count + " TEMP.ELEMENTS: " +
//                                        movingEntity.Item2.TempStillEntitiesCount);
                        ComponentManager.Debug_ListComponentsForEntity(movingEntity);
                        ComponentManager.Debug_ListComponentsForEntity(stillEntity);
                    }
                }
            }
        }

        private const bool PROFILING = false;

        private bool EntityIsContained(uint entity, uint zone)
        {
            var movingDimensionsComponent =
                ComponentManager.GetEntityComponentOrDefault<DimensionsComponent>(entity);
            if (movingDimensionsComponent == null) return false;
            var movingPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity);
            if (movingPositionComponent == null) return false;

            var stillDimensionsComponent =
                ComponentManager.GetEntityComponentOrDefault<DimensionsComponent>(zone);
            if (stillDimensionsComponent == null) return false;
            var stillPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(zone);
            if (stillPositionComponent == null) return false;

            return new Rectangle((int) stillPositionComponent.Position.X, (int) stillPositionComponent.Position.Y,
                stillDimensionsComponent.Width, stillDimensionsComponent.Height).Contains(
                new Rectangle((int) movingPositionComponent.Position.X, (int) movingPositionComponent.Position.Y,
                    movingDimensionsComponent.Width, movingDimensionsComponent.Height));
        }

        private bool EntitiesCollide(uint movingEntity, uint stillEntity)
        {
            Stopwatch timer = null;
            if (PROFILING) timer = Stopwatch.StartNew();
            var movingDimensionsComponent =
                ComponentManager.GetEntityComponentOrDefault<DimensionsComponent>(movingEntity);
            if (movingDimensionsComponent == null) return false;
            var movingPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(movingEntity);
            if (movingPositionComponent == null) return false;

            var stillDimensionsComponent =
                ComponentManager.GetEntityComponentOrDefault<DimensionsComponent>(stillEntity);
            if (stillDimensionsComponent == null) return false;
            var stillPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(stillEntity);
            if (stillPositionComponent == null) return false;

            var movingMoveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(movingEntity);
            if (movingMoveComponent == null) return false;
            var movingOffset = Vector2.Zero;
            //            movingEntityOffset != null
            //                ? new Vector2(movingEntityOffset.Offset.X, movingEntityOffset.Offset.Y)
            //                : Vector2.Zero;
            var movingSpriteComponent = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(movingEntity);
            if (movingSpriteComponent == null) return false;

            var stillMoveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(stillEntity);
            //            if (stillMoveComponent == null) return false;
            var stillEntityAngle = stillMoveComponent?.Direction ?? 0;
            var stillOffset = Vector2.Zero;
            //            stillEntityOffset != null
            //                ? new Vector2(stillEntityOffset.Offset.X, stillEntityOffset.Offset.Y)
            //                : Vector2.Zero;
            var stillSpriteComponent = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(stillEntity);
            if (stillSpriteComponent == null) return false;

            if (PROFILING)
            {
                timer.Stop();
                Debug.WriteLine("GETTING DATA: " + timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }

            var colorA = TextureCache(movingSpriteComponent, movingEntity);

            var colorB = TextureCache(stillSpriteComponent, stillEntity);

            if (PROFILING)
            {
                timer.Stop();
                Debug.WriteLine("TEXTURE DATA: " + timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }

            var matrixA =
                Matrix.CreateTranslation(new Vector3(
                    (float) (-movingSpriteComponent.TileWidth * 0.5),
                    (float) (-movingSpriteComponent.TileHeight * 0.5), 0)) *
                Matrix.CreateScale(
                    (float) movingDimensionsComponent.Width / (float) movingSpriteComponent.TileWidth,
                    (float) movingDimensionsComponent.Height / (float) movingSpriteComponent.TileHeight,
                    1) *
                Matrix.CreateRotationZ(movingMoveComponent.Direction) *
                Matrix.CreateTranslation(movingPositionComponent.Position.X, movingPositionComponent.Position.Y, 0f);

            var matrixB =
                Matrix.CreateTranslation(new Vector3(
                    (float) (-stillSpriteComponent.TileWidth * 0.5),
                    (float) (-stillSpriteComponent.TileHeight * 0.5), 0)) *
                Matrix.CreateScale(
                    (float) stillDimensionsComponent.Width / (float) stillSpriteComponent.TileWidth,
                    (float) stillDimensionsComponent.Height / (float) stillSpriteComponent.TileHeight,
                    1) *
                Matrix.CreateRotationZ(stillEntityAngle) *
                Matrix.CreateTranslation(stillPositionComponent.Position.X, stillPositionComponent.Position.Y, 0f);

            if (PROFILING)
            {
                timer.Stop();
                Debug.WriteLine("MATRIX SETUP: " + timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }

            var movingEntityCollisionBounds =
                CalculateCollisionBounds(
                    new Rectangle(
                        (int) 0,
                        (int) 0,
                        movingSpriteComponent.TileWidth,
                        movingSpriteComponent.TileHeight
                    ),
                    0,
                    movingOffset,
                    matrixA
                );

            var stillEntityCollisionBounds =
                CalculateCollisionBounds(
                    new Rectangle(
                        (int) 0,
                        (int) 0,
                        stillSpriteComponent.TileWidth,
                        stillSpriteComponent.TileHeight
                    ),
                    0,
                    stillOffset,
                    matrixB
                );


            if (movingEntityCollisionBounds.Intersects(stillEntityCollisionBounds))
            {
                if (PROFILING)
                {
                    timer.Stop();
                    Debug.WriteLine("BOUNDING BOXES: " + timer.ElapsedTicks);
                    timer = Stopwatch.StartNew();
                }
                if (IntersectPixs(matrixA,
                    movingSpriteComponent.TileWidth, movingSpriteComponent.TileHeight, colorA,
                    matrixB, stillSpriteComponent.TileWidth, stillSpriteComponent.TileHeight,
                    colorB))
                {
                    if (PROFILING)
                    {
                        timer.Stop();
                        Debug.WriteLine("PIXEL TRUE: " + timer.ElapsedTicks);
                    }
                    return true;
                }

                if (PROFILING)
                {
                    timer.Stop();
                    Debug.WriteLine("PIXEL FALSE: " + timer.ElapsedTicks);
                }
                return false;
            }
            if (PROFILING)
            {
                timer.Stop();
                Debug.WriteLine("BOUNDING BOXES [NO INTERSECT]: " + timer.ElapsedTicks);
            }
            return false;
        }

        public Rectangle CalculateCollisionBounds(Rectangle spriteBounds, float angle, Vector2 offset,
            Matrix transformation)
        {
            Vector2 leftTop = new Vector2(spriteBounds.Left, spriteBounds.Top);
            Vector2 rightTop = new Vector2(spriteBounds.Right, spriteBounds.Top);
            Vector2 leftBottom = new Vector2(spriteBounds.Left, spriteBounds.Bottom);
            Vector2 rightBottom = new Vector2(spriteBounds.Right, spriteBounds.Bottom);
            Vector2.Transform(ref leftTop, ref transformation, out leftTop);
            Vector2.Transform(ref rightTop, ref transformation, out rightTop);
            Vector2.Transform(ref leftBottom, ref transformation, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transformation, out rightBottom);

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                Vector2.Max(leftBottom, rightBottom));

            return new Rectangle((int) min.X,
                (int) min.Y,
                (int) (max.X - min.X),
                (int) (max.Y - min.Y));
        }

        public static bool IntersectPixs(
            Matrix transformA, int aWidth, int aHeight, Color[] dataA,
            Matrix transformB, int bWidth, int bHeight, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            //     Vector3 xStep = Vector3.Transform(new Vector3(1, 0, 0), transformAToB);
            //     Vector3 yStep = Vector3.Transform(new Vector3(0, 1, 0), transformAToB);

            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < aHeight; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < aWidth; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int) Math.Round(posInB.X);
                    int yB = (int) Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < bWidth &&
                        0 <= yB && yB < bHeight)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * aWidth];
                        Color colorB = dataB[xB + yB * bWidth];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }
                    // Move to the next pixel in the row
                    posInB += stepX;
                }
                // Move to the next row
                yPosInB += stepY;
            }
            // No intersection found
            return false;
        }

        public void ClearCache()
        {
            cache.Clear();
        }
    }
}
