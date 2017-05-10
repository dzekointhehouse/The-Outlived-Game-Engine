using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    public class GameContent
    {
        public Texture2D MainBackground;
        public Texture2D GameModeBackground;
        public Texture2D ButtonEsc;
        public Texture2D ButtonEnter;
        public Texture2D ButtonBack;


        public SpriteFont MenuFont;

        public GameContent(Microsoft.Xna.Framework.Game game)
        {
            MenuFont = game.Content.Load<SpriteFont>("Fonts/ZMenufont");
            MainBackground = game.Content.Load<Texture2D>("Images/mainmenu");
            GameModeBackground = game.Content.Load<Texture2D>("gamemodesv2");
            ButtonEsc = game.Content.Load<Texture2D>("Images/Keyboard/esc");
            ButtonEnter = game.Content.Load<Texture2D>("Images/Keyboard/enter");
            ButtonBack = game.Content.Load<Texture2D>("Images/Keyboard/left");
        }
    }
}
