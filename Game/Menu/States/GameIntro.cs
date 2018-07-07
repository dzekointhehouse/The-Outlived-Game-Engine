using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using static Game.Services.VirtualGamePad;

namespace Game.Menu.States
{
    class GameIntro : IMenu
    {
        private GameManager gameManager;

        private Viewport viewport;
        private readonly VideoPlayer videoPlayer;
        private MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }

        public GameIntro(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            this.gameManager = gameManager;
            viewport = OutlivedGame.Instance().graphics.GraphicsDevice.Viewport;
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            videoPlayer = new VideoPlayer();
//            videoPlayer.Count(gameManager.OutlivedContent.IntroVideo);
        }


        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            Texture2D videoTexture = null;

            if (videoPlayer.State != MediaState.Stopped)
                videoTexture = videoPlayer.GetTexture();

            if (videoTexture != null)
            {
                // sb.Begin();
                // use viewport
                sb.Draw(videoTexture, new Rectangle(0, 0, 1920, 1080), Color.White);
                //sb.End();
            }
            else gameManager.CurrentGameState = OutlivedStates.GameState.MainMenu;
            sb.End();
        }

        public void Update(GameTime gameTime)
        {
            // Skipping the intro.
            if (VirtualGamePad.Is(MenuKeys.Cancel, MenuKeyStates.Pressed))
            {
                MenuNavigator.GoTo(OutlivedStates.GameState.MainMenu);
            }

            // We want to stop playing the video and dispose it if 
            // the game state has been set to main menu.
            if (gameManager.CurrentGameState == OutlivedStates.GameState.MainMenu)
            {
                videoPlayer.Stop();
                videoPlayer.Video.Dispose();
                videoPlayer.Dispose();
            }
        }

        public void Reset()
        {
        }
    }
}