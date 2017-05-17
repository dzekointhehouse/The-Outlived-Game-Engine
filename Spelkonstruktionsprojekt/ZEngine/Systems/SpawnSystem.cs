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

                Rectangle rect = new Rectangle();
                
                Random rand = new Random();
                int spawnpoint = rand.Next(2500,3000);
                int spawnpoint2 = rand.Next(-2000, 0);

                int x = spawnpoint / 2;
                int y = spawnpoint / 2;
                for (int i = 0; i < GlobalSpawnComponent.WaveSize; i++)
                {
                    CreateEnemy(x, y, SpawnSpriteComponent);
                    x = spawnpoint2/2;
                    y = spawnpoint2 / 2;
                }

                GlobalSpawnComponent.WaveSize += GlobalSpawnComponent.WaveSizeIncreaseConstant;
            }


        }


      
    }
}
