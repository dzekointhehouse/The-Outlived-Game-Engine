using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
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
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.BulletCollision, Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            if (TargetIsShooter(collisionEvent)) return;

            if (ComponentManager.EntityHasComponent<HealthComponent>(collisionEvent.Target))
            {
                Debug.WriteLine("Added damage");
                var healthComponent = ComponentManager.GetEntityComponentOrDefault<HealthComponent>(collisionEvent.Target);
                if (healthComponent != null)
                {
                    var bulletComponent = ComponentManager.GetEntityComponentOrDefault<BulletComponent>(collisionEvent.Entity);
                    if (bulletComponent != null)
                    {
                        var damage = bulletComponent.Damage;
                        healthComponent.Damage.Add(damage);
                    }
                    else {
//                        healthComponent.Damage.Add(5);
                        Debug.WriteLine("Entity " + collisionEvent.Entity + " does not have the bullet component");
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

        private void StopBulletAnimation(double currentTime, int bulletId)
        {
            var animationComponent = ComponentManager.Instance.GetEntityComponentOrDefault<AnimationComponent>(bulletId);
            animationComponent.Animations.ForEach(
                animation => { animation.Animation.Invoke(currentTime + animation.Length); });
        }
    }
}
