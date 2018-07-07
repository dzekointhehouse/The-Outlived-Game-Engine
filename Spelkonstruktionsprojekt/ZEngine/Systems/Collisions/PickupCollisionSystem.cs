using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    internal class PickupCollisionSystem : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        public void Update(GameTime gt)
        {
        }
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;
        private readonly EventBus EventBus = EventBus.Instance;
        //Pickup Values, should be moved to components later
        //private int HealingAmount = 50;
        //private int AmmoAmount = 10;


        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.PickupCollision, Handle);
        }
        

        /*
         * Target is the pickup, entity is the player
         * This function determines what kind of pickup type it is, and calls the appropriate method.
         * First it checks if it has already been used(has the delete tag), in case 2 players touched it in the same frame.
         */
        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            if (ComponentManager.GetEntityComponentOrDefault<TagComponent>(collisionEvent.Target).Tags.Contains(Tag.Delete))
            {
                return;
            }
            if (ComponentManager.EntityHasComponent(typeof(HealthPickupComponent), collisionEvent.Target))
            {
                HandleHealthPickup(collisionEvent.Entity, collisionEvent.Target);
            }
            else if (ComponentManager.EntityHasComponent(typeof(AmmoPickupComponent), collisionEvent.Target))
            {
                HandleAmmoPickup(collisionEvent.Entity, collisionEvent.Target);
            }

        }

        private void HandleHealthPickup(uint player, uint pickup)
        {
            var HealthComponent = ComponentManager.GetEntityComponentOrDefault<HealthComponent>(player);
            var HealthPickupComponent = ComponentManager.GetEntityComponentOrDefault<HealthPickupComponent>(pickup);
            //if (HealthComponent.CurrentHealth < HealthComponent.MaxHealth)
            {
                HealthComponent.Damage.Add(-HealthPickupComponent.Amount);
                DeletePickup(pickup);
            }
        }


        private void HandleAmmoPickup(uint player, uint pickup)
        {
            var AmmoComponent = ComponentManager.GetEntityComponentOrDefault<AmmoComponent>(player);
            var AmmoPickupComponent = ComponentManager.GetEntityComponentOrDefault<AmmoPickupComponent>(pickup);
            AmmoComponent.SpareAmmoAmount += AmmoPickupComponent.Amount;
            DeletePickup(pickup);
        }

        /*
         This function tags the pickup for deletion. 
         Should only be called when the pickup was actually used up.
        */
        private void DeletePickup(uint pickup)
        {
            EntityManager.AddEntityToDestructionList(pickup);
        }

    }
}

