using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Spelkonstruktionsprojekt.ZEngine.Wrappers
{
    public class Vector2D
    {
        public static Vector2D Create(double x, double y)
        {
            return new Vector2D()
            {
                X = x,
                Y = y
            };
        }

        public static implicit operator Vector2D(Vector2 vector2)
        {
            return Vector2D.Create(vector2.X, vector2.Y);
        }

        public static implicit operator Vector2(Vector2D vector2D)
        {
            return new Vector2((float) vector2D.X, (float) vector2D.Y);
        }

        public double X { get; set; }
        public double Y { get; set; }

        public override string ToString()
        {
            return "[  X:" + X + ",  Y:" + Y + "  ]";
        }
    }
}
