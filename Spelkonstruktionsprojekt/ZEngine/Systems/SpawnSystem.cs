using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using System.Threading;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class SpawnSystem : ISystem
    {
        bool FirstRound = true;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void CreateEnemy(int x, int y, SpriteComponent SpawnSpriteComponent)
        {

            var monster = new EntityBuilder()
                .SetPosition(new Vector2(x, y), layerDepth: 20)
                .SetRendering(200, 200)
                .SetSound("zombiewalking")
                .SetMovement(205, 5, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetArtificialIntelligence()
                .SetRectangleCollision()
                .SetSpawn()
                .SetHealth()
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
            ComponentManager.Instance.AddComponentToEntity(SpawnSpriteComponent, monster);
            ComponentManager.Instance.AddComponentToEntity(animationBindings, monster);
        }
        public void HandleWaves()
        {
            //GlobalSpawn
            var GlobalSpawnEntities =
             ComponentManager.Instance.GetEntitiesWithComponent(typeof(GlobalSpawnComponent));
            if (GlobalSpawnEntities.Count <= 0) return;
            var GlobalSpawnComponent =
                ComponentManager.GetEntityComponentOrDefault<GlobalSpawnComponent>(GlobalSpawnEntities.First().Key);
            GlobalSpawnComponent.EnemiesDead = true;
            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(SpawnComponent)))
            {
                var HealthComponent = ComponentManager.GetEntityComponentOrDefault<HealthComponent>(entity.Key);
                if (HealthComponent == null) continue;
                if (HealthComponent.Alive)
                {
                    GlobalSpawnComponent.EnemiesDead = false;
                    // if anyone is alive break
                    break;
                }
                


            }
            if (GlobalSpawnComponent.EnemiesDead)
            {
                //SpawnSprite, the sprite for all monsters.
                var SpawnSpriteEntities =
                    ComponentManager.Instance.GetEntitiesWithComponent(typeof(SpawnFlyweightComponent));
                if (SpawnSpriteEntities.Count <= 0) return;
                var SpawnSpriteComponent =
                    ComponentManager.Instance
                        .GetEntityComponentOrDefault<SpriteComponent>(SpawnSpriteEntities.First().Key);

              
                int x = 500;
                int y = 500;
                for (int i = 0; i < GlobalSpawnComponent.WaveSize; i++)
                {
                    CreateEnemy(x, y, SpawnSpriteComponent);
                    x += 100;
                    y += 100;
                }

                GlobalSpawnComponent.WaveSize += GlobalSpawnComponent.WaveSizeIncreaseConstant;
            }


        }


        // Creates a wave wavesize big.. with enemies
        public void SetupWave(int wavesize, SpriteComponent SpawnSpriteComponent)
        {
            ////   int x = new Random(DateTime.Now.Millisecond).Next(1000, 3000);
            ////     int y = new Random(DateTime.Now.Millisecond).Next(1000, 3000);'
            int x = 800;
            int y = 800;
            ////example on how to do the wave
            ////we go through a loop that gives us places for each enemy to spawn and we create wavesize amount of enemies.
            for (int i = 1; i <= wavesize; i++)
            {
                //CreateEnemy(x, y, SpawnSpriteComponent);
            }
        }


      
    }
}
