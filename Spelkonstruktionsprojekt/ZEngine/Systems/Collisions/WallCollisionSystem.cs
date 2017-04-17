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
//            Debug.WriteLine("Handle wall collision");
            Debug.WriteLine("WALL E:" + collisionEvent.Entity + ", T:" + collisionEvent.Target + ", -:" + collisionEvent.Event);
            var bulletComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Target);
            if (bulletComponent != null)
            {
                if (bulletComponent.ShooterEntityId == collisionEvent.Entity) return;
            }
            var bulletComponent2 =
                ComponentManager.Instance.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Entity);
            if (bulletComponent2 != null)
            {
                if (bulletComponent2.ShooterEntityId == collisionEvent.Target) return;
            }
            var entityMoveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(collisionEvent.Entity);
            var entityRenderComponent = ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(collisionEvent.Entity);

            entityRenderComponent.PositionComponent.Position = entityMoveComponent.PreviousPosition;
            entityMoveComponent.Speed = 0;

            //var collisonComponent = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(collisionEvent.Entity);
            //collisonComponent.collisions.Remove(collisionEvent.Target);
        }
    }
}
