using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;
namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class WeaponComponent : IComponent
    {
        string Name { get; set; }
        int Damage { get; set; }
        float Range { get; set; }
        int RateOfFire{get; set;}
    }
}
