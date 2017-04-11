using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public double X { get; set; }
        public double Y { get; set; }
    }
}
