using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private ControlConfiguration controls;
        private OptionsState currentPosition = OptionsState.Continue;
        private Viewport viewport;
        private SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

        // different menu options
        private enum OptionsState
        {
            Continue,
            Credits,
            Exit
        }

        public MainMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.engine.Dependencies.Game;
            controls = new ControlConfiguration(0, 2, gameManager);
            viewport = game.GraphicsDevice.Viewport;
        }


        // This method displays all the options
        // in an ordered fashion.
        private void MainMenuDisplay()
        {
            String textEscape = "CREDITS";
            String textContinue = "PLAY IF YOU DARE";
            String textExit = "TAKE THE EASY WAY OUT";

            sb.Begin();
            sb.Draw(gameManager.GameContent.MainOptionsBackground, viewport.Bounds, Color.White);

            sb.DrawString(gameManager.GameContent.MenuFont, textContinue, new Vector2(600, viewport.Height * 0.45f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textEscape, new Vector2(600, viewport.Height * 0.55f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textExit, new Vector2(600, viewport.Height * 0.65f), Color.White);

            switch (currentPosition)
            {
                case OptionsState.Continue:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.40f), Color.White);
                    break;
                case OptionsState.Credits:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.50f), Color.White);
                    break;
                case OptionsState.Exit:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.60f), Color.White);
                    break;
            }

            sb.End();
        }

        // Draws
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MainMenuDisplay();

        }

        // Updates. When the players clicks continue we go to
        // the next state that is specified in the switch case,
        // depending on which option we currently are at.
        public void Update(GameTime gameTime)
        {
            currentPosition = (OptionsState) controls.MoveOptionPositionVertically((int) currentPosition);

            switch (currentPosition)
            {
                case OptionsState.Continue:
                    controls.ContinueButton(GameManager.GameState.GameModesMenu);
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
