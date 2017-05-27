using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Penumbra;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class HullComponent : IComponent
    {
        public Hull Hull { get; set; }
        public IComponent Reset()
        {
            Hull = null;
            return this;
        }
    }
}
