using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Components;
namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class WeaponComponent : IComponent
    {
        public WeaponTypes WeaponType { get; set; }
        public bool Automatic { get; set; }
        public int Damage { get; set; }
        public string Name { get; set; }
        public float Range { get; set; }
        public int RateOfFire { get; set; }
        public int ClipSize { get; set; }
        public double LastFiredMoment { get; set; }

        public IComponent Reset()
        {
            WeaponType = WeaponTypes.Pistol;
            Automatic = false;
            Damage = 0;
            Name = string.Empty;
            Range = 0f;
            RateOfFire = 0;
            ClipSize = 0;
            LastFiredMoment = 0;
            return this;
        }

        public enum WeaponTypes
        {
            Pistol,
            Rifle,
            Shotgun
        }
    }
}
