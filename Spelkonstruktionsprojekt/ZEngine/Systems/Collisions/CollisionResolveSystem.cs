using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.Managers;
using static ZEngine.Systems.CollisionEvents;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Debug = System.Diagnostics.Debug;

namespace ZEngine.Systems
{
    class CollisionResolveSystem : ISystem
    {
        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        private const bool PROFILING_COLLISIONS = false;
        public void ResolveCollisions(Dictionary<CollisionRequirement, CollisionEvent> collisionEvents,
            GameTime gameTime)
        {
            Stopwatch timer;
            if (PROFILING_COLLISIONS)
            {
                timer = Stopwatch.StartNew();
            }
//For each collidable entity
            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(CollisionComponent)))
            {
//                var collisions = new List<uint>();
                var collisionComponent = entity.Value as CollisionComponent;
//                collisionComponent.collisions.ForEach(c => collisions.Add(c));

//Check every occured collision
                for (var i = 0; i < collisionComponent.collisions.Count; i++)
                {
                    var collisionTarget = collisionComponent.collisions[i];
                    //If the collision matches any valid collision event
                    foreach (var collisionEvent in collisionEvents)
                    {
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
//                    collisionComponent.collisions.Remove(collisionTarget);
                }
                collisionComponent.collisions.Clear();
                StateManager.TryRemoveState(entity.Key, State.Collided, 0);
            }

            if (PROFILING_COLLISIONS)
            {
                Debug.WriteLine("COLLISION RESOLVE" + timer.ElapsedTicks);
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

        public static Dictionary<CollisionEvent, string> EventNames = new Dictionary<CollisionEvent, string>()
        {
            {CollisionEvent.Wall, EventConstants.WallCollision},
            {CollisionEvent.Bullet, EventConstants.BulletCollision},
            {CollisionEvent.Enemy, EventConstants.EnemyCollision},
            {CollisionEvent.Neutral, "NeutralCollision"},
            {CollisionEvent.Pickup, EventConstants.PickupCollision}
        };

        public static CollisionEvent FromCollisionEventName(string collisionEventName)
        {
            return EventTypes[collisionEventName];
        }

        public static string FromCollisionEventType(CollisionEvent collisionEvent)
        {
            return EventNames[collisionEvent];
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