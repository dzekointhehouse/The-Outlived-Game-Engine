using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using TheOutlivedGL;
using ZEngine.Wrappers;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    class Credits : IMenu
    {
        private readonly GameManager gameManager;
        private Viewport viewport;

        public Credits(GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.viewport = OutlivedGame.Instance().graphics.GraphicsDevice.Viewport;
        }

        // Draws a simple background which contains
        // the credits. Woohoo.
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gameManager.effects.DrawExpandingEffect(sb, AssetManager.Instance.Get<Texture2D>("Images/Menu/background3"));
            sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/credits"), viewport.Bounds, Color.White);
            sb.End();
        }

        // The backspace button is added which makes
        // it possible to go back to the previous game
        // state.
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