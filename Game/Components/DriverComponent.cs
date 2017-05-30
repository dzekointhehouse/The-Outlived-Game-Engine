using ZEngine.Components;

namespace Game.Components
{
    public class DriverComponent : IComponent
    {
        public uint Car { get; set; }
        
        public IComponent Reset()
        {
            return this;
        }
    }
}