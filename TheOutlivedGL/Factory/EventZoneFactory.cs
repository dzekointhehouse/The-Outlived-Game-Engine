using System.Diagnostics.Eventing.Reader;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Game.Factory
{
    public class EventZoneFactory
    {
        public static uint EventZone(Rectangle zone, params string[] events)
        {
            return new EntityBuilder()
                .SetCollision()
                .SetPosition(new Vector2(zone.X, zone.Y), 0)
                .SetDimensions(zone.Width, zone.Height)
                .SetZone(events)
                .BuildAndReturnId();
        }
    }
}