using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    class Credits : IMenu
    {
        private readonly GameManager gameManager;
        private readonly ControlsConfig controls;
        private SpriteBatch sb = GameDependencies.Instance.SpriteBatch;
        private Viewport viewport;
        
        public Credits(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = this.gameManager.Engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
            controls = new ControlsConfig(gameManager);
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
            controls.GoBackButton();
        }
    }
}
