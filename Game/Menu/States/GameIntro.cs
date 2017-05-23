using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using static Game.GameManager.GameState;
using static Game.Services.VirtualGamePad;

namespace Game.Menu.States
{
    class GameIntro : IMenu
    {
        private GameManager gameManager;

        // private SpriteBatch sb;
        private Viewport viewport;

        private readonly VideoPlayer videoPlayer;
        private MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }

        public GameIntro(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            this.gameManager = gameManager;
            viewport = gameManager.Engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            videoPlayer = new VideoPlayer();
//            videoPlayer.Play(gameManager.MenuContent.IntroVideo);
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
                // use viewport
                spriteBatch.Draw(videoTexture, new Rectangle(0, 0, 1920, 1080), Color.White);
                //spriteBatch.End();
            }
            else gameManager.CurrentGameState = GameManager.GameState.MainMenu;
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            // Skipping the intro.
            if (VirtualGamePad.Is(MenuKeys.Cancel, MenuKeyStates.Pressed))
            {
                MenuNavigator.GoTo(GameManager.GameState.MainMenu);
            }

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