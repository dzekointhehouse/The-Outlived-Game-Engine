using System;
using System.Diagnostics;
using System.Net;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ZEngine.Wrappers;
using static Game.GameManager;
using static Game.Menu.States.MainMenu.OptionsState;

namespace Game.Menu.States
{
    /// <summary>
    /// The main game menu state. This state is used as the
    /// state that the users come to when starting the game
    /// (exl. intro state). It presents options that the players
    /// can choose from.
    /// </summary>
    class MainMenu : IMenu
    {
        public VirtualGamePad VirtualGamePad { get; }
        public MenuNavigator MenuNavigator { get; }
        private Microsoft.Xna.Framework.Game game;
        private GameManager gameManager;
        private Viewport viewport;
        //private SpriteBatch sb;
        private SidewaysBackground fogBackground;
        private SidewaysBackground mainBackground;

        public GenericButtonNavigator<OptionsState> MenuPosition { get; set; }
        
        // different menu options
        internal enum OptionsState
        {
            Continue,
            About,
            Credits,
            Exit
        }

        public OptionsState[] Options =
        {
            Continue, About, OptionsState.Credits, Exit
        };
        
        public MainMenu(GameManager gameManager, VirtualGamePad virtualGamePad, MenuNavigator menuNavigator)
        {
            VirtualGamePad = virtualGamePad;
            MenuNavigator = menuNavigator;
            MenuPosition = new GenericButtonNavigator<OptionsState>(Options);
            this.gameManager = gameManager;

            game = gameManager.game;
           // sb = gameManager.spriteBatch;
            viewport = gameManager.viewport;

            mainBackground = new SidewaysBackground(gameManager.MenuContent.Background, new Vector2(10, 10), 1.5f);
            fogBackground = new SidewaysBackground(gameManager.MenuContent.BackgroundFog, new Vector2(20, 20), 1f);
        }



        // This method displays all the options
        // in an ordered fashion.
        private void MainMenuDisplay(SpriteBatch sb)
        {
            string textCredits = "CREDITS";
            string textContinue = "PLAY IF YOU DARE";
            string textAbout = "WHAT HAPPENED?";
            string textExit = "TAKE THE EASY WAY OUT";

            sb.Draw(gameManager.MenuContent.MainOptionsBackground, viewport.Bounds, Color.White);
            fogBackground.Draw(sb);
            sb.DrawString(gameManager.MenuContent.MenuFont, textContinue, new Vector2(600, viewport.Height * 0.45f), Color.White);
            sb.DrawString(gameManager.MenuContent.MenuFont, textAbout, new Vector2(600, viewport.Height * 0.55f), Color.White);
            sb.DrawString(gameManager.MenuContent.MenuFont, textCredits, new Vector2(600, viewport.Height * 0.65f), Color.White);
            sb.DrawString(gameManager.MenuContent.MenuFont, textExit, new Vector2(600, viewport.Height * 0.75f), Color.White);

            switch (MenuPosition.CurrentPosition)
            {
                case Continue:
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.42f), Color.White);
                    break;
                case About:
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.52f), Color.White);
                    break;
                case OptionsState.Credits:
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.62f), Color.White);
                    break;
                case Exit:
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.72f), Color.White);
                    break;

            }
        }

        // Draws
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            ScalingBackground.DrawBackgroundWithScaling(sb, gameManager.MenuContent, 0f);
            mainBackground.Draw(sb);
            
            MainMenuDisplay(sb);
            sb.End();
        }

        // Updates. When the players clicks continue we go to
        // the next state that is specified in the switch case,
        // depending on which option we currently are at.
        public void Update(GameTime gameTime)
        {
            fogBackground.Update(gameTime, new Vector2(1,0),viewport);
            //mainBackground.UpdateTimer(gameTime, new Vector2(1, 0), viewport);
            // playing beatiful mainBackground music.
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Volume = 0.8f;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(gameManager.MenuContent.BackgroundSong);
            }
            
            MenuPosition.UpdatePosition(VirtualGamePad);
            
            if (VirtualGamePad.Is(VirtualGamePad.MenuKeys.Accept, VirtualGamePad.MenuKeyStates.Pressed))
            {
                gameManager.MenuContent.ClickSound.Play();
                switch (MenuPosition.CurrentPosition)
                {
                    case Continue:
                        MenuNavigator.GoTo(GameState.GameModesMenu);
                        break;
                    case About:
                        MenuNavigator.GoTo(GameState.About);
                        break;
                    case OptionsState.Credits:
                        MenuNavigator.GoTo(GameState.Credits);
                        break;
                    case Exit:
                        MenuNavigator.GoTo(GameState.Quit);
                        break;
                }
            }
        }

        public void Reset()
        {
        }
    }
}
