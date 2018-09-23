

using ZEngine.Components;

namespace Game.Components
{
    public class CarComponent : IComponent
    {
        public uint? Driver { get; set; }
        
        public IComponent Reset()
        {
            Driver = null;
            return this;
        }
    }
}