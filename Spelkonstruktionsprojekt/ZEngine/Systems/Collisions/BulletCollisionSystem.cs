using System.Diagnostics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    internal class BulletCollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;
        private readonly EventBus EventBus = EventBus.Instance;

        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.BulletCollision, Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            if (TargetIsShooter(collisionEvent)) return;
            if (TargetIsBullet(collisionEvent)) return;
            if (ComponentManager.EntityHasComponent<HealthComponent>(collisionEvent.Target))
            {
//                Debug.WriteLine("Added damage");
                var healthComponent =
                    ComponentManager.GetEntityComponentOrDefault<HealthComponent>(collisionEvent.Target);
                if (healthComponent != null)
                {
                    var bulletComponent =
                        ComponentManager.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Entity);
                    if (bulletComponent != null)
                    {
                        var damage = bulletComponent.Damage;
                        healthComponent.Damage.Add(damage);
                    }
                }
            }
            StopBulletAnimation(collisionEvent.EventTime, collisionEvent.Entity);
        }

        private bool TargetIsShooter(SpecificCollisionEvent collisionEvent)
        {
            var bulletComponent = ComponentManager.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Entity);
            return collisionEvent.Target == bulletComponent.ShooterEntityId;
        }

        private bool TargetIsBullet(SpecificCollisionEvent collisionEvent)
        {
            var bulletComponent = ComponentManager.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Target);
            return bulletComponent != null;
        }

        private void StopBulletAnimation(double currentTime, uint bulletId)
        {
            var animationComponent = ComponentManager.Instance
                .GetEntityComponentOrDefault<AnimationComponent>(bulletId);
            if (animationComponent == null) return;
            animationComponent.Animations.ForEach(
                animation => { animation.Animation.Invoke(currentTime + animation.Length); });
        }
    }
}