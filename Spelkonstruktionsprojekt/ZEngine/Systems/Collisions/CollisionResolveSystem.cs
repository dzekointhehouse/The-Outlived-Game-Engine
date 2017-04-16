using System;
using System.Collections.Generic;
using System.Linq;
using ZEngine.Components;
using ZEngine.Managers;
using static ZEngine.Systems.CollisionEvents;

namespace ZEngine.Systems
{
    class CollisionResolveSystem : ISystem
    {
        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void ResolveCollisions(Dictionary<CollisionRequirement, CollisionEvent> collisionEvents)
        {
            var collidableEntities = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();

            //For each collidable entity
            foreach (var entity in collidableEntities)
            {
                var collisions = new List<int>();
                entity.Value.collisions.ForEach(c => collisions.Add(c));

                //Check every occured collision
                foreach (var collisionTarget in collisions)
                {
                    //If the collision matches any valid collision event
                    foreach (var collisionEvent in collisionEvents)
                    {
                        //Collision events are made up from requirement of each party
                        //If both entities (parties) fulfil the component requirements
                        //Then there is a match for a collision event
                        int movingEntityId = entity.Key;
                        var collisionRequirements = collisionEvent.Key;
                        var collisionEventType = collisionEvent.Value;

                        if (MatchesCollisionEvent(collisionRequirements, movingEntityId, collisionTarget))
                        {
                            //When there is a match for a collision-event, an event is published
                            // for any system to pickup and resolve
                            var collisionEventTypeName = FromCollisionEventType(collisionEventType);
                            var collisionEventWrapper = new SpecificCollisionEvent(movingEntityId, collisionTarget, collisionEventType);
                            EventBus.Publish(collisionEventTypeName, collisionEventWrapper);
                        }
                    }
                }
            }
        }

        private bool MatchesCollisionEvent(CollisionRequirement collisionRequirements, int movingEntityId, int targetId)
        {
            return collisionRequirements.MovingEntityRequirements
                       .Count(componentType => ComponentManager.EntityHasComponent(componentType, movingEntityId))
                            == collisionRequirements.MovingEntityRequirements.Count
                   && collisionRequirements.TargetEntityRequirements
                       .Count(componentType => ComponentManager.EntityHasComponent(componentType, targetId))
                            == collisionRequirements.TargetEntityRequirements.Count;
        }
    }

    public static class CollisionEvents
    {
        public enum CollisionEvent
        {
            Wall = 0,
            Bullet,
            Enemy,
            Neutral
        }

        public static Dictionary<string, CollisionEvent> EventTypes = new Dictionary<string, CollisionEvent>()
            {
                { "WallCollision", CollisionEvent.Wall },
                { "BulletCollision", CollisionEvent.Bullet },
                { "EnemyCollision", CollisionEvent.Enemy },
                { "NeutralCollision", CollisionEvent.Neutral },
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
        public int Entity;
        public int Target;
        public CollisionEvent Event;

        public SpecificCollisionEvent(int entity, int target, CollisionEvent collisionEvent)
        {
            Entity = entity;
            Target = target;
            Event = collisionEvent;
        }
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
        public static Dictionary<CollisionRequirement, CollisionEvent> StandardCollisionEvents { get; } = new Dictionary<CollisionRequirement, CollisionEvent>()
            {
                {
                    new CollisionRequirement()
                    {
                        MovingEntityRequirements = new List<Type>()
                        {
                            typeof(MoveComponent)
                        },
                        TargetEntityRequirements = new List<Type>()
                        {
                            typeof(CollisionComponent)
                        }
                    },
                    CollisionEvent.Wall
                },
                {
                    new CollisionRequirement()
                    {
                        MovingEntityRequirements = new List<Type>()
                        {
                            typeof(MoveComponent)
                        },
                        TargetEntityRequirements = new List<Type>()
                        {
                            typeof(CollisionComponent),
                            typeof(MoveComponent),
                            typeof(AIComponent)
                        }
                    },
                    CollisionEvent.Enemy
                },
            };
    }

    public class CollisionRequirement
    {
        public List<Type> MovingEntityRequirements;
        public List<Type> TargetEntityRequirements;
    }
}
