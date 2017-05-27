using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using ZEngine.Managers;

namespace ZEngine.Components
{
    // Component to use when the entity can make sounds.
    public class SoundComponent : IComponent
    {
        public enum SoundBank
        {
            Reload,
            EmptyMag,
        }
        public string SoundEffectName { get; set; }
        public SoundEffect SoundEffect { get; set; }
        public float Volume { get; set; } = 1f;

        public Dictionary<SoundBank, SoundEffectInstance> SoundList = new Dictionary<SoundBank, SoundEffectInstance>(5);

        public SoundComponent()
        {
            Reset();
        }



        public IComponent Reset()
        {
            Volume = 1f;
            SoundEffectName = string.Empty;
            SoundList = new Dictionary<SoundBank, SoundEffectInstance>(5);
            SoundEffect = null;
            return this;
        }
    }

}
