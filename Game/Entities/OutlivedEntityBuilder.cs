using Game.Components;
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
        
    }
}