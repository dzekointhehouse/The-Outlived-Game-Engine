using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Components;
using static System.String;

namespace ZEngine.Components
{
    public class SpriteComponent : IComponent
    {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public Point Position { get; set; }

        public bool SpriteIsLoaded { get; set; }
        public Texture2D Sprite { get; set; }
        public string SpriteName { get; set; }

        // Added to be used for gradient transparency when an entity
        // dies, but can be used in other cases also.
        public float Scale { get; set; }
        public float Alpha { get; set; }

        public SpriteComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            TileWidth = 0;
            TileHeight = 0;
            Position = Point.Zero;
//            SpriteIsLoaded = false;
//            Sprite = null;
//            SpriteName = Empty;
            Scale = 1;
            Alpha = 1;
            return this;
        }
    }
}
