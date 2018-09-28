using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using System.Linq;
using TheOutlivedGL;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    /// <summary>
    /// Pause game state, when the user want's to
    /// pause the game.
    /// </summary>
    class AboutMenu : IMenu
    {
        private readonly GameManager gameManager;
        private Viewport viewport;

        public AboutMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = OutlivedGame.Instance().graphics.GraphicsDevice.Viewport;
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gameManager.effects.DrawExpandingEffect(sb, AssetManager.Instance.Get<Texture2D>("Images/Menu/background3"));
            sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/about"), viewport.Bounds, Color.White);
            sb.End();
        }

        // A pause button that goes to the pause game state,
        // but if the current game state is the pause state
        // then we go back to the previous state.
        public void Update(GameTime gameTime)
        {
            if (gameManager.playerControllers.Controllers.Any(c => c.Is(Cancel, Pressed)))
            {
                gameManager.MenuNavigator.GoBack();
            }
        }

        public void Reset()
        {
        }
    }
}
