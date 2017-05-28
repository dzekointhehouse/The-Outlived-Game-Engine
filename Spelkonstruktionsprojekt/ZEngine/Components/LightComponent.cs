using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Penumbra;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class LightComponent : IComponent
    {
        public bool KillSwitchOn { get; set; } = true;
        public Light Light { get; set; }

        public LightComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            KillSwitchOn = false;
            Light = null;
            return this;
        }
    }
}
