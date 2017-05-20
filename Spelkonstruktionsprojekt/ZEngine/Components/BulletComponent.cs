using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class BulletComponent : IComponent
    {
        public int Damage;
        public uint ShooterEntityId;

        public IComponent Reset()
        {
            Damage = 0;
            ShooterEntityId = default(uint);
            return this;
        }
    }
}
