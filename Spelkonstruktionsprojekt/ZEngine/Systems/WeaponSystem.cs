using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            EventBus.Subscribe<ShootEvent>("entityFireWeapon", HandleFireWeapon);
            return this;
        }

        public ISystem Stop()
        {
            return this;
        }

        public void HandleFireWeapon(ShootEvent shootEvent)
        {
            if (shootEvent.KeyEvent == ActionBindings.KeyEvent.KeyDown)
            {
                Debug.WriteLine("Say hello to my little friend");
            }
        }

    }





    public class ShootEvent
    {
        public int EntityId { get; set; }
        public ActionBindings.KeyEvent KeyEvent { get; set; }

        public ShootEvent(int entityId, ActionBindings.KeyEvent keyEvent)
        {
            EntityId = entityId;
            KeyEvent = keyEvent;
        }
    }
}
