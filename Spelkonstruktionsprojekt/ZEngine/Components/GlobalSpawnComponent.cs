using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;
namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class GlobalSpawnComponent : IComponent
    {
        public int WaveSize { get; set; } = 50;
        public bool EnemiesDead { get; set; } = true;
        public int MaxLimitWaveSize { get; set; } = 500;

        public int WaveSizeIncreaseConstant = 20;

        public GlobalSpawnComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            WaveSize = 50;
            EnemiesDead = true;
            WaveSizeIncreaseConstant = 1;
            return this;
        }
    }
}
