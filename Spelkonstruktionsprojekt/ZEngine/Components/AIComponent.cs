using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ZEngine.Components
{
    class AIComponent : IComponent
    {
        public Vector2 Target;
        public bool Wander { get; set; } = false;

        public float FollowDistance { get; set; } = 500;
    }
}
    