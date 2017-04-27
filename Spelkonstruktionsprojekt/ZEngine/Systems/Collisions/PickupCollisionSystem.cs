using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    internal class PickupCollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;
        private readonly EventBus EventBus = EventBus.Instance;


        //Pickup Values, should be moved to components later
        private int HealingAmount = 50;


        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.PickupCollision, Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            var pickupComponent = (PickupComponent)ComponentManager.GetEntityComponentOrDefault(typeof(PickupComponent), collisionEvent.Entity);

            if (pickupComponent.PickupType == EventConstants.HealthPickup)
            {
                HandleHealthPickup(collisionEvent.Target, collisionEvent.Entity);
            }

        }

        private void HandleHealthPickup(int player, int pickup)
        {
            var HealthComponent = (HealthComponent)ComponentManager.GetEntityComponentOrDefault(typeof(HealthComponent), pickup);
            if (HealthComponent.CurrentHealth < HealthComponent.MaxHealth)
            {
                HealthComponent.Damage.Add(-HealingAmount);
                ComponentManager.DeleteEntity(pickup);
            }


            /*
            if (HealthComponent.MaxHealth < (HealthComponent.CurrentHealth += HealingAmount))
            { 
                HealthComponent.CurrentHealth = HealthComponent.MaxHealth;
                //Remove Component
                ComponentManager.DeleteEntity(pickup);
            }
            */




        }
    }
}

