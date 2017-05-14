using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;
namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class SpawnComponent : IComponent
    {
        public int WaveSize { get; set; } = 40;
        public int EnemiesLeft { get; set; }

        public int WaveSizeIncreaseConstant = 10;

    }
}
