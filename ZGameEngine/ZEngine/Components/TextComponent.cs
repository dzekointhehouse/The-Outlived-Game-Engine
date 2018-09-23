using Microsoft.Xna.Framework.Graphics;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class TextComponent : IComponent
    {
        public string Text { get; set; }
        public int Size { get; set; }
        public string SpriteFontName { get; set; }
        public bool LoadedFont { get; set; } = false;
        public SpriteFont SpriteFont { get; set; }
        
        public IComponent Reset()
        {
            Text = "";
            Size = 32;
            SpriteFontName = "ZEone";
            LoadedFont = false;
            SpriteFont = null;
            return this;
        }
    }
}