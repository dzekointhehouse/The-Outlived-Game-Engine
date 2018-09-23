using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents
{
    public class AmmoPickupComponent : IComponent
    {
        public int Amount { get; set; }

        public AmmoPickupComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            Amount = 10;
            return this;
        }
    }
}
