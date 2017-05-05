using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    // Optimus prime
    public class VideoPlayerSystem : ISystem
    {
        // Maybe having our own player instance
        private VideoPlayer _videoPlayer;

        // We instantiate our player when we create our system.
        public void Start()
        {
            _videoPlayer = new VideoPlayer();
        }

        // method to check if the videoplayer is playing.
        public bool IsPlaying()
        {
            return _videoPlayer.State == MediaState.Playing;
        }

        public bool IsStopped()
        {
            return _videoPlayer.State == MediaState.Stopped;
        }

        // if the video is not already playing, than this method
        // calls it to play.
        public void Play(Video video)
        {
            if (IsStopped())
            {
                _videoPlayer.Play(video);
            }

        }

        // This draw method call will render the video playing to the screen.
        public void Draw(GameDependencies gameDependencies)
        {
            Texture2D videoTexture = null;

            var viewport = gameDependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;

            if (!IsStopped())
                videoTexture = _videoPlayer.GetTexture();

            if (videoTexture != null)
            {
                gameDependencies.SpriteBatch.Begin();
                gameDependencies.SpriteBatch.Draw(videoTexture, new Rectangle(0, 0, (int) viewport.X, (int) viewport.Y),
                    Color.White);
                gameDependencies.SpriteBatch.End();
            }
        }
    }
}

//Texture2D videoTexture = null;

//            if (player.State != MediaState.Stopped)
//                videoTexture = player.GetTexture();

//            if (videoTexture != null)
//            {
//                spriteBatch.Begin();
//                spriteBatch.Draw(videoTexture, new Rectangle(0, 0, (int)viewportDimensions.X, (int)viewportDimensions.Y), Color.White);
//                spriteBatch.End();
//            }