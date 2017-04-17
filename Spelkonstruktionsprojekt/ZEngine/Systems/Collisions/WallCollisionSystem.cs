using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
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
            EventBus.Subscribe<SpecificCollisionEvent>("WallCollision", Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            if (IsBulletRelatedShouldBeIgnored(collisionEvent)) return;

            var entityMoveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(collisionEvent.Entity);
            var entityRenderComponent = ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(collisionEvent.Entity);
            HaltMovement(entityMoveComponent, entityRenderComponent);
        }

        public void HaltMovement(MoveComponent moveComponent, RenderComponent renderComponent)
        {
            renderComponent.PositionComponent.Position = moveComponent.PreviousPosition;
            moveComponent.Speed = 0;
        }

        public bool IsBulletRelatedShouldBeIgnored(SpecificCollisionEvent collisionEvent)
        {
            var bulletComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Target);
            if (bulletComponent != null)
            {
                return bulletComponent.ShooterEntityId == collisionEvent.Entity;
            }
            var bulletComponent2 =
                ComponentManager.Instance.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Entity);
            if (bulletComponent2 != null)
            {
                return bulletComponent2.ShooterEntityId == collisionEvent.Target;
            }
            return false;
        }
    }
}
