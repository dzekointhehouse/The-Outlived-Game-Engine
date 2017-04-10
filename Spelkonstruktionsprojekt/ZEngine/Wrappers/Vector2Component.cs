using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ZEngine.Wrappers
{
    public class Vector2Component
    {
        public Vector2 Vectors { get; set; }

        public Vector2Component(float x, float y)
        {
            Vectors = new Vector2(x, y);
        }
    }
}
