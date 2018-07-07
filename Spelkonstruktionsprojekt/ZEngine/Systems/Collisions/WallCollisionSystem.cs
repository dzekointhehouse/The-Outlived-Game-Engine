using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    class WallCollisionSystem : ISystem, IUpdateables
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.WallCollision, Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            //Debug.WriteLine("WALL E:" + collisionEvent.Entity + ", T:" + collisionEvent.Target + ", -:" + collisionEvent.Event);
            if (IsBulletCollisionAndNotRelevant(collisionEvent)) return;
            var collisionComponent =
                ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(collisionEvent.Entity);
            var targetCollisionComponent =
                ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(collisionEvent.Target);
            if (targetCollisionComponent != null)
            {
                if (collisionComponent.Disabled || targetCollisionComponent.Disabled) return;
            }
            
            var entityMoveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(collisionEvent.Entity);
            var entityPositionComponent = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(collisionEvent.Entity);
            StopMovement(entityPositionComponent, entityMoveComponent);
        }

        private void StopMovement(PositionComponent positionComponent, MoveComponent moveComponent)
        {
            positionComponent.Position = moveComponent.PreviousPosition;
            moveComponent.Direction = moveComponent.PreviousDirection;
        }

        private static bool IsBulletCollisionAndNotRelevant(SpecificCollisionEvent collisionEvent)
        {
            var bulletComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Target);
            if (bulletComponent != null)
            {
                if (bulletComponent.ShooterEntityId == collisionEvent.Entity) return true;
            }
            var bulletComponent2 =
                ComponentManager.Instance.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Entity);
            if (bulletComponent2 != null)
            {
                if (bulletComponent2.ShooterEntityId == collisionEvent.Target) return true;
            }

            var sourceBulletComponent = ComponentManager.Instance.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Entity);
            if (sourceBulletComponent != null) return true;

            return false;
        }

        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }
        public void Update(GameTime gt)
        {
        }
    }
}
