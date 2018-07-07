using Spelkonstruktionsprojekt.ZEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    class AiWallCollisionSystem : ISystem, IUpdateables
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
//            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.AiWallCollision, Update);
//            EventBus.Subscribe<CloseEncounterEvent>(EventConstants.AiCloseEncounter, HandleCloseEncounter);
        }

        public void HandleCloseEncounter(CloseEncounterEvent closeEncounter)
        {
            AIComponent aiComponent = ComponentManager.GetEntityComponentOrDefault<AIComponent>(closeEncounter.Entity);
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(closeEncounter.Entity);

            var positiveDirection = moveComponent.Direction - moveComponent.PreviousDirection;
            var negativeDirection = MathHelper.TwoPi - moveComponent.Direction + moveComponent.PreviousDirection;
            aiComponent.TimeOfLastWallCollision = closeEncounter.EventTime;
            if (positiveDirection < negativeDirection)
            {
                moveComponent.Direction += (float) (Math.PI * 0.01);
            }
        }
        /*
         * Target is the pickup, entity is the player
         * This function determines what kind of pickup type it is, and calls the appropriate method.
         * First it checks if it has already been used(has the delete tag), in case 2 players touched it in the same frame.
         */
        public void Handle(SpecificCollisionEvent collisionEvent)
        {

            AIComponent aiComponent = ComponentManager.GetEntityComponentOrDefault<AIComponent>(collisionEvent.Entity);
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(collisionEvent.Entity);

//            var directionDiff = moveComponent.Direction - moveComponent.PreviousDirection;

            //if(moveComponent.Direction > moveComponent.PreviousDirection)
            var positiveDirection = moveComponent.Direction - moveComponent.PreviousDirection;
            var negativeDirection = MathHelper.TwoPi - moveComponent.Direction + moveComponent.PreviousDirection;
            if (positiveDirection < negativeDirection)
            {
                moveComponent.Direction += (float) (Math.PI * 0.01);
            }
            else
            {
                moveComponent.Direction -= (float) (Math.PI * 0.01);
            }
//            moveComponent.Direction += (float)(Math.PI * 0.01);
//            moveComponent.PreviousDirection += (float)(Math.PI * 0.01);
            aiComponent.TimeOfLastWallCollision = collisionEvent.EventTime;
        }

    }
}
