using Game.Components;
using Microsoft.Xna.Framework;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Game.Entities
{
    public class OutlivedEntityBuilder : EntityBuilder
    {
        public OutlivedEntityBuilder SetCar(uint? driver = null)
        {
            var component = ComponentFactory.NewComponent<CarComponent>();
            component.Driver = driver;
            components.Add(component);
            return this;
        }

        public OutlivedEntityBuilder SetCarLights()
        {
            var component = ComponentFactory.NewComponent<CarLightsComponent>();
            component.LeftLight = new EntityBuilder()
                .SetPosition(new Vector2(500, 500), 899)
                .SetLight(new Spotlight()
                {
                    Enabled = false,
                    Scale = new Vector2(1250f),
                    Radius = (float) 1f,
                    Intensity = (float) 0.95,
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves
                }).BuildAndReturnId();
            component.RightLight = new EntityBuilder()
                .SetPosition(new Vector2(500, 500), 899)
                .SetLight(new Spotlight()
                {
                    Enabled = false,
                    Scale = new Vector2(1250f),
                    Radius = (float) 1f,
                    Intensity = (float) 0.95,
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves
                }).BuildAndReturnId();
            components.Add(component);
            return this;
        }
    }
}