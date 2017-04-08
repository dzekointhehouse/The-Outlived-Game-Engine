using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEngine.Components
{
    class RenderComponent : IComponent
    {
        public static string GetComponentName => "Render";
        
        public PositionComponent PositionComponent { get; set; }

        public DimensionsComponent DimensionsComponent { get; set; }

        public double Radius { get; set; }
        string IComponent.GetComponentName
        {
            get { return GetComponentName; }
        }
    }
}
