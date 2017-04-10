using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    // Component to use when the entity can make sounds.
    class SoundComponent : IComponent
    {
        public string SongName { get; set; }
        public Song Song { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
