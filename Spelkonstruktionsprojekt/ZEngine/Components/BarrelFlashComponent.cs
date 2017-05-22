using Penumbra;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class BarrelFlashComponent : IComponent
    {

        public Light Light { get; set; }

        public BarrelFlashComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            Light = null;
            return this;
        }

    }
}