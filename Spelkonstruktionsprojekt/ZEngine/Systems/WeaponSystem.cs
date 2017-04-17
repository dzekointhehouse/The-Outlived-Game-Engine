using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class WeaponSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public ISystem Start()
        {
            EventBus.Subscribe<InputEvent>("entityFireWeapon", HandleFireWeapon);
            return this;
        }

        public ISystem Stop()
        {
            return this;
        }

        public void HandleFireWeapon(InputEvent shootEvent)
        {
            if (shootEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                Debug.WriteLine("Say hello to my little friend");
            }
        }

    }
}
