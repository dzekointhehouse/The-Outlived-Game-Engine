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
using Penumbra;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class ReloadSystem : ISystem
    {
        private EventBus eventBus = EventBus.Instance;
        private int timeToReload = 200;
        public void Start()
        {
            eventBus.Subscribe<InputEvent>(EventConstants.ReloadWeapon, Handle);
        }

        private void Handle(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) { return; }
            //Debug.WriteLine(inputEvent.EntityId + ": Reloading!");
            var ammoComponent = ComponentManager.Instance.GetEntityComponentOrDefault<AmmoComponent>(inputEvent.EntityId);
            var weaponComponent = ComponentManager.Instance.GetEntityComponentOrDefault<WeaponComponent>(inputEvent.EntityId);
            if (ammoComponent == null || weaponComponent == null) { return; } 
            if (weaponComponent.ClipSize == ammoComponent.Amount) { return; } // Return if clip is full
            if (ammoComponent.SpareAmmoAmount == 0)
            {
                eventBus.Publish("voice_noAmmoForReload", inputEvent.EntityId);
                return;
            }

            eventBus.Publish(EventConstants.ReloadWeaponSound, inputEvent.EntityId);
            Reload(ammoComponent, weaponComponent);
        }

        public async void Reload(AmmoComponent ammoComponent, WeaponComponent weaponComponent)
        {
            await Task.Delay(timeToReload);

            int ammoToBeAdded = weaponComponent.ClipSize - ammoComponent.Amount;

            if (ammoToBeAdded >= ammoComponent.SpareAmmoAmount) // Check to see if they don't have enough ammo for a full reload
            {
                ammoComponent.Amount += ammoComponent.SpareAmmoAmount;
                ammoComponent.SpareAmmoAmount = 0;
            }
            else //full reload
            {
                ammoComponent.Amount += ammoToBeAdded;
                ammoComponent.SpareAmmoAmount -= ammoToBeAdded;
            }
        }
    }
}
