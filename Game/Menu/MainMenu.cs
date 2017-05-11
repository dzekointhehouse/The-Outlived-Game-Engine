using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Wrappers;

namespace Game.Menu
{
    class MainMenu : IMenu
    {
        private Microsoft.Xna.Framework.Game game;
        private GameManager gameManager;
        private OptionsState currentPosition = OptionsState.Exit;
        Viewport viewport;

        private enum OptionsState
        {
            Continue,
            Pause,
            Exit
        }

        public MainMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.engine.Dependencies.Game;

            viewport = game.GraphicsDevice.Viewport;
        }

        private void ContinueButton(GameManager.GameState state)
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Enter) && gameManager.OldState.IsKeyUp(Keys.Enter))
            {
                gameManager.CurrentGameState = state;

            }
            gameManager.OldState = newState;
        }

        private void ArrowPosition()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;
            // get the newest state
            KeyboardState newState = Keyboard.GetState();

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Up) && gameManager.OldState.IsKeyUp(Keys.Up))
            {
                if(currentPosition != 0)
                    currentPosition -= 1;
                gameManager.OldState = newState;

            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Down) && gameManager.OldState.IsKeyUp(Keys.Down))
            {
                if (currentPosition != OptionsState.Exit)
                    currentPosition++;
                gameManager.OldState = newState;
            }



        }

        private void MainMenuDisplay()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;
            //LoadMenu();
            String textEscape = "BACK TO THE MAIN MENU / PAUSE THE GAME";
            String textContinue = "CONTINUE";
            String textExit = "EXIT THE GAME";


            sb.Begin();

            sb.Draw(gameManager.GameContent.MainBackground, viewport.Bounds, Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textContinue, new Vector2(400, viewport.Height * 0.45f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textEscape, new Vector2(400, viewport.Height * 0.55f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textExit, new Vector2(400, viewport.Height * 0.65f), Color.White);

            switch (currentPosition)
            {
                case OptionsState.Continue:
                    sb.Draw(gameManager.GameContent.ButtonEnter, new Vector2(250, viewport.Height * 0.45f), Color.White);
                    break;
                case OptionsState.Pause:
                    sb.Draw(gameManager.GameContent.ButtonEnter, new Vector2(250, viewport.Height * 0.55f), Color.White);
                    break;
                case OptionsState.Exit:
                    sb.Draw(gameManager.GameContent.ButtonEnter, new Vector2(250, viewport.Height * 0.65f), Color.White);
                    break;
            }

            sb.End();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MainMenuDisplay();

        }

        public void Update(GameTime gameTime)
        {
            ArrowPosition();

            switch (currentPosition)
            {
                case OptionsState.Continue:
                    ContinueButton(GameManager.GameState.GameModesMenu);
                    break;
                case OptionsState.Pause:
                    ContinueButton(GameManager.GameState.MainMenu);
                    break;
                case OptionsState.Exit:
                    ContinueButton(GameManager.GameState.GameOver);
                    break;
            }
        }
    }
}
