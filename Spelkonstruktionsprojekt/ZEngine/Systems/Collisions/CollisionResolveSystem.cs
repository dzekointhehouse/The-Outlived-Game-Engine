using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.Managers;
using static ZEngine.Systems.CollisionEvents;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;
using Spelkonstruktionsprojekt.ZEngine.Managers;

namespace ZEngine.Systems
{
    class CollisionResolveSystem : ISystem
    {
        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void ResolveCollisions(Dictionary<CollisionRequirement, CollisionEvent> collisionEvents,
            GameTime gameTime)
        {
            var collidableEntities = ComponentManager.GetEntitiesWithComponent(typeof(CollisionComponent))
                .Select(pair => new KeyValuePair<uint, IComponent>(pair.Key, pair.Value));

            var nEntriesBefore = 0;
            var nEntriesAfter = 0;
            var nEntriesBetween = 0;

//For each collidable entity
            foreach (var entity in collidableEntities)
            {
                var collisions = new List<uint>();
                var collisionComponent = entity.Value as CollisionComponent;
                collisionComponent.collisions.ForEach(c => collisions.Add(c));

//Check every occured collision
                foreach (uint collisionTarget in collisions)
                {
                    //If the collision matches any valid collision event
                    foreach (var collisionEvent in collisionEvents)
                    {
                        if (collisionEvent.Value == CollisionEvent.Bullet)
                        {
//                            Debug.WriteLine("");
                        }
                        //Collision events are made up from requirement of each party
                        //If both entities (parties) fulfil the component requirements
                        //Then there is a match for a collision event
                        uint movingEntityId = entity.Key;
                        var collisionRequirements = collisionEvent.Key;
                        var collisionEventType = collisionEvent.Value;

//                        Debug.WriteLine("Testing match for " + FromCollisionEventType(collisionEventType));
                        if (MatchesCollisionEvent(collisionRequirements, movingEntityId, (uint) collisionTarget))
                        {
//                            Debug.WriteLine("Matched with " + FromCollisionEventType(collisionEventType));
                            //When there is a match for a collision-event, an event is published
                            // for any system to pickup and resolve
                            var collisionEventTypeName = FromCollisionEventType(collisionEventType);
                            var collisionEventWrapper = new SpecificCollisionEvent()
                            {
                                Entity = (uint) movingEntityId,
                                Target = collisionTarget,
                                Event = collisionEventType,
                                EventTime = gameTime.TotalGameTime.TotalMilliseconds
                            };
                            EventBus.Publish(collisionEventTypeName, collisionEventWrapper);
                        }
                    }
                    collisionComponent.collisions.Remove(collisionTarget);
                }
                StateManager.TryRemoveState(entity.Key, State.Collided, 0);
            }
        }

        private bool MatchesCollisionEvent(CollisionRequirement collisionRequirements, uint movingEntityId,
            uint targetId)
        {
            return ComponentManager.ContainsAllComponents(movingEntityId, collisionRequirements.MovingEntityRequirements)
                && ComponentManager.ContainsAllComponents(targetId, collisionRequirements.TargetEntityRequirements);
        }
    }

    public static class CollisionEvents
    {
        public enum CollisionEvent
        {
            Wall = 0,
            Bullet,
            Enemy,
            Neutral,
            Pickup
        }

        public static Dictionary<string, CollisionEvent> EventTypes = new Dictionary<string, CollisionEvent>()
        {
            {EventConstants.WallCollision, CollisionEvent.Wall},
            {EventConstants.BulletCollision, CollisionEvent.Bullet},
            {EventConstants.EnemyCollision, CollisionEvent.Enemy},
            {"NeutralCollision", CollisionEvent.Neutral},
            {EventConstants.PickupCollision, CollisionEvent.Pickup}
        };

        public static CollisionEvent FromCollisionEventName(string collisionEventName)
        {
            return EventTypes[collisionEventName];
        }

        public static string FromCollisionEventType(CollisionEvent collisionEvent)
        {
            return EventTypes.First(e => e.Value == collisionEvent).Key;
        }
    }

    //Used for passing event data to system responsible for resolving collision
    public class SpecificCollisionEvent
    {
        public uint Entity = default(uint);
        public uint Target = default(uint);
        public CollisionEvent Event = 0;
        public double EventTime = 0;
    }


    //Not currently used, but should possible be used by the CollisionComponent
    public class UnkownCollisionEvent
    {
        public int Entity;
        public int Target;
    }

    //Used for mapping componenet requirements to CollisionEvents
    //This is a preset. The user may setup its own component requirements.
    public class ZEngineCollisionEventPresets
    {
        public static Dictionary<CollisionRequirement, CollisionEvent> StandardCollisionEvents { get; } =
            new Dictionary<CollisionRequirement, CollisionEvent>()
            {
                {
                    new CollisionRequirement
                    {
                        MovingEntityRequirements = new List<Type>(),
                        TargetEntityRequirements = new List<Type>()
                    },
                    CollisionEvent.Wall
                },
                {
                    new CollisionRequirement()
                    {
                        MovingEntityRequirements = new List<Type>()
                        {
                            typeof(PlayerComponent)
                        },
                        TargetEntityRequirements = new List<Type>()
                        {
                            typeof(AIComponent)
                        }
                    },
                    CollisionEvent.Enemy
                },
                {
                    new CollisionRequirement()
                    {
                        MovingEntityRequirements = new List<Type>
                        {
                            typeof(PlayerComponent)
                        },
                        TargetEntityRequirements = new List<Type>
                        {
                            typeof(AmmoPickupComponent)
                        }
                    },
                    CollisionEvent.Pickup
                },
                {
                    new CollisionRequirement()
                    {
                        MovingEntityRequirements = new List<Type>
                        {
                            typeof(PlayerComponent)
                        },
                        TargetEntityRequirements = new List<Type>
                        {
                            typeof(HealthPickupComponent)
                        }
                    },
                    CollisionEvent.Pickup
                },
                {
                    new CollisionRequirement()
                    {
                        MovingEntityRequirements = new List<Type>()
                        {
                            typeof(BulletComponent)
                        },
                        TargetEntityRequirements = new List<Type>()
                    },
                    CollisionEvent.Bullet
                },
            };
    }

    public class CollisionRequirement
    {
        public List<Type> MovingEntityRequirements;
        public List<Type> TargetEntityRequirements;
    }
}