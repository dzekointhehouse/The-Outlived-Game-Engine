using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    /*
     * This component is for medkits and ammo pickups, has an enum so we know what type it is.
     */
    class PickupComponent : IComponent
    {
        public string PickupType { get; set; }

    }
}
