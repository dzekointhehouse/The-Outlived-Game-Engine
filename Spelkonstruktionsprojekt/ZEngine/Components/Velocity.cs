using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ZEngine.Components;

namespace ZombieGame
{
    class Velocity : Component
    {
        public Vector2 velocity { get; set; }
        public string GetComponentName => "Velocity";
    }
}
