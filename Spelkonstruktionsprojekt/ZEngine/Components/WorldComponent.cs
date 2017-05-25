using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class WorldComponent : IComponent
    {
        public int[,] World { get; set; }
        public int WorldHeight { get; set; }
        public int WorldWidth { get; set; }
        public IComponent Reset()
        {
            return this;
        }
    }
}
