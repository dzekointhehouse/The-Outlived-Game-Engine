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
        public double score { get; set; }

        public EntityScoreComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            score = 0;
            return this;
        }
    }
}
