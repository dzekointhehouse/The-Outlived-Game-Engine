using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    public class EventZoneCollisionSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.EventZoneCollision, Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            var eventZone = ComponentManager.GetEntityComponentOrDefault<EventZoneComponent>(collisionEvent.Target);
            if (eventZone == null) return;
            
            foreach (var eventString in eventZone.Events)
            {
                EventBus.Publish(eventString, new ZoneEvent
                {
                    EventTime = collisionEvent.EventTime,
                    Player = collisionEvent.Entity,
                    Zone = collisionEvent.Target
                });
            }
        }

        private class ZoneEvent
        {
            public double EventTime { get; set; }
            public uint Player { get; set; }
            public uint Zone { get; set; }
        }
    }
}