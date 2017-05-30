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
                .SetLight(new PointLight()
                {
                    Enabled = false,
                    Scale = new Vector2(650f),
                    Radius = (float) 0.0008f,
                    Intensity = (float) 0.5,
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves
                }).BuildAndReturnId();
            component.RightLight = new EntityBuilder()
                .SetLight(new PointLight()
                {
                    Enabled = false,
                    Scale = new Vector2(650f),
                    Radius = (float) 0.0008f,
                    Intensity = (float) 0.5,
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves
                }).BuildAndReturnId();
            components.Add(component);
            return this;
        }
    }
}