using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game
{
    public class GameContent
    {
        public Video IntroVideo;
        public Texture2D Background;
        public Texture2D CreditsBackground;
        
        public Texture2D MainOptionsBackground;
        public Texture2D GameModeHiglightExtinction;
        public Texture2D GameModeHiglightSurvival;
        public Texture2D GameModeHiglightBlockworld;

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

            // Intro
            IntroVideo = game.Content.Load<Video>("version1");
            // Main menu
            MainOptionsBackground = game.Content.Load<Texture2D>("Images/Menu/mainoptions");
            Background = game.Content.Load<Texture2D>("Images/Menu/background");

            // Credits
            CreditsBackground = game.Content.Load<Texture2D>("Images/Menu/credits");

            // Game mode
            GameModeHiglightExtinction = game.Content.Load<Texture2D>("Images/Menu/gamemodemenu_he");
            GameModeHiglightSurvival = game.Content.Load<Texture2D>("Images/Menu/gamemodemenu_hs");
            GameModeHiglightBlockworld = game.Content.Load<Texture2D>("Images/Menu/gamemodemenu_hb");

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
