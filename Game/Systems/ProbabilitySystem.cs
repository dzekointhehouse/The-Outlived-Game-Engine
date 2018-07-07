using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Components;
using ZEngine.EventBus;

namespace Game.Systems
{
    /// <summary>
    /// Plays Random zombie sounds.
    /// </summary>
    class ProbabilitySystem : ISystem, IUpdateables
    {
        private Random random;
        private EventBus EventBus = EventBus.Instance;
        private Queue<SoundEffectInstance> zombieSounds;



        public ProbabilitySystem()
        {
            zombieSounds = new Queue<SoundEffectInstance>(20);
            random = new Random();

            AddToQueue(
                "grunt_high_1",
                "grunt_low_4",
                "grunt_high_2",
                "grunt_low_3",
                "grunt_high_4",
                "grunt_med_3",
                "grunt_low_2",
                "grunt_high_3",
                "grunt_low_1",
                "grunt_med_2",
                "grunt_med_1",
                "shout_high_1",
                "shout_med_1",
                "shout_med_2"
                );
            
        }

        public void Update(GameTime gt)
        {
            MediumProbabilityEvent();
        }

        public void AddToQueue(params string[] sounds)
        {
            foreach (var sound in sounds)
            {
                zombieSounds.Enqueue(OutlivedGame.Instance()
                    .Content.Load<SoundEffect>("Sound/zombieSounds/" + sound)
                    .CreateInstance());
            }

        }


        private void MediumProbabilityEvent()
        {

            var zombiesComponent = ComponentManager.Instance.GetEntitiesWithComponent(typeof(AIComponent));

            foreach (var zombie in zombiesComponent)
            {

                var n = random.NextDouble();

                if (n < 0.010)
                {
                    var playerComponent = ComponentManager.Instance.GetEntitiesWithComponent(typeof(PlayerComponent)).First();
                    var player = playerComponent.Value as PlayerComponent;
                    var playerPosition = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(playerComponent.Key);
                    var zombiePosition = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(zombie.Key);
                    var distance = Vector2.Distance(playerPosition.Position, zombiePosition.Position);


                        var soundToPlay = zombieSounds.Dequeue();

                        // The closer the monster is to the player the louder the monster moaning.
                        if (distance < 500)
                        {
                            //play at full volume
                            soundToPlay.Volume = 0.8f;
                        }
                        else
                        {
                            var step = 1 / 1500;
                            var newVolume = 1 - step * (distance - 500);
                            //play sound at newVolume
                            soundToPlay.Volume = newVolume - 0.2f;
                        }

                        if (distance < 2000 && soundToPlay.State != SoundState.Playing)
                        {
                            soundToPlay.Play();
                        }
                    zombieSounds.Enqueue(soundToPlay);

                    
                }
            }

        }

        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }
    }
}
