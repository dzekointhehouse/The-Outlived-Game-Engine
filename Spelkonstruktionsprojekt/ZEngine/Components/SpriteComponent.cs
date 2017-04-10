using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Components;

namespace ZEngine.Components
{
    class SpriteComponent : IComponent
    {
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;

        public float Angle { get; set; } = 0;
        public bool SpriteIsLoaded { get; set; } = false;
        public Texture2D Sprite { get; set; }
        public string SpriteName { get; set; }

        public float Scale { get; set; } = 1;
    }
}
