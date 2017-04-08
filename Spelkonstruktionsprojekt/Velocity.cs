using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ZombieGame
{
    class Velocity : IComponent
    {
        public Vector2 velocity { get; set; }
        public string GetComponentName => "Velocity";
    }
}
