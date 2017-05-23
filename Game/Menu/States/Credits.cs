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
        private readonly GameManager gameManager;
        private MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }
        private SpriteBatch sb = GameDependencies.Instance.SpriteBatch;
        private Viewport viewport;

        public Credits(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            this.gameManager = gameManager;
            viewport = this.gameManager.Engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
//            controls = new ControlsConfig(gameManager);
        }

        // Draws a simple background which contains
        // the credits. Woohoo.
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            ScalingBackground.DrawBackgroundWithScaling(spriteBatch, gameManager.MenuContent, 0.0001f);
            sb.Draw(gameManager.MenuContent.CreditsBackground, viewport.Bounds, Color.White);
            spriteBatch.End();
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
    }
}