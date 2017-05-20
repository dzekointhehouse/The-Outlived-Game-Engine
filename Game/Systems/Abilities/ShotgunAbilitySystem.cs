using System;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.Bullets;
using ZEngine.EventBus;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler
{
    public class ShotgunAbilitySystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        private BulletFactory BulletFactory { get; set; }

        private const double RateOfFire = 1000;

        public void Start(BulletFactory bulletFactory)
        {
            BulletFactory = bulletFactory;
            EventBus.Subscribe<InputEvent>(EventConstants.FirePistolWeapon, HandleFireWeapon);
        }

        public void HandleFireWeapon(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) return;

            var weaponComponent =
                ComponentManager.GetEntityComponentOrDefault<WeaponComponent>(inputEvent.EntityId);
            if (weaponComponent == null) return;
            if (weaponComponent.WeaponType != WeaponComponent.WeaponTypes.Shotgun) return;
            if (weaponComponent.LastFiredMoment + RateOfFire > inputEvent.EventTime) return;
            weaponComponent.LastFiredMoment = inputEvent.EventTime;

            //Check if they have ammo
            var ammoComponent = ComponentManager.GetEntityComponentOrDefault<AmmoComponent>(inputEvent.EntityId);
            if (ammoComponent != null)
            {
                if (ammoComponent.Amount > 0)
                {
                    ammoComponent.Amount -= 1;
                }
                else
                {
                    //Debug.WriteLine(inputEvent.EntityId + " is out of ammo");
                    return; //Should play a clicking noise
                }
            }

            var bulletIds =
                BulletFactory.CreateShotgunBullet(
                    inputEvent.EntityId,
                    inputEvent.EventTime,
                    weaponComponent.Damage);
            if (bulletIds.Length == 0) return;

            var animationComponent = ComponentManager.ComponentFactory.NewComponent<AnimationComponent>();
            ComponentManager.AddComponentToEntity(animationComponent, bulletIds[0]);

            var animation = new GeneralAnimation()
            {
                AnimationType = "BulletAnimation",
                StartOfAnimation = inputEvent.EventTime,
                Length = 2000,
                Unique = true
            };
            NewBulletAnimation(animation, bulletIds);
            animationComponent.Animations.Add(animation);
        }

        // Animation for when the bullet should be deleted.
        public void NewBulletAnimation(GeneralAnimation generalAnimation, uint[] bulletIds)
        {
            generalAnimation.Animation = delegate(double currentTimeInMilliseconds)
            {
                if (currentTimeInMilliseconds - generalAnimation.StartOfAnimation > generalAnimation.Length)
                {
                    for (var i = 0; i < bulletIds.Length; i++)
                    {
                        var tagComponent = ComponentManager.GetEntityComponentOrDefault<TagComponent>(bulletIds[i]);
                        if (tagComponent == null)
                        {
                            throw new Exception(
                                "Entity does not have a tag component which is needed to remove the entity.");
                        }
                        tagComponent.Tags.Add(Tag.Delete);
                    }
                    generalAnimation.IsDone = true;
                }
            };
        }
    }
}