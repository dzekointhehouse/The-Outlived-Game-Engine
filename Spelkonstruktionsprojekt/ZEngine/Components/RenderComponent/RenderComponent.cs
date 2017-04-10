using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Wrappers;

namespace ZEngine.Components
{
    class RenderComponent : IComponent
    {
        public Vector2 Position { get; set; }

        public DimensionsComponent DimensionsComponent { get; set; }

        public double Radius { get; set; } = 0;

        public bool IsVisible { get; set; } = true;
    }
}
