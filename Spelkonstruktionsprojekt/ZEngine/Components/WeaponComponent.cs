using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;
namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class WeaponComponent : IComponent
    {
        public bool Automatic { get; set; }
        public int Damage { get; set; }
        public string Name { get; set; }
        public float Range { get; set; }
        public int RateOfFire { get; set; }
        public int ClipSize { get; set; }

    }
}
