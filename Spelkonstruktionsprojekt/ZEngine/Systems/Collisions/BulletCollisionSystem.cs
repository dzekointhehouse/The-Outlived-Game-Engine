using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    class BulletCollisionSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>("BulletCollision", Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            Debug.WriteLine("Handle bullet collision");
            var bulletComponent = ComponentManager.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Entity);
            if (collisionEvent.Target == bulletComponent.ShooterEntityId) return;
            var entityMoveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(collisionEvent.Entity);
            var entityRenderComponent = ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(collisionEvent.Entity);
            if (entityMoveComponent == null) return;

            int bulletId = collisionEvent.Entity;
            var animationComponent = ComponentManager.Instance.GetEntityComponentOrDefault<AnimationComponent>(bulletId);
            StopAnimation(collisionEvent.EventTime, animationComponent);

            //entityRenderComponent.PositionComponent.Position = entityMoveComponent.PreviousPosition;
            //entityMoveComponent.Speed = 0;

            //var collisonComponent = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(collisionEvent.Entity);
            //collisonComponent.collisions.Remove(collisionEvent.Target);
        }

        private static void StopAnimation(double currentTime, AnimationComponent animationComponent)
        {
            animationComponent.Animations.ForEach(
                animation => { animation.Animation.Invoke(currentTime + animation.Length); });
        }
    }
}
