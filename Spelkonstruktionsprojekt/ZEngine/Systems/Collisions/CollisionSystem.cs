using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Managers;
using ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Wrappers;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using UnityEngine;
using Color = Microsoft.Xna.Framework.Color;
using Debug = System.Diagnostics.Debug;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using ShapeRendering2D;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace ZEngine.Systems
{
    class CollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        private Dictionary<Tuple<uint, Point>, Color[]> cache = new Dictionary<Tuple<uint, Point>, Color[]>();

        public Color[] TextureCache(uint entityId, SpriteComponent spriteComponent)
        {
            Color[] data;
            var key = new Tuple<uint, Point>(entityId, spriteComponent.Position);
            var status = cache.TryGetValue(key, out data);
            if (!status)
            {
                data =
                    new Color[spriteComponent.TileWidth * spriteComponent.TileHeight];
                spriteComponent.Sprite.GetData(0,
                    new Rectangle(
                        spriteComponent.Position.X,
                        spriteComponent.Position.Y,
                        spriteComponent.TileWidth,
                        spriteComponent.TileHeight),
                    data, 0, data.Length);
                cache[key] = data;
            }
            return data;
        }

        private const bool PROFILING_COLLISIONS = true;
        public void DetectCollisions()
        {

            Stopwatch timer;
            if (PROFILING_COLLISIONS)
            {
                timer = Stopwatch.StartNew();
            }

            var collisionEntities = ComponentManager.GetEntitiesWithComponent(typeof(CollisionComponent));

            foreach(var movingEntity in collisionEntities)
            {
                if (ComponentManager.GetEntityComponentOrDefault<MoveComponent>(movingEntity.Key) == null) continue;
                var movingEntityId = movingEntity.Key;
                var movingEntityCollisionComponent = movingEntity.Value as CollisionComponent;

                foreach (var stillEntity in collisionEntities)
                {
                    if (ComponentManager.GetEntityComponentOrDefault<BulletComponent>(stillEntity.Key) != null) continue;
                    var stillEntityId = stillEntity.Key;
                    if (movingEntityId == stillEntityId) continue;

                    var stillEntityCollisionComponent = stillEntity.Value as CollisionComponent;

                    if (stillEntityCollisionComponent.IsCage)
                    {
                        continue;
//                        var cageComponent = ComponentManager.GetEntityComponentOrDefault<CageComponent>(movingEntityId);
//                        var shouldBeCaged = cageComponent != null && cageComponent.CageId == stillEntityId;
//                        var isCaged = EntitiesCollide(stillEntityId, movingEntityId) == ContainmentType.Contains;
//                        if (shouldBeCaged && isCaged)
//                        {
//                            if (!movingEntityCollisionComponent.collisions.Contains(stillEntityId))
//                            {
//                                movingEntityCollisionComponent.collisions.Add(stillEntityId);
//                            }
//                            if (!stillEntityCollisionComponent.collisions.Contains(movingEntityId))
//                            {
//                                stillEntityCollisionComponent.collisions.Add(movingEntityId);
//                            }
//                        }
                    }
                    if (EntitiesCollide(movingEntityId, stillEntityId))
                    {
                        if (!movingEntityCollisionComponent.collisions.Contains(stillEntityId))
                        {
                            movingEntityCollisionComponent.collisions.Add(stillEntityId);
                        }
                        if (!stillEntityCollisionComponent.collisions.Contains(movingEntityId))
                        {
                            stillEntityCollisionComponent.collisions.Add(movingEntityId);
                        }
                    }
                }
            }
            if (PROFILING_COLLISIONS)
            {
                Debug.WriteLine("COLLISION TOTAL " + timer.ElapsedTicks);
            }
        }

        private const bool PROFILING = false;
        private bool EntitiesCollide(uint movingEntity, uint stillEntity)
        {
            Stopwatch timer;
            if(PROFILING) timer = Stopwatch.StartNew();
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

//            //Roughly check distance
//            var outside = ((stillPositionComponent.Position.X + stillDimensionsComponent.Width <
//                           movingPositionComponent.Position.X
//                           || stillPositionComponent.Position.X > movingPositionComponent.Position.X +
//                           movingDimensionsComponent.Width)
//                           && (stillPositionComponent.Position.Y + stillDimensionsComponent.Height <
//                           movingPositionComponent.Position.Y
//                           || stillPositionComponent.Position.Y > movingPositionComponent.Position.Y +
//                           movingDimensionsComponent.Height));

            var aproxDistance = Math.Abs(
                Math.Pow(stillPositionComponent.Position.X - movingPositionComponent.Position.X, 2) +
                Math.Pow(stillPositionComponent.Position.Y - movingPositionComponent.Position.Y, 2));
            var approxResult = (aproxDistance >
                                Math.Pow(movingDimensionsComponent.Width * 0.5 + stillDimensionsComponent.Width * 0.5,
                                    2));
            if (PROFILING)
            {
                timer.Stop();
                Debug.WriteLine("APPROX: " + timer.ElapsedTicks);
            }
            if (approxResult) return false;
            if(PROFILING) timer = Stopwatch.StartNew();
            var movingMoveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(movingEntity);
            if (movingMoveComponent == null) return false;
            var movingCollisionBox = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(movingEntity);
            var movingEntityOffset = ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(movingEntity);
            var movingOffset = Vector2.Zero;
//            movingEntityOffset != null
//                ? new Vector2(movingEntityOffset.Offset.X, movingEntityOffset.Offset.Y)
//                : Vector2.Zero;
            var movingSpriteComponent = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(movingEntity);
            if (movingSpriteComponent == null) return false;

            var stillMoveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(stillEntity);
//            if (stillMoveComponent == null) return false;
            var stillCollisionBox = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(stillEntity);
            var stillEntityAngle = 0;
            var stillEntityOffset = ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(stillEntity);
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

            var colorA = TextureCache(movingEntity, movingSpriteComponent);

            var colorB = TextureCache(stillEntity, stillSpriteComponent);

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
                    movingCollisionBox,
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
                    stillCollisionBox,
                    stillOffset,
                    matrixB
                );



//            var shapeRenderer = CollisionRendering.renderer;
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
//                    shapeRenderer.AddBoundingRectangle(movingEntityCollisionBounds, Color.Red, 0.02f);
//                    shapeRenderer.AddBoundingRectangle(stillEntityCollisionBounds, Color.DarkRed, 0.02f);

                    if (PROFILING)
                    {
                        timer.Stop();
                        Debug.WriteLine("PIXEL TRUE: " + timer.ElapsedTicks);
                    }
                    return true;
                }
//                shapeRenderer.AddBoundingRectangle(movingEntityCollisionBounds, Color.Blue, 0.02f);
//                shapeRenderer.AddBoundingRectangle(stillEntityCollisionBounds, Color.DarkBlue, 0.02f);

                if (PROFILING)
                {
                    timer.Stop();
                    Debug.WriteLine("PIXEL FALSE: " + timer.ElapsedTicks);
                }
                return false;
            }
//            shapeRenderer.AddBoundingRectangle(movingEntityCollisionBounds, Color.Green, 0.02f);
//            shapeRenderer.AddBoundingRectangle(stillEntityCollisionBounds, Color.DarkGreen, 0.02f);

            if (PROFILING)
            {
                timer.Stop();
                Debug.WriteLine("BOUNDING BOXES [NO INTERSECT]: " + timer.ElapsedTicks);
            }
            return false;
        }

        public Rectangle CalculateCollisionBounds(Rectangle spriteBounds, float angle,
            CollisionComponent collisionComponent, Vector2 offset, Matrix transformation)
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
    }
}