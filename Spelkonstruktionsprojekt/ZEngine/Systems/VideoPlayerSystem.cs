using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class VideoPlayerSystem
    {

        private readonly VideoPlayer _videoPlayer;

        public VideoPlayerSystem()
        {
            _videoPlayer = new VideoPlayer();
        }

        public bool IsDonePlaying()
        {
                return _videoPlayer.State == MediaState.Stopped;  
        }

        public void Play(Video video)
        {
            if (_videoPlayer.State == MediaState.Stopped)
            {
                _videoPlayer.Play(video);
            }

        }

        public void Draw(GameDependencies gameDependencies)
        {
            var viewport = gameDependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
            Texture2D videoTexture = null;

            if (!IsDonePlaying())
                videoTexture = _videoPlayer.GetTexture();

            if (videoTexture != null)
            {
                gameDependencies.SpriteBatch.Begin();
                gameDependencies.SpriteBatch.Draw(videoTexture, new Rectangle(0, 0, (int)viewport.X, (int)viewport.Y), Color.White);
                gameDependencies.SpriteBatch.End();
            }
        }
    }
}
