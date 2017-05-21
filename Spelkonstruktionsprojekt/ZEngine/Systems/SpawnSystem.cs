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
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class SpawnSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        // Creates the enemy to be spawned.
        public void CreateEnemy(Vector2 position, SpriteComponent sprite)
        {
            var monster = new EntityBuilder()
                .FromLoadedSprite(sprite.Sprite, sprite.SpriteName, new Point(1244, 311), 311, 311)
                .SetPosition(position, layerDepth: 20)
                .SetRendering(100, 100)
                .SetSound("zombiewalking")
                .SetMovement(50, 5, 0.5f, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetArtificialIntelligence()
                .SetSpawn()
                .SetRectangleCollision()
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

            ComponentManager.Instance.AddComponentToEntity(animationBindings, monster);
        }
        public void HandleWaves()
        {
            //GlobalSpawn
            var GlobalSpawnEntities =
             ComponentManager.GetEntitiesWithComponent(typeof(GlobalSpawnComponent));
            if (GlobalSpawnEntities.Count <= 0) return;
            var GlobalSpawnComponent =
                ComponentManager.GetEntityComponentOrDefault<GlobalSpawnComponent>(GlobalSpawnEntities.First().Key);

            GlobalSpawnComponent.EnemiesDead = true;

            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(SpawnComponent)))
            {
                var HealthComponent = ComponentManager.GetEntityComponentOrDefault<HealthComponent>(entity.Key);
                if (HealthComponent == null) continue;

                // if anyone is alive then we set EnemiesDead
                // to false and break out of the loop.
                if (HealthComponent.Alive)
                {
                    GlobalSpawnComponent.EnemiesDead = false;
                    break;
                }
            }

            // If they are all dead
            if (GlobalSpawnComponent.EnemiesDead)
            {
                //SpawnSprite, the sprite for all monsters.
                var SpawnSpriteEntities =
                ComponentManager.GetEntitiesWithComponent(typeof(SpawnFlyweightComponent));

                if (SpawnSpriteEntities.Count <= 0) return;
                var SpawnSpriteComponent = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(SpawnSpriteEntities.First().Key);

                //camera
                List<CameraViewComponent> cameras = new List<CameraViewComponent>(4);
                var cameraEntities =
                ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent));


                Random random = new Random();

                // Looping through the next zombie wave and then
                // adding them if their position is outside of the
                // the players view bounds.
                for (int i = 0; i < GlobalSpawnComponent.WaveSize; i++)
                {
                    CreateEnemy(GetSpawnPosition(cameras, random), SpawnSpriteComponent);
                }
                // When done, increase the wave size...
                if(GlobalSpawnComponent.WaveSize <= GlobalSpawnComponent.MaxLimitWaveSize)
                    GlobalSpawnComponent.WaveSize += GlobalSpawnComponent.WaveSizeIncreaseConstant;



                if (random.Next(0, 1) == 1)
                {
                    if (random.Next(1, 3) == 1)
                    {
                        // Health
                        var HealthpickupEntities =
                        ComponentManager.GetEntitiesWithComponent(typeof(FlyweightPickupComponent));
                        if (HealthpickupEntities.Count == 0)
                        {
                            var HealthpickupComponent =
                                ComponentManager
                                    .GetEntityComponentOrDefault<SpriteComponent>(HealthpickupEntities.First().Key);
                            CreatePickup(1, HealthpickupComponent);
                        }
                    }
                    else
                    {
                        //Ammo
                        var ammoPickUpEntities =
                       ComponentManager.GetEntitiesWithComponent(typeof(FlyweightPickupComponent));
                        if (ammoPickUpEntities.Count == 0)
                        {
                            var ammopickupComponent =
                                ComponentManager
                                    .GetEntityComponentOrDefault<SpriteComponent>(ammoPickUpEntities.Last().Key);
                            CreatePickup(2, ammopickupComponent);
                        }
                    }
                }

            }
        }

        private Vector2 GetSpawnPosition(List<CameraViewComponent> cameras, Random random)
        {
            int x = 0, y = 0;
            bool isInside = true;
            //while (isInside)
            //{
                x = random.Next(0, 5000);
                y = random.Next(0, 5000);
                //foreach (var camera in cameras)
                //{
                //    if (!camera.View.Bounds.Contains(x, y))
                //    {
                //        isInside = false;
                //    }
                //}
          //  }
            
            return new Vector2(x, y);
        }

        public uint CreatePickup(int type, SpriteComponent pickupComponent)
        {
            var entity = new EntityBuilder()
                .SetSound("pickup")
                .SetPosition(new Vector2(40, 40), 100)
                .SetRendering(40, 40)
                .SetLight(new PointLight())
                .SetRectangleCollision()
                .BuildAndReturnId();
            if (type == 1)
            {
                ComponentManager.Instance.AddComponentToEntity(
                    ComponentManager.Instance.ComponentFactory.NewComponent<HealthPickupComponent>(),
                    entity
                );
            }
            else
            {
                ComponentManager.Instance.AddComponentToEntity(
                    ComponentManager.Instance.ComponentFactory.NewComponent<AmmoPickupComponent>(),
                    entity
                );
            }
            ComponentManager.Instance.AddComponentToEntity(pickupComponent, entity);
            return entity;

        }
    }
}
