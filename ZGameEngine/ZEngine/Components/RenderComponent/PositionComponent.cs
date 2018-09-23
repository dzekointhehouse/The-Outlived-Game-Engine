using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace ZEngine.Components
{
    public class PositionComponent : IComponent
    {
        public Vector2 Position { get; set; }
        public int ZIndex { get; set; }
        public IComponent Reset()
        {
            Position = Vector2.Zero;
            ZIndex = 0;
            return this;
        }
    }
}
