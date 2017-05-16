using System;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    /// <summary>
    /// The main game menu state. This state is used as the first
    /// state that the users come to when starting the game
    /// (exl. intro state). It presents options that the players
    /// can choose from.
    /// </summary>
    class MainMenu : IMenu
    {
        private Microsoft.Xna.Framework.Game game;
        private GameManager gameManager;
        private ControlsConfig controls;
        private OptionsState currentPosition = OptionsState.Continue;
        private Viewport viewport;
        private SpriteBatch sb = GameDependencies.Instance.SpriteBatch;
        private SidewaysBackground fogBackground;
        private SidewaysBackground mainBackground;

        // different menu options
        private enum OptionsState
        {
            Continue,
            About,
            Credits,
            Exit
        }

        public MainMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.Engine.Dependencies.Game;
            controls = new ControlsConfig(0, 3, gameManager);
            viewport = game.GraphicsDevice.Viewport;
            mainBackground = new SidewaysBackground(gameManager.MenuContent.Background, new Vector2(10, 10), 1.5f);
            fogBackground = new SidewaysBackground(gameManager.MenuContent.BackgroundFog, new Vector2(20, 20), 1f);
        }


        // This method displays all the options
        // in an ordered fashion.
        private void MainMenuDisplay()
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

            switch (currentPosition)
            {
                case OptionsState.Continue:
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.40f), Color.White);
                    break;
                case OptionsState.About:
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.50f), Color.White);
                    break;
                case OptionsState.Credits:
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.60f), Color.White);
                    break;
                case OptionsState.Exit:
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.70f), Color.White);
                    break;

            }
        }

        // Draws
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            ScalingBackground.DrawBackgroundWithScaling(spriteBatch, gameManager.MenuContent, 0f);
            mainBackground.Draw(spriteBatch);
            
            MainMenuDisplay();
            spriteBatch.End();
        }

        // Updates. When the players clicks continue we go to
        // the next state that is specified in the switch case,
        // depending on which option we currently are at.
        public void Update(GameTime gameTime)
        {
            fogBackground.Update(gameTime, new Vector2(1,0),viewport);
            //mainBackground.Update(gameTime, new Vector2(1, 0), viewport);
            // playing beatiful mainBackground music.
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Volume = 0.8f;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(gameManager.MenuContent.BackgroundSong);

            }

            currentPosition = (OptionsState) controls.MoveOptionPositionVertically((int) currentPosition);

            switch (currentPosition)
            {
                case OptionsState.Continue:
                    controls.ContinueButton(GameManager.GameState.GameModesMenu);
                    break;
                case OptionsState.About:
                    controls.ContinueButton(GameManager.GameState.About);
                    break;
                case OptionsState.Credits:
                    controls.ContinueButton(GameManager.GameState.Credits);
                    break;
                case OptionsState.Exit:
                    controls.ContinueButton(GameManager.GameState.Quit);
                    break;
            }
        }
    }
}
