using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    /*
     * This component is for medkits and ammo pickups, has an enum so we know what type it is.
     */
    class PickupComponent
    {
        public enum PickupEventType
        {
            Health,
            Ammo
        }
    }
}
