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
using Spelkonstruktionsprojekt.ZEngine.Systems;

namespace ZEngine.Systems
{
    class CollisionSystem : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        private Dictionary<uint, bool> IsAI;
        private Dictionary<uint, bool> IsBullet;
        private Dictionary<uint, int> CallCount;
        private QuadTree QuadTree;

        public CollisionSystem()
        {
            IsAI = new Dictionary<uint, bool>();
            IsBullet = new Dictionary<uint, bool>();
            CallCount = new Dictionary<uint, int>();
            QuadTree = new QuadTree(ComponentManager.Instance);
        }

        public void Update(GameTime gt)
        {
        }


        private int movingEntity = 0;
        private int stillEntity = 0;
        private int detectCollision = 0;
        private int entitiesCollide = 0;

        public async Task Update()
        {

            QuadTree.CreateTree(
                ComponentManager.GetEntitiesWithComponent(typeof(CollisionComponent)).Keys,
                new Rectangle(0, 0, 50000, 50000)
            );

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

                    tasks.Add(CollisionDetection(movingEntity.Key, stillEntity));
                }
            }

            await Task.WhenAll(tasks);

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
            var movingCollision =
                ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(movingEntity);
            var areaComponent =
                ComponentManager.GetEntityComponentOrDefault<EventZoneComponent>(stillEntity);

            if (areaComponent != null)
            {
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
            }
            else
            {

                if (EntitiesCollide(movingEntity, stillEntity))
                {
                    this.entitiesCollide++;
                    movingCollision.Collisions.Add(stillEntity);
                    //TODO might be that we need to add collision id to stillEntity as well
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

                return true;

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

    }
}
