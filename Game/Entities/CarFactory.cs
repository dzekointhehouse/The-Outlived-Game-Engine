using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Game.Entities
{
    public class CarFactory
    {
        public static uint CreatePickupsWithPenumbraLights()
        {
            return new OutlivedEntityBuilder()
                .SetCar()
                .SetCarLights()
                .SetPosition(new Vector2(500, 900), ZIndexConstants.Car)
                .SetRectangleCollision()
                .SetMovement(500, 100, (float) 0.5, 0)
                .SetRendering(250, 140)
                .SetSprite("car_black")
                .BuildAndReturnId();
        }
        public static uint CreatePickup()
        {
            return new OutlivedEntityBuilder()
                .SetCar()
                .SetPosition(new Vector2(500, 900), ZIndexConstants.Car)
                .SetRectangleCollision()
                .SetMovement(500, 100, (float) 0.5, 0)
                .SetRendering(250, 140)
                .SetInertiaDampening(100)
                .SetSprite("car_black")
                .BuildAndReturnId();
        }
    }
}