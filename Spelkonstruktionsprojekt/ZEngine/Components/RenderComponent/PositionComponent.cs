using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEngine.Components
{
    class PositionComponent : IComponent
    {
        public static string SystemName = "Position";

        public PositionComponent()
        {

        }

        public string GetComponentName => "PositionComponent";
    }
}
