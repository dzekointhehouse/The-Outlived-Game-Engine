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
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class SoundSystem : ISystem
    {
        private readonly EventBus EventBus = EventBus.Instance;

        public ISystem Start()
        {
            // Here we subscribe what will happen when the entity walks forwards
            // We'll use WalkingSounds to handle it.
            EventBus.Subscribe<InputEvent>(EventConstants.WalkForward, WalkingSounds);
            EventBus.Subscribe<InputEvent>(EventConstants.WalkBackward, WalkingSounds);

            // We subscribe to the input inputEvent for when the entity fires a
            // weapon, then we use the WeaponSounds method to "say" what should
            // be done. 
            EventBus.Subscribe<InputEvent>(EventConstants.FireWeapon, WeaponSounds);
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
                // To play the bullet sound the entity needs to have
                // the BulletFlyweightComponent
                var bulletSpriteEntities =
                    ComponentManager.Instance.GetEntitiesWithComponent<BulletFlyweightComponent>();
                if (bulletSpriteEntities.Count <= 0) return;

                // Get the sound instance for this entity
                var sound = 
                    ComponentManager.Instance.GetEntityComponentOrDefault<SoundComponent>(bulletSpriteEntities.First().Key);

                // We create a SoundEffectInstance which gives us more control
                var soundInstance = sound.SoundEffect.CreateInstance();

                // If it is not already playing then play it
                if (soundInstance.State != SoundState.Playing)
                {
                    soundInstance.IsLooped = false;
                    soundInstance.Play();
                }
            }
        }


        // This method is supposed to be used to subscribe to events
        // where the entity is walking. We check if a key is pressed,
        // then we check if the entity has the right components.
        public void WalkingSounds(InputEvent moveEvent)
        {
            if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                // We need to have the sound component to play the sound
                var soundComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<SoundComponent>(moveEvent.EntityId);

                if (!ComponentManager.Instance.EntityHasComponent<AnimationComponent>(moveEvent.EntityId))
                {
                    var animationComponent = new AnimationComponent();

                    ComponentManager.Instance.AddComponentToEntity(animationComponent, moveEvent.EntityId);

                    var animation = new GeneralAnimation()
                    {
                        StartOfAnimation = moveEvent.EventTime,
                        Length = 2000
                    };
                    
                    var sound = soundComponent.SoundEffect.CreateInstance();

                    var animationAction = NewWalkingSoundAnimation(sound, moveEvent.EntityId);
                    animation.Animation = animationAction;

                    animationComponent.Animations.Add(animation);
                }
            }
        }

        // This one is fascinating. We add this to our entitys animation component instance which
        // contains a list of GeneralAnimations, this method is the action that is stored in the
        // general animation. This action will then be performed in another system and can do it's
        // own stuff independently of this system later on. Good for when we maybe trigger something
        // that will go on for a while until a set tim
        public Action<double> NewWalkingSoundAnimation(SoundEffectInstance sound, int entityId)
        {
            return delegate (double elapsedTime)
            {
                var soundComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(entityId);
                if (soundComponent.Speed <= 0 && sound.State == SoundState.Playing)
                    sound.Stop();
                if (sound.State == SoundState.Playing) return;
               // if (isLooped) sound.IsLooped = true;
               System.Diagnostics.Debug.WriteLine("yes yes");
                sound.Play();
            };
        }
    }
}

