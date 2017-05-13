using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    class Credits : IMenu
    {
        private readonly GameManager gameManager;
        private readonly ControlConfiguration controls;
        private Viewport viewport;


        public Credits(GameManager gameManager)
        {
            this.gameManager = gameManager;
            viewport = this.gameManager.engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
            controls = new ControlConfiguration(gameManager);
        }



        private void DrawCredits()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

            sb.Begin();
            sb.Draw(gameManager.GameContent.CreditsBackground, viewport.Bounds, Color.White);
            sb.End();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawCredits();

        }

        public void Update(GameTime gameTime)
        {
            controls.GoBackButton();
        }
    }
}
