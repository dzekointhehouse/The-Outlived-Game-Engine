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
        public void Start()
        {
            // Here we subscribe what will happen when the entity walks forwards
            // We'll use WalkingSounds to handle it.
            EventBus.Subscribe<MoveEvent>("entityWalkForwards", WalkingSounds);
            EventBus.Subscribe<MoveEvent>("entityWalkBackwards", WalkingSounds);
        }

        public void WalkingSounds(MoveEvent moveEvent)
        {
            var soundComponent = ComponentManager.GetEntityComponentOrDefault<SoundComponent>(moveEvent.EntityId);
            if (soundComponent == default(SoundComponent)) return;

            soundComponent.SoundInstace.Volume = soundComponent.Volume;

            if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                soundComponent.SoundInstace.Play();
            } else if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyReleased)
            {
                // REMEMBER set pan value depending on X position.
                soundComponent.SoundInstace.Stop();

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
            };
        }
    }

}
