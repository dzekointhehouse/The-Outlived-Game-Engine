using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZEngine.Components;
using ZEngine.Helpers;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

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

        public async Task Update()
        {

            QuadTree.CreateTree(
                ComponentManager.GetEntitiesWithComponent(typeof(CollisionComponent)).Keys,
                new Rectangle(0, 0, 50000, 50000)
            );

            var tasks = new List<Task>();
            foreach (var movingEntity in QuadTree.Entities())
            {
                if (CallCount.ContainsKey(movingEntity.Key))
                {
                    CallCount[movingEntity.Key]++;
                }
                else
                {
                    CallCount[movingEntity.Key] = 1;
                }

                var movingCollision =
                     ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(movingEntity.Key);

                movingCollision.BoundingShape.UpdateVolume(movingEntity.Key);

                foreach (var stillEntity in movingEntity.Value)
                {
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
        }



        private async Task CollisionDetection(uint movingEntity, uint stillEntity)
        {
            var movingCollision =
                ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(movingEntity);

            var stillCollision =
                ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(stillEntity);

            var areaComponent =
                ComponentManager.GetEntityComponentOrDefault<EventZoneComponent>(stillEntity);

            if (areaComponent != null)
            {
                var isPlayer = ComponentManager.EntityHasComponent<PlayerComponent>(movingEntity);
                //if (isPlayer)
                //{
                //    if (EntityIsContained(movingEntity, stillEntity))
                //    {
                //        if (!areaComponent.Inhabitants.Contains(movingEntity))
                //        {
                //            areaComponent.NewInhabitants.Add(movingEntity);
                //            areaComponent.Inhabitants.Add(movingEntity);
                //        }
                //    }
                //    else
                //    {
                //        areaComponent.Inhabitants.Remove(movingEntity);
                //    }
                //}
            }
            else
            {
                if (movingCollision.BoundingShape.Intersects(stillCollision.BoundingShape))
                {

                    movingCollision.Collisions.Add(stillEntity);
                    //TODO might be that we need to add collision id to stillEntity as well
                }
            }
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

            return new Rectangle((int)stillPositionComponent.Position.X, (int)stillPositionComponent.Position.Y,
                stillDimensionsComponent.Width, stillDimensionsComponent.Height).Contains(
                new Rectangle((int)movingPositionComponent.Position.X, (int)movingPositionComponent.Position.Y,
                    movingDimensionsComponent.Width, movingDimensionsComponent.Height));
        }

    }
}
