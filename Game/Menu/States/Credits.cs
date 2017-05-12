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
        private Microsoft.Xna.Framework.Game game;
        private GameManager gameManager;
        private ControlConfiguration controls;
        private float minScale = 1.0f, maxScale = 2.0f, scale = 1.0f;
        private bool moveHigher = true;
        private Viewport viewport;


        public Credits(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.engine.Dependencies.Game;
            controls = new ControlConfiguration(0, 2, gameManager);
            viewport = game.GraphicsDevice.Viewport;
        }



        private void DrawCredits()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

            //if (scale <= maxScale && scale >= minScale)
            //{
            //    if (scale <= minScale + 0.1)
            //    {
            //        moveHigher = true;
            //    }
            //    if (scale >= maxScale - 0.1)
            //    {
            //        moveHigher = false;
            //    }

            //    if (moveHigher)
            //    {
            //        scale = scale + 0.0001f;
            //    }
            //    else
            //    {
            //        scale = scale - 0.0001f;
            //    }
            //}


            sb.Begin();
            //sb.Draw(
            //    texture: gameManager.GameContent.Background,
            //    position: Vector2.Zero,
            //    color: Color.White,
            //    scale: new Vector2(scale)
            //);
            sb.Draw(gameManager.GameContent.CreditsBackground, viewport.Bounds, Color.White);
            sb.End();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawCredits();

        }

        public void Update(GameTime gameTime)
        {
            controls.GoBackButton();
        }
    }
}
