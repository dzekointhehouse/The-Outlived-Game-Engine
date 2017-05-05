using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEngine.Components
{
    public class CollisionComponent : IComponent
    {
        public Rectangle SpriteBoundingRectangle { get; set; }

        public bool IsCage = false;

        //each int is an entityId
        public List<int> collisions = new List<int>();
    }
}
