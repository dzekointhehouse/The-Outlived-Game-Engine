using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.Bullets;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Game.Systems.Abilities
{
    public class RifleAbilitySystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        private BulletFactory BulletFactory { get; set; }

        private const double RateOfFire = 100;

        public void Start(BulletFactory bulletFactory)
        {
            BulletFactory = bulletFactory;
            EventBus.Subscribe<InputEvent>(EventConstants.FireWeapon, HandleFireWeapon);
        }

        public void HandleFireWeapon(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent != ActionBindings.KeyEvent.KeyDown) return;

            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(inputEvent.EntityId);
            if (moveComponent == null) return;

            var weaponComponent =
                ComponentManager.GetEntityComponentOrDefault<WeaponComponent>(inputEvent.EntityId);
            if (weaponComponent == default(WeaponComponent)) return;
            if (weaponComponent.LastFiredMoment + RateOfFire > inputEvent.EventTime) return;
            weaponComponent.LastFiredMoment = inputEvent.EventTime;

            var shooterPosition =
                ComponentManager.GetEntityComponentOrDefault<PositionComponent>(inputEvent.EntityId);
            if (shooterPosition == null) return;
            var shooterDimensions =
                ComponentManager.GetEntityComponentOrDefault<DimensionsComponent>(inputEvent.EntityId);
            if (shooterDimensions == null) return;

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

            var bulletIds = new int[1];
            for (var i = 0; i < bulletIds.Length; i++)
            {
                bulletIds[i] =
                    BulletFactory.CreateBullet(
                        inputEvent.EntityId,
                        shooterPosition,
                        shooterDimensions,
                        weaponComponent.Damage,
                        moveComponent.Direction);
            }

            var animationComponent = new AnimationComponent();
            ComponentManager.AddComponentToEntity(animationComponent, bulletIds[0]);

            var animation = new GeneralAnimation()
            {
                AnimationType = "BulletAnimation",
                StartOfAnimation = inputEvent.EventTime,
                Length = 2000,
                Unique = true
            };
            NewBulletAnimation(animation, bulletIds, moveComponent);
            animationComponent.Animations.Add(animation);
        }

        // Animation for when the bullet should be deleted.
        public void NewBulletAnimation(GeneralAnimation generalAnimation, int[] bulletIds,
            MoveComponent shooterMoveComponent)
        {
            var lastFired = generalAnimation.StartOfAnimation;
            var counter = 0;
            generalAnimation.Animation = delegate(double currentTimeInMilliseconds)
            {
                if (counter >= bulletIds.Length && currentTimeInMilliseconds - generalAnimation.StartOfAnimation >
                    generalAnimation.Length)
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
                else if (counter < bulletIds.Length && currentTimeInMilliseconds - lastFired > RateOfFire)
                {
                    lastFired = currentTimeInMilliseconds;
                    BulletFactory.FireBullet(bulletIds[counter++], shooterMoveComponent.Direction);
                }
            };
        }
    }
}