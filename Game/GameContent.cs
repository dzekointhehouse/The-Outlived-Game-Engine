using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game
{
    public class GameContent
    {
        public Texture2D MainBackground;
        public Texture2D GameModeBackground;
        public Texture2D ButtonEsc;
        public Texture2D ButtonContinue;
        public Texture2D ButtonBack;
        public Texture2D CharacterBackground;

        // Continue
        // Go back
        // Quit


        public SpriteFont MenuFont;

        public GameContent(Microsoft.Xna.Framework.Game game)
        {
            MenuFont = game.Content.Load<SpriteFont>("Fonts/ZMenufont");
            MainBackground = game.Content.Load<Texture2D>("Images/mainmenu");
            GameModeBackground = game.Content.Load<Texture2D>("gamemodesv2");
            CharacterBackground = game.Content.Load<Texture2D>("Images/zombieCharacter");
            ButtonEsc = game.Content.Load<Texture2D>("Images/Keyboard/esc");

            ButtonBack = game.Content.Load<Texture2D>("Images/Keyboard/left");

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                ButtonContinue = game.Content.Load<Texture2D>("Images/GamePad/X");
            }
            else
            {
                ButtonContinue = game.Content.Load<Texture2D>("Images/Keyboard/enter");
            }

        }
    }
}
