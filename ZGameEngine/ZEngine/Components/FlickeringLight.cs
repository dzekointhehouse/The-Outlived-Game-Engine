using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class FlickeringLight : IComponent
    {
        public double FlickerSpeed = 100;
        public double FlickerChance = 0.1;
        public double MillisecondsSinceLastOn = 0;
        
        public IComponent Reset()
        {
            MillisecondsSinceLastOn = 0;
            FlickerChance = 0.1;
            MillisecondsSinceLastOn = 0;
            return this;
        }
    }
}