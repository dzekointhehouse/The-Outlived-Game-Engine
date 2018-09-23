using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using TheOutlivedGL;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    /// <summary>
    /// Pause game state, when the user want's to
    /// pause the game.
    /// </summary>
    class PausedMenu : IMenu
    {
        private readonly GameManager gameManager;
        private Viewport viewport;

        public PausedMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = OutlivedGame.Instance().graphics.GraphicsDevice.Viewport;
        }

        // drawing the menu background.
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/paused"), viewport.Bounds, Color.White);
            sb.End();
        }

        // A pause button that goes to the pause game state,
        // but if the current game state is the pause state
        // then we go back to the previous state.
        public void Update(GameTime gameTime)
        {
            foreach (var virtualInput in gameManager.playerControllers.Controllers)
            {
                if (virtualInput.Is(Pause, Pressed))
                {
                    gameManager.MenuNavigator.Pause();
                }
                else if(virtualInput.Is(Cancel, Pressed))
                {
                    gameManager.MenuNavigator.Pause();
                    gameManager.MenuNavigator.GoTo(OutlivedStates.GameState.MainMenu);
                }
            }
        }

        public void Reset()
        {
        }
    }
}
