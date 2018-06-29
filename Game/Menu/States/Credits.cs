using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Wrappers;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    class Credits : IMenu
    {
        private readonly GameManager gm;
        private MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }
        private Viewport viewport;

        public Credits(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            this.gm = gameManager;
            this.viewport = OutlivedGame.Instance().graphicsDeviceManager.GraphicsDevice.Viewport;
//            controls = new ControlsConfig(gameManager);
        }

        // Draws a simple background which contains
        // the credits. Woohoo.
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gm.effects.DrawExpandingEffect(sb, gm.MenuContent.Background);
            sb.Draw(gm.MenuContent.CreditsBackground, viewport.Bounds, Color.White);
            sb.End();
        }

        // The backspace button is added which makes
        // it possible to go back to the previous game
        // state.
        public void Update(GameTime gameTime)
        {
            if (VirtualGamePad.Is(Cancel, Pressed))
            {
                MenuNavigator.GoBack();
            }
        }

        public void Reset()
        {
        }
    }
}