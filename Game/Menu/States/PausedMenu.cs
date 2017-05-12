using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    class PausedMenu : IMenu
    {
        private readonly GameManager gameManager;
        private readonly ControlConfiguration controls;
        private Viewport viewport;

        public PausedMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            controls = new ControlConfiguration(gameManager);
            viewport = this.gameManager.engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

            sb.Begin();
            sb.Draw(gameManager.GameContent.PauseBackground, viewport.Bounds, Color.White);
            sb.End();
        }

        public void Update(GameTime gameTime)
        {
            controls.PauseButton();
        }

        public void ContinueButton(GameManager.GameState state)
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.BigButton == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Q) && gameManager.OldState.IsKeyUp(Keys.Q))
            {
                gameManager.PreviousGameState = gameManager.CurrentGameState;
                gameManager.CurrentGameState = GameManager.GameState.Quit;
            }
            gameManager.OldState = newState;
        }
    }
}
