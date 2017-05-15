using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;
namespace Spelkonstruktionsprojekt.ZEngine.Systems
{    
    class SpawnSystem : ISystem
    {
        
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void HandleWaves() {
            SetupWave(40);
            foreach (var entitys in ComponentManager.GetEntitiesWithComponent(typeof(SpawnComponent)))
            {
                var spawn = entitys.Value as SpawnComponent;
                foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(AIComponent)))
                {
                    
                    //if (spawn == null) { Debug.WriteLine("spawn is null u dumbass"); break; }
                   
                    if (ComponentManager.EntityHasComponent<HealthComponent>(entity.Key))
                    {
                        var HealthComponent = ComponentManager.GetEntityComponentOrDefault<HealthComponent>(entity.Key);
                        if (!HealthComponent.Alive)
                        {
                            spawn.EnemiesDead++;
                        }

                    }
                    if (spawn.EnemiesDead == spawn.WaveSize)
                    {
                        spawn.WaveSize += spawn.WaveSizeIncreaseConstant;
                        //    can we do this ?? there might be a problem with doing it like this.
                        SetupWave(spawn.WaveSize);
                    }

                }

            }
        }

        public void SetupWave(int wavesize) {
            int x = new Random(DateTime.Now.Millisecond).Next(1000, 3000);
            int y = new Random(DateTime.Now.Millisecond).Next(1000, 3000);
            //example on how to do the wave
            //we go through a loop that gives us places for each enemy to spawn and we create wavesize amount of enemies.
            for (int i = 1; i <= wavesize; i++) {
                SetupEnemy(x, y);
            }



        }


        public void SetupEnemy(int x, int y)
        {
            //var x = new Random(DateTime.Now.Millisecond).Next(1000, 3000);
            //var y = new Random(DateTime.Now.Millisecond).Next(1000, 3000);

            var monster = new EntityBuilder()
                .SetPosition(new Vector2(x, y), layerDepth: 20)
                .SetRendering(200, 200)
                .SetSprite("player_sprites", new Point(1252, 206), 313, 206)
                .SetSound("zombiewalking")
                .SetMovement(205, 5, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetArtificialIntelligence()
                .SetRectangleCollision()
                .SetHealth()
                //.SetHUD("hello")
                .BuildAndReturnId();

            var animationBindings = new SpriteAnimationBindingsBuilder()
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(1252, 206), new Point(0, 1030))
                        .StateConditions(State.WalkingForward)
                        .Length(40)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingForward)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingBackwards)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Build();

//            ComponentManager.Instance.AddComponentToEntity(animationBindings, monster);
        }
    }
}
