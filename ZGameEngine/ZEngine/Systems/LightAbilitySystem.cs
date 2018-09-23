using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class LightAbilitySystem : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        public void Update(GameTime gt)
        {
        }
        private readonly EventBus EventBus = EventBus.Instance;

        public ISystem Start()
        {
            // Subscribe to the Lightstatus event, and set the method that will
            // handle it.
            EventBus.Subscribe<InputEvent>(EventConstants.LightStatus, HandleLightStatus);
            return this;
        }
        public ISystem Stop()
        {
            return this;
        }

        // Every Entity that has an LightStatus actionbinding to its lightComponent is able to
        // turn it's light on or off. /ELVIR: I don't think we need to fade the light so don't touch
        // without asking.
        private void HandleLightStatus(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var lightComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(inputEvent.EntityId);
                if (lightComponent == null) return;
                if (lightComponent.KillSwitchOn) return;

                lightComponent.Light.Enabled = lightComponent.Light.Enabled == false;
            }
        }


    }
}
