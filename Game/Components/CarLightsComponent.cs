using ZEngine.Components;

namespace Game.Components
{
    public class CarLightsComponent : IComponent
    {
        
        public bool KillSwitchOn { get; set; } = true;
        public uint? LeftLight { get; set; }
        public uint? RightLight { get; set; }
        
        public IComponent Reset()
        {
            KillSwitchOn = false;
            LeftLight = null;
            RightLight = null;
            return this;
        }
    }
}