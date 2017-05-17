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
    /// <summary>
    /// Game content used in the menu.
    /// </summary>
    public class MenuContent
    {
        public Video IntroVideo;
        public Song BackgroundSong;
        public Texture2D Background;
        public Texture2D BackgroundFog;
        public Texture2D CreditsBackground;
        public Texture2D PauseBackground;

        //Game Over
        public SpriteFont ScoreFont;
        public Texture2D GameOver;

        // Characters
        public Texture2D HighlightFirst;
        public Texture2D HighlightSecond;
        public Texture2D HighlightThird;
        public Texture2D HighlightFourth;

        public Texture2D GamePadIcon;
        public Texture2D GamePadIconHighlight;
        public Texture2D TeamOptions;
        
        public Texture2D MainOptionsBackground;
        public Texture2D GameModeHiglightExtinction;
        public Texture2D GameModeHiglightSurvival;
        public Texture2D GameModeHiglightBlockworld;

        public Texture2D ButtonEsc;
        public Texture2D ButtonContinue;
        public Texture2D ButtonBack;
        public Texture2D CharacterBackground;
        public Texture2D AboutBackground;

        // Continue
        // Go back
        // Quit


        public SpriteFont MenuFont;

        public MenuContent(Microsoft.Xna.Framework.Game game)
        {
            MenuFont = game.Content.Load<SpriteFont>("Fonts/ZMenufont");

            // Intro
            IntroVideo = game.Content.Load<Video>("version1");

            // Multiplayer
            GamePadIcon = game.Content.Load<Texture2D>("Images/Gamepad/gamepad");
            GamePadIconHighlight = game.Content.Load<Texture2D>("Images/Gamepad/gamepad_h");
            TeamOptions = game.Content.Load<Texture2D>("Images/Menu/teamoptions");

            // Main menu
            BackgroundSong = game.Content.Load<Song>("Sound/creepyPiano");
            MainOptionsBackground = game.Content.Load<Texture2D>("Images/Menu/mainoptions");
            Background = game.Content.Load<Texture2D>("Images/Menu/background3");
            BackgroundFog = game.Content.Load<Texture2D>("Images/Menu/movingfog");

            // Credits
            CreditsBackground = game.Content.Load<Texture2D>("Images/Menu/credits");

            // Game mode
            GameModeHiglightExtinction = game.Content.Load<Texture2D>("Images/Menu/gamemodemenu_he");
            GameModeHiglightSurvival = game.Content.Load<Texture2D>("Images/Menu/gamemodemenu_hs");
            GameModeHiglightBlockworld = game.Content.Load<Texture2D>("Images/Menu/gamemodemenu_hb");

            // Pause
            PauseBackground = game.Content.Load<Texture2D>("Images/Menu/paused");
            // About
            AboutBackground = game.Content.Load<Texture2D>("Images/Menu/about");

            CharacterBackground = game.Content.Load<Texture2D>("Images/zombieCharacter");
            //ButtonEsc = game.Content.Load<Texture2D>("Images/Keyboard/esc");

            //ButtonBack = game.Content.Load<Texture2D>("Images/Keyboard/left");

            HighlightFirst = game.Content.Load<Texture2D>("Images/Characters/character_hb");
            HighlightSecond = game.Content.Load<Texture2D>("Images/Characters/character_he");
            HighlightThird = game.Content.Load<Texture2D>("Images/Characters/character_hw");
            HighlightFourth = game.Content.Load<Texture2D>("Images/Characters/character_hj");

            //Game Over stuff
            ScoreFont = game.Content.Load<SpriteFont>("Fonts/Score");
            GameOver = game.Content.Load<Texture2D>("Images/GameOver/GameOver");

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
