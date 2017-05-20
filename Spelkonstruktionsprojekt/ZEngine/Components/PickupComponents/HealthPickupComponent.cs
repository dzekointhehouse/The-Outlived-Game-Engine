using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents
{
    public class HealthPickupComponent : IComponent
    {
        public int Amount { get; set; }

        public HealthPickupComponent()
        {
            Reset();
        }
        
        public IComponent Reset()
        {
            Amount = 50;
            return this;
        }
    }
}
