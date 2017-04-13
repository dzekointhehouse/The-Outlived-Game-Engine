using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;

namespace ZEngine.Components
{
    public class PositionComponent : IComponent
    {
        public Vector2D Position { get; set; }

        public int ZIndex { get; set; }
    }
}
