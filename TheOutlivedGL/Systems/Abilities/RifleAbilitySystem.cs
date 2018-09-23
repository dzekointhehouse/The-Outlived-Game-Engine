using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
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

        private const double RateOfFire = 80;

        public void Start(BulletFactory bulletFactory)
        {
            BulletFactory = bulletFactory;
            EventBus.Subscribe<InputEvent>(EventConstants.FirePistolWeapon, HandleFireWeapon);
        }

        public void HandleFireWeapon(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent != ActionBindings.KeyEvent.KeyDown) return;

            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(inputEvent.EntityId);
            if (moveComponent == null) return;

            var weaponComponent =
                ComponentManager.GetEntityComponentOrDefault<WeaponComponent>(inputEvent.EntityId);
            if (weaponComponent == null) return;
            if (weaponComponent.WeaponType != WeaponComponent.WeaponTypes.Rifle) return;
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
                    EventBus.Publish(EventConstants.EmptyMagSound, inputEvent.EntityId);
                    //Debug.WriteLine(inputEvent.EntityId + " is out of ammo");
                    return; //Should play a clicking noise
                }
            }

            var bulletIds = new uint[1];
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

            var animationComponent = ComponentManager.ComponentFactory.NewComponent<AnimationComponent>();
            ComponentManager.AddComponentToEntity(animationComponent, bulletIds[0]);

            var animation = new GeneralAnimation()
            {
                AnimationType = "BulletAnimation",
                StartOfAnimation = inputEvent.EventTime,
                Length = 2000,
                Unique = true
            };
            NewBulletAnimation(animation, bulletIds, moveComponent, inputEvent.EntityId, weaponComponent.WeaponType);
            animationComponent.Animations.Add(animation);
        }

        // Animation for when the bullet should be deleted.
        public void NewBulletAnimation(GeneralAnimation generalAnimation, uint[] bulletIds,
            MoveComponent shooterMoveComponent, uint shooterId, WeaponComponent.WeaponTypes weaponType)
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
                        EntityManager.AddEntityToDestructionList(bulletIds[i]);
                    }
                    generalAnimation.IsDone = true;
                }
                else if (counter < bulletIds.Length && currentTimeInMilliseconds - lastFired > RateOfFire)
                {
                    lastFired = currentTimeInMilliseconds;
                    BulletFactory.FireBullet(bulletIds[counter++], shooterId, shooterMoveComponent.Direction);
                    FireSound(weaponType);
                }
            };
        }

        public async void FireSound(WeaponComponent.WeaponTypes weaponType)
        {
            await Task.Delay(1);
            EventBus.Publish(EventConstants.FireWeaponSound, weaponType);
        }
    }
}