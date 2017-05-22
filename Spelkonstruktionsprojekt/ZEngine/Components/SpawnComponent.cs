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
        /// <summary>
        /// DEPRECATED
        /// </summary>
        public int WaveSize { get; set; }
        public int EnemiesDead { get; set; }
        public bool FirstRound;

        public int WaveSizeIncreaseConstant;

        public SpawnComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            WaveSize = 1;
            EnemiesDead = 0;
            FirstRound = true;
            WaveSizeIncreaseConstant = 1;
            return this;
        }
    }
}
