using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class GameScoreComponent : IComponent
    {
        public int ElapsedSecondsLimit { get; set; } = 10;

        public int PointsPerSecondsLimit { get; set; }

        public int TotalGameScore { get; set; }
    }
}
