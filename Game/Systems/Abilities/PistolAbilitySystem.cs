using System;
using System.Linq;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.Bullets;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler
{
    public class PistolAbilitySystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        private BulletFactory BulletFactory { get; set; }

        private const double RateOfFire = 500;

        public void Start(BulletFactory bulletFactory)
        {
            BulletFactory = bulletFactory;
            EventBus.Subscribe<InputEvent>(EventConstants.FireWeapon, HandleFireWeapon);
        }


        public void HandleFireWeapon(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) return;

            var weaponComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<WeaponComponent>(inputEvent.EntityId);
            if (weaponComponent == default(WeaponComponent)) return;
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

            var bulletEntityId =
                BulletFactory.CreatePistolBullet(
                    inputEvent.EntityId,
                    inputEvent.EventTime,
                    weaponComponent.Damage);
            if (bulletEntityId == -1) return;

            var animationComponent = new AnimationComponent();
            ComponentManager.AddComponentToEntity(animationComponent, bulletEntityId);

            var animation = new GeneralAnimation()
            {
                AnimationType = "BulletAnimation",
                StartOfAnimation = inputEvent.EventTime,
                Length = 2000,
                Unique = true
            };
            NewBulletAnimation(animation, bulletEntityId);
            animationComponent.Animations.Add(animation);
        }

        // Animation for when the bullet should be deleted.
        public void NewBulletAnimation(GeneralAnimation generalAnimation, uint entityId)
        {
            generalAnimation.Animation = delegate(double currentTimeInMilliseconds)
            {
                if (currentTimeInMilliseconds - generalAnimation.StartOfAnimation > generalAnimation.Length)
                {
                    var tagComponent = ComponentManager.GetEntityComponentOrDefault<TagComponent>(entityId);
                    if (tagComponent == null)
                    {
                        throw new Exception(
                            "Entity does not have a tag component which is needed to remove the entity.");
                    }
                    tagComponent.Tags.Add(Tag.Delete);
                    generalAnimation.IsDone = true;
                }
            };
        }
    }
}