using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class SoundSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;

        public ISystem Start()
        {
            // Here we subscribe what will happen when the entity walks forwards
            // We'll use WalkingSounds to handle it.
            EventBus.Subscribe<MoveEvent>("entityWalkForwards", WalkingSounds);
            EventBus.Subscribe<MoveEvent>("entityWalkBackwards", WalkingSounds);

            // We subscribe to the input inputEvent for when the entity fires a
            // weapon, then we use the WeaponSounds method to "say" what should
            // be done. 
            EventBus.Subscribe<InputEvent>("entityFireWeapon", WeaponSounds);
            return this;
        }
        public ISystem Stop()
        {
            return this;
        }

        private void WeaponSounds(InputEvent inputEvent)
        {
            // First things first, we only handle this event if the key is pressed
            // that is associated with this event "entityFireWeapon"
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                // To play the bullet sound the entity needs to have
                // the BulletFlyweightComponent
                var bulletSpriteEntities =
                    ComponentManager.Instance.GetEntitiesWithComponent<BulletFlyweightComponent>();
                if (bulletSpriteEntities.Count <= 0) return;

                // Get the sound instance for this entity
                var sound = 
                    ComponentManager.Instance.GetEntityComponentOrDefault<SoundComponent>(bulletSpriteEntities.First().Key);

                var soundInstance = sound.SoundEffect.CreateInstance();

                if (soundInstance.State != SoundState.Playing)
                {
                    soundInstance.IsLooped = false;
                    soundInstance.Play();
                }
            }
        }

        public void WalkingSounds(MoveEvent moveEvent)
        {
            if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var moveComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(moveEvent.EntityId);

                var soundComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<SoundComponent>(moveEvent.EntityId);

                var animationComponent = new AnimationComponent();
                ComponentManager.Instance.AddComponentToEntity(animationComponent, moveEvent.EntityId);

                var animation = new GeneralAnimation()
                {
                    StartOfAnimation = moveEvent.CurrentTimeMilliseconds,
                    Length = 2000
                };
                var animationAction = NewWalkingSoundAnimation(soundComponent, moveComponent);
                animation.Animation = animationAction;

                animationComponent.Animations.Add(animation);
            }
        }

        public Action<double> NewWalkingSoundAnimation(SoundComponent soundComponent, MoveComponent moveComponent)
        {
            var sound = soundComponent.SoundEffect.CreateInstance();

            return delegate (double elapsedTime)
            {
                if (sound.State == SoundState.Playing) return;
               // if (isLooped) sound.IsLooped = true;

                sound.Play();
            };
        }
    }
}

