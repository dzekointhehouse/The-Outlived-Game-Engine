using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    class WallCollisionSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.WallCollision, Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            Debug.WriteLine("WALL E:" + collisionEvent.Entity + ", T:" + collisionEvent.Target + ", -:" + collisionEvent.Event);
            if (IsBulletCollisionAndNotRelevant(collisionEvent)) return;

            var entityMoveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(collisionEvent.Entity);
            var entityRenderComponent = ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(collisionEvent.Entity);
            StopMovement(entityRenderComponent, entityMoveComponent);
        }

        private void StopMovement(RenderComponent renderComponent, MoveComponent moveComponent)
        {
            renderComponent.PositionComponent.Position = moveComponent.PreviousPosition;
            moveComponent.Speed = 0;
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
            return false;
        }
    }
}
