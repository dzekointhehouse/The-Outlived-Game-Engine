using Microsoft.Xna.Framework;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Spelkonstruktionsprojekt.ZEngine.GameObjects
{
    public class LampFactory
    {
        public uint TurnedOnFlickering(int scale, float intensity, float radius)
        {   
            var light = new PointLight()
            {
                Scale = new Vector2(scale),
                Radius = radius,
                Intensity = intensity,
                ShadowType = ShadowType.Solid // Will not lit hulls themselves
            };
            var lamp = new EntityBuilder()
                .SetLight(light)
                .SetLightFlickering(30, 2)
                .SetRendering(50, 50)
                .SetSprite("Images/lamp")
                .SetPosition(new Vector2(600, 600), 800)
                .BuildAndReturnId();
            return 0;
        }

        public uint TurnedOffFlickering(int scale, float intensity, float radius)
        {
            var light = new PointLight()
            {
                Scale = new Vector2(scale),
                Radius = radius,
                Intensity = intensity,
                ShadowType = ShadowType.Solid // Will not lit hulls themselves
            };
            var lamp = new EntityBuilder()
                .SetLight(light)
                .SetLightFlickering(3000, 3)
                .SetRendering(50, 50)
                .SetSprite("Images/lamp")
                .SetPosition(new Vector2(600, 600), 800)
                .BuildAndReturnId();
            return 0;

        }

        public uint HullTester()
        {
            return new EntityBuilder()
//                .SetRendering(50, 50)
//                .SetSprite("RedDot")
//                .SetPosition(new Vector2(600, 600), 100)
//                .SetHull()
                .BuildAndReturnId();
        }
    }
}