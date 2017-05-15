using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
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
    class ReloadSystem : ISystem
    {
        private EventBus eventBus = EventBus.Instance;

        public void Start()
        {
            eventBus.Subscribe<InputEvent>(EventConstants.ReloadWeapon, Handle);
        }

        private void Handle(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) { return; }
            Debug.WriteLine(inputEvent.EntityId + ": Reloading!");
            var ammoComponent = (AmmoComponent) ComponentManager.Instance.GetEntityComponentOrDefault(typeof(AmmoComponent), inputEvent.EntityId);
            var weaponComponent = (WeaponComponent)ComponentManager.Instance.GetEntityComponentOrDefault(typeof(WeaponComponent), inputEvent.EntityId);
            if (ammoComponent == null || weaponComponent == null) { return; }

            int ammoToBeAdded = weaponComponent.ClipSize - ammoComponent.Amount;
            if (ammoToBeAdded >= ammoComponent.SpareAmmoAmount)
            {
                ammoComponent.Amount += ammoComponent.SpareAmmoAmount;
                ammoComponent.SpareAmmoAmount = 0;
            }
            else
            {
                ammoComponent.Amount += ammoToBeAdded;
                ammoComponent.SpareAmmoAmount -= ammoToBeAdded;
            }
        }
    }
}
