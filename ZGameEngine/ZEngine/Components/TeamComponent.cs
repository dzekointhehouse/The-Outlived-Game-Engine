using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class TeamComponent : IComponent
    {
        public int TeamId { get; set; }
        public List<int> members { get; set; } = new List<int>();
        public IComponent Reset()
        {
            TeamId = 0;
            members.Clear();
            return this;
        }
    }
}
