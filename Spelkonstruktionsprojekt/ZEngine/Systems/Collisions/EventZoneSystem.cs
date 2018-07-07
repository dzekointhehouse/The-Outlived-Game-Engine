using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    public class EventZoneSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void Update(GameTime gameTime)
        {
            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(EventZoneComponent)))
            {
                var eventZone = entity.Value as EventZoneComponent;

                foreach (var player in eventZone.NewInhabitants)
                {
                    foreach (var eventString in eventZone.Events)
                    {
                        EventBus.Publish(eventString, new ZoneEvent
                        {
                            EventTime = gameTime.ElapsedGameTime.TotalSeconds,
                            Player = player,
                            Zone = entity.Key
                        });
                    }
                }
                eventZone.NewInhabitants.Clear();
            }
        }

        public class ZoneEvent
        {
            public double EventTime { get; set; }
            public uint Player { get; set; }
            public uint Zone { get; set; }
        }
    }
}