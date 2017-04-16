using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class VideoPlayerSystem
    {

        private VideoPlayer videoPlayer;

        public bool IsDonePlaying()
        {
                return videoPlayer.State == MediaState.Stopped;  
        }
    }
}
