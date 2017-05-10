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

        public MainMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.engine.Dependencies.Game;
        }

        //private void LoadMenu()
        //{
        //    font = game.Content.Load<SpriteFont>("Fonts/ZMenufont");
        //    background = game.Content.Load<Texture2D>("Images/mainmenu");
        //}
        private void ContinueButton()
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Enter) && newState != gameManager.OldState)
            {
                gameManager.CurrentGameState = GameManager.GameState.GameModesMenu;
                gameManager.OldState = newState;
            }

        }

        private void BackToMenu()
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Escape) && newState != gameManager.OldState)
            {
                gameManager.CurrentGameState = GameManager.GameState.MainMenu;
                gameManager.OldState = newState;
            }
        }
        private void ExitButton()
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || newState.IsKeyDown(Keys.S) && newState != gameManager.OldState)
            {
                game.Exit();
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
            var viewport = game.GraphicsDevice.Viewport;

            sb.Begin();

            sb.Draw(gameManager.GameContent.MainBackground, viewport.Bounds, Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textContinue, new Vector2(400, viewport.Height * 0.45f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textEscape, new Vector2(400, viewport.Height * 0.55f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textExit, new Vector2(400, viewport.Height * 0.65f), Color.White);

            sb.End();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MainMenuDisplay();

        }

        public void Update(GameTime gameTime)
        {
            ContinueButton();
            BackToMenu();
            ExitButton();
        }
    }
}
