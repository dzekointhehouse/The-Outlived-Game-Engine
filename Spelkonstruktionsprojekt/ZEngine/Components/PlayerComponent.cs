using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class PlayerComponent : IComponent
    {
        public string Name { get; set; }
        public bool isHuman { get; set; }
    }
}
