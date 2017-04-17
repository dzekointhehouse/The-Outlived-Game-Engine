using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.Xna.Framework.Audio;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class SoundSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;
        public void Update()
        {
            EventBus.Subscribe<MoveEvent>("entityWalkForwards", WalkingSounds);


        }

        public void WalkingSounds(MoveEvent moveEvent)
        {
            Debug.WriteLine("In sound system");
            var soundComponent = ComponentManager.GetEntityComponentOrDefault<SoundComponent>(moveEvent.EntityId);
            if (soundComponent == default(SoundComponent)) return;

            var entityId = moveEvent.EntityId;
            if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var isLooped = true;
                var lengthInSeconds = 1;
                var animation = new AnimationComponent()
                {
                    LenghtInSeconds = lengthInSeconds,
                    Animation = NewWalkingSoundAnimation(soundComponent, isLooped),
                    Loop = isLooped
                };
                if (!ComponentManager.EntityHasComponent<AnimationComponent>(entityId))
                {
                    ComponentManager.AddComponentToEntity(animation, entityId);
                    Debug.WriteLine("Added sound animation");

                }
            }
        }
        
        public Action<double> NewWalkingSoundAnimation(SoundComponent soundComponent, bool isLooped)
        {
            SoundEffectInstance sound = soundComponent.SoundInstace;
            return delegate(double elapsedTime)
            {
                if (sound.State == SoundState.Playing) return;
                if (isLooped) sound.IsLooped = true;

                sound.Play();
                Debug.WriteLine("Played sound");
            };
        }
    }

}
