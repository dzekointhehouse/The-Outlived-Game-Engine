using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEngine.Components
{
    class RenderComponent : IComponent
    {
        public PositionComponent PositionComponent { get; set; }

        public DimensionsComponent DimensionsComponent { get; set; }

        public double Radius { get; set; }
        
        public bool IsVisible { get; set; }
    }
}
