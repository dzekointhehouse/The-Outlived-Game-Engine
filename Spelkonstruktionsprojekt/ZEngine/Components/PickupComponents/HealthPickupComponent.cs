using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents
{
    class HealthPickupComponent : IComponent
    {
        public int Amount { get; set; } = 50;
    }
}
