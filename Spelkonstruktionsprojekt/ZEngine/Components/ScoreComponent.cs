using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class ScoreComponent : IComponent
    {
        public int score { get; set; } = 0;
    }
}
