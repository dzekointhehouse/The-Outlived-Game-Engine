using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Game.Systems
{
    // Optimus prime
    public class SoundSystem : ISystem
    {
        private readonly EventBus EventBus = EventBus.Instance;
        private AudioEmitter emitter = new AudioEmitter();
        private AudioListener listener = new AudioListener();

        public ISystem Start()
        {
            // Here we subscribe what will happen when the entity walks forwards
            // We'll use WalkingSounds to handle it.
            // EventBus.Subscribe<InputEvent>(EventConstants.WalkForward, WalkingSounds);
            // EventBus.Subscribe<InputEvent>(EventConstants.WalkBackward, WalkingSounds);

            // We subscribe to the input inputEvent for when the entity fires a
            // weapon, then we use the WeaponSounds method to "say" what should
            // be done. 
            EventBus.Subscribe<InputEvent>(EventConstants.FirePistolWeapon, WeaponSounds);
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.PickupCollision, PickupSounds);
            //EventBus.Subscribe<StateChangeEvent>("StateChanged", WalkingSounds);
            return this;
        }

        public ISystem Stop()
        {
            return this;
        }

        // We want to subscribe this method to events where a entity fires
        // a weapon. We check if the bullet fire key has been pressed, then we
        // fire the sound.
        private void WeaponSounds(InputEvent inputEvent)
        {
            
            // First things first, we only handle this event if the key is pressed
            // that is associated with this event "entityFireWeapon"
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var ammoComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<AmmoComponent>(inputEvent.EntityId);
                // No ammo no sound
                if (ammoComponent == null) return;
                if (ammoComponent.Amount == 0) return;

                // To play the bullet sound the entity needs to have
                // the BulletFlyweightComponent
                //var bulletFlyweightComponent =
                //    ComponentManager.Instance.GetEntitiesWithComponent(typeof(BulletFlyweightComponent));
                //if (bulletFlyweightComponent.Count <= 0) return;

                var weaponComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<WeaponComponent>(inputEvent.EntityId) as WeaponComponent;
                if (weaponComponent == null) return;


                SoundEffectInstance soundEffectInstance = OutlivedGame.Instance().Content.Load<SoundEffect>("Sound/Weapon/m4a1_fire").CreateInstance();
                if (weaponComponent.WeaponType == WeaponComponent.WeaponTypes.Pistol)
                {
                    soundEffectInstance = OutlivedGame.Instance().Content.Load<SoundEffect>("Sound/Weapon/gun_fire").CreateInstance();
                    soundEffectInstance.IsLooped = false;
                }
                if (weaponComponent.WeaponType == WeaponComponent.WeaponTypes.Shotgun)
                {
                    soundEffectInstance = OutlivedGame.Instance().Content.Load<SoundEffect>("Sound/Weapon/shotgun_fire").CreateInstance();
                    soundEffectInstance.IsLooped = false;
                }
                if (weaponComponent.WeaponType == WeaponComponent.WeaponTypes.Rifle)
                {
                    soundEffectInstance.IsLooped = false;
                    soundEffectInstance = OutlivedGame.Instance().Content.Load<SoundEffect>("Sound/Weapon/m4a1_fire").CreateInstance();
                }

                    // Get the sound instance for this entity
                    //var sound =
                    //    ComponentManager.Instance.GetEntityComponentOrDefault<SoundComponent>(bulletFlyweightComponent
                    //        .First()
                    //        .Key);

                    // We create a SoundEffectInstance which gives us more control
                    //var soundInstance = sound.SoundEffect.CreateInstance();


                    var positionComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(inputEvent.EntityId);
                
                // it's logical that the sound comes from the weapon which is the source of the
                // sound and the location where the sound will be the strongest.
                emitter.Position = new Vector3(positionComponent.Position, positionComponent.ZIndex);
                listener.Position = new Vector3(positionComponent.Position, positionComponent.ZIndex);

                // Applying the 3d sound effect to the sound instance
                soundEffectInstance.Apply3D(listener, emitter);

                // If it is not already playing then play it
                if (soundEffectInstance.State != SoundState.Playing)
                {
                    //soundEffectInstance.IsLooped = false;
                    soundEffectInstance.Play();
                }
            }
        }

        private void PickupSounds(SpecificCollisionEvent inputEvent)
        {
                // We get the entities pickup components, we don't know which
                // so we try them all.
                var pickup =
                    ComponentManager.Instance.GetEntityComponentOrDefault<AmmoPickupComponent>(inputEvent.Target);
                var health =
                    ComponentManager.Instance.GetEntityComponentOrDefault<HealthPickupComponent>(inputEvent.Target);
                // Atlest one pickup needs to exist.
                if (health == null && pickup == null) return;

            //var globalPickUpEntity =
            //   ComponentManager.Instance.GetEntitiesWithComponent(typeof(FlyweightPickupComponent));
            //if (globalPickUpEntity.Count <= 0) return;


            // Get the sound instance for this entity
                    //ComponentManager.Instance.GetEntityComponentOrDefault<SoundComponent>(globalPickUpEntity.First().Key);
            var sound =
                    ComponentManager.Instance.GetEntityComponentOrDefault<SoundComponent>(inputEvent.Target);
                if (sound == null) return;
                // We create a SoundEffectInstance which gives us more control
                var soundInstance = sound.SoundEffect.CreateInstance();

                // If it is not already playing then play it
                if (soundInstance.State != SoundState.Playing)
                {
                    soundInstance.IsLooped = false;
                    soundInstance.Play();
                }
        }


        // This method is supposed to be used to subscribe to events
        // where the entity is walking. We check if a key is pressed,
        // then we check if the entity has the right components.
        public void WalkingSounds(StateChangeEvent stateChangeEvent)
        {
            var entityId = stateChangeEvent.EntityId;
            if (!stateChangeEvent.NewState.Contains(State.WalkingForward)) return;
            var moveComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(entityId);

            var soundComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<SoundComponent>(entityId);
            if (soundComponent == null) return;

            var animationComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<AnimationComponent>(entityId);
            if (animationComponent == null)
            {
                animationComponent = ComponentManager.Instance.ComponentFactory.NewComponent<AnimationComponent>();
                ComponentManager.Instance.AddComponentToEntity(animationComponent, entityId);
            }

            var animation = new GeneralAnimation()
            {
                StartOfAnimation = stateChangeEvent.EventTime,
                Length = 500,
                Unique = true
            };

            var sound = soundComponent.SoundEffect.CreateInstance();

            if (sound.State != SoundState.Playing)
                sound.Play();

            var animationAction = NewWalkingSoundAnimation(animation, sound, moveComponent);
            animation.Animation = animationAction;
            animationComponent.Animations.Add(animation);
        }

        // This one is fascinating. We add this to our entitys animation component instance which
        // contains a list of GeneralAnimations, this method is the action that is stored in the
        // general animation. This action will then be performed in another system and can do it's
        // own stuff independently of this system later on. Good for when we maybe trigger something
        // that will go on for a while until a set tim
        public Action<double> NewWalkingSoundAnimation(GeneralAnimation animation, SoundEffectInstance sound,
            MoveComponent moveComponent)
        {
            return delegate (double currentTime)
            {
                var elapsedTime = currentTime - animation.StartOfAnimation;
                if (elapsedTime > animation.Length && moveComponent.Speed > -5 && moveComponent.Speed < 5)
                {
                    sound.Stop();
                    animation.IsDone = true;
                }
            };
        }
    }
}