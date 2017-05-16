using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;
namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class GlobalSpawnComponent : IComponent
    {
        bool FirstGameRound { get; set; } = true;
        int Wavesize { get; set; }
    }
}
