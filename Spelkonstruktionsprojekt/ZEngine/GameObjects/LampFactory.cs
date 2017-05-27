using Microsoft.Xna.Framework;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Spelkonstruktionsprojekt.ZEngine.GameObjects
{
    public class LampFactory
    {
        public uint FlickeringLamp()
        {
            var lamp = new EntityBuilder()
                .SetLight(new PointLight())
                .BuildAndReturnId();
            return 0;
        }

        public uint HullTester()
        {
            return new EntityBuilder()
                .SetRendering(50, 50)
                .SetSprite("RedDot")
                .SetPosition(new Vector2(600, 600), 100)
                .SetHull()
                .BuildAndReturnId();
        }
    }
}