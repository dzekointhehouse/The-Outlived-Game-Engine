using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Game.Menu.States
{
    class GameIntro : IMenu
    {
        private GameManager gameManager;
       // private SpriteBatch sb;
        private Viewport viewport;
        private readonly VideoPlayer videoPlayer;
        private readonly ControlsConfig controls;
        public GameIntro(GameManager gameManager)
        {
            this.gameManager = gameManager;
           // sb = gameManager.engine.Dependencies.SpriteBatch;
            viewport = gameManager.Engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
            controls = new ControlsConfig(gameManager);
            videoPlayer = new VideoPlayer();
            videoPlayer.Play(gameManager.GameContent.IntroVideo);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Texture2D videoTexture = null;

            if (videoPlayer.State != MediaState.Stopped)
                videoTexture = videoPlayer.GetTexture();

            if (videoTexture != null)
            {
               // spriteBatch.Begin();
                spriteBatch.Draw(videoTexture, new Rectangle(0, 0, 1920, 1080), Color.White);
                //spriteBatch.End();

            }
            else gameManager.CurrentGameState = GameManager.GameState.MainMenu;
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            controls.ContinueButton(GameManager.GameState.MainMenu);

            // We want to stop playing the video and dispose it if 
            // the game state has been set to main menu.
            if (gameManager.CurrentGameState == GameManager.GameState.MainMenu)
            {
                videoPlayer.Stop();
                videoPlayer.Video.Dispose();
                videoPlayer.Dispose();
            }
                
        }
    }
}
