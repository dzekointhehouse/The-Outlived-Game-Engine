using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class EntityScoreComponent : IComponent
    {
        public double score { get; set; } = 0;

        public double multiplier = 1;
        public double survivalScoreFactor = 5;
        public int damageScore = 50;
        public int damagePenalty = -500;

    }
}
