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
        public string SongName { get; set; }
        public SoundEffectInstance SoundInstace { get; set; }
        public float Volume { get; set; } = 1f;


    }

    public class ZEngineSoundBank
    {
        public enum Sounds
        {
            WalkingForward

        }

        public static Dictionary<Sounds, SoundEffectInstance> SoundInstances = new Dictionary<Sounds, SoundEffectInstance>();

        public static void Load(ContentManager contentManager)
        {
           // SoundInstances[Sounds.WalkingForward] = contentManager.Load<SoundEffect>("walking").CreateInstance();

            var entities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(SoundComponent));
            foreach (var entity in entities)
            {
                SoundComponent soundComponent = (SoundComponent) entity.Value;
                soundComponent.SoundInstace = contentManager.Load<SoundEffect>(soundComponent.SongName).CreateInstance();

            }
        }

        

    }


}
