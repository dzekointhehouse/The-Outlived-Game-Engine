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
        public int WaveSize { get; set; } = 2;
        public bool EnemiesDead { get; set; } = true;

        public int WaveSizeIncreaseConstant = 1;
    }
}
