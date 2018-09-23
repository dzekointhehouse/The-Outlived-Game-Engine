using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using TheOutlivedGL;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Game.Systems
{
    public class SpawnSystem : ISystem, IUpdateables
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;
        private EventBus Eventbus = EventBus.Instance;
        private readonly Random _random = new Random();
        private CountdownTimer _timer = new CountdownTimer(minutes: 0, seconds: 5);

        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        public void Start()
        {
            Eventbus.Subscribe<uint>(EventConstants.PlayerDeath, RespawnPlayers);
        }

        public void Update(GameTime gameTime)
        {
            _timer.UpdateTimer(gameTime);
//            Debug.WriteLine(_timer.Seconds);

            //World
            var worldComponent = ComponentManager.Instance.GetEntitiesWithComponent(typeof(WorldComponent)).First();
            var world = worldComponent.Value as WorldComponent;

            //GlobalSpawn
            var globalSpawnEntities =
                ComponentManager.GetEntitiesWithComponent(typeof(GlobalSpawnComponent));
            if (globalSpawnEntities.Count <= 0) return;

            var globalSpawnComponent =
                ComponentManager.GetEntityComponentOrDefault<GlobalSpawnComponent>(globalSpawnEntities.First().Key);

            globalSpawnComponent.EnemiesDead = true;

            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(SpawnComponent)))
            {
                var healthComponent = ComponentManager.GetEntityComponentOrDefault<HealthComponent>(entity.Key);
                if (healthComponent == null) continue;

                // if anyone is alive then we set EnemiesDead
                // to false and break out of the loop.
                if (healthComponent.IsAlive)
                {
                    globalSpawnComponent.EnemiesDead = false;
                    break;
                }
                _timer.StartCounter();
            }

            // If they are all dead
            if (globalSpawnComponent.EnemiesDead && _timer.IsDone)
            {
                // if all the enemies are dead, reset timer and count to next wave.
                _timer.Reset();

                var waveHud =
                    ComponentManager.Instance.GetEntityComponentOrDefault<RenderHUDComponent>(globalSpawnEntities
                        .First().Key);
                var nCameras = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent))
                    .Count;

                if (waveHud != null && nCameras == 1)
                {
                    waveHud.HUDtext = "Wave " + globalSpawnComponent.WaveLevel.ToString();
                    waveHud.IsOnlyHUD = true;
                    waveHud.Color = Color.DarkGray;
                }

                //SpawnSprite, the sprite for all monsters.
                var spawnSpriteEntities =
                    ComponentManager.GetEntitiesWithComponent(typeof(SpawnFlyweightComponent));

                if (spawnSpriteEntities.Count <= 0) return;
                var spawnSpriteComponent =
                    ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(spawnSpriteEntities.First().Key);

                var cameraComponents = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent));

                // Looping through the next zombie wave and then
                // adding them if their position is outside of the
                // the players view bounds.
                for (int i = 0; i < globalSpawnComponent.WaveSize; i++)
                {
                    CreateEnemy(GetSpawnPosition(world, cameraComponents, _random), spawnSpriteComponent);
                }
                // When done, increase the wave size...
                if (globalSpawnComponent.WaveSize <= globalSpawnComponent.MaxLimitWaveSize)
                    globalSpawnComponent.WaveSize += globalSpawnComponent.WaveSizeIncreaseConstant;

                // We increase the wave level, this is used to display progress.
                globalSpawnComponent.WaveLevel++;

                var waveSound = OutlivedGame.Instance().Content.Load<SoundEffect>("Sound/NextWave")
                    .CreateInstance();
                waveSound.Volume = 0.7f;
                if (waveSound.State == SoundState.Stopped)
                {
                    waveSound.Play();
                }


                if (_random.Next(0, 1) == 1)
                {
                    if (_random.Next(1, 3) == 1)
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

        // Spawing the zombies outside of the players view.
        private Vector2 GetSpawnPosition(WorldComponent world, Dictionary<uint, IComponent> cameraComponents,
            Random random)
        {
            if (cameraComponents == null) return default(Vector2);
            int x = 0, y = 0;
            var worldCenter = new Point(world.WorldWidth / 2, world.WorldHeight / 2);
            bool isInside = true;
            while (isInside)
            {
                x = random.Next(0, world.WorldWidth);
                y = random.Next(0, world.WorldHeight);
                foreach (var cameraComponent in cameraComponents)
                {
                    var camera = cameraComponent.Value as CameraViewComponent;
                    var cameraDimensions = camera.View.Bounds.Size;
                    var bounds = new Rectangle(
                        new Point(
                            (int) (worldCenter.X - cameraDimensions.X * 0.5), 
                            (int) (worldCenter.Y - cameraDimensions.Y * 0.5)),
                        cameraDimensions);
                    if (world.WorldData == null)
                    {
                        if (!bounds.Contains(x, y))
                        {
                            isInside = false;
                        }
                    }
                    else
                    {
                        Color color = world.WorldData[y, x];
                        if (color.Equals(Color.Yellow))
                        {
                            isInside = false;
                        }
                    }
                }
            }
            //Debug.WriteLine("outside");

            return new Vector2(x, y);
        }


        public Vector2 GetSpawnPositionBasedOnColorData(WorldComponent world, Random random, Color spawnColor)
        {
            int x = 0, y = 0;
            bool isInside = true;
            while (isInside)
            {
                x = random.Next(0, world.WorldWidth);
                y = random.Next(0, world.WorldHeight);

                Color color = world.WorldData[y, x];
                if (color.Equals(spawnColor))
                {
                    isInside = false;
                }
            }
            //Debug.WriteLine("outside");

            return new Vector2(x, y);
        }

        public uint CreatePickup(int type, SpriteComponent pickupComponent)
        {
            var entity = new EntityBuilder()
                .SetSound("pickup")
                .SetPosition(new Vector2(_random.Next(0,3000), _random.Next(0, 3000)), 100)
                .SetRendering(40, 40)
                .SetLight(new PointLight())
                .SetCollision()
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

        // Creates the enemy to be spawned.
        public void CreateEnemy(Vector2 position, SpriteComponent sprite)
        {
            Dictionary<SoundComponent.SoundBank, SoundEffectInstance> soundList =
                new Dictionary<SoundComponent.SoundBank, SoundEffectInstance>(1);

            soundList.Add(SoundComponent.SoundBank.Death, OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/Splash")
                .CreateInstance());

            var monster = new EntityBuilder()
                .FromLoadedSprite(sprite.Sprite, sprite.SpriteName, new Point(1244, 311), 311, 311)
                .SetPosition(position, layerDepth: 20)
                .SetRendering(200, 200)
                .SetSound(soundList: soundList)
                .SetMovement(50, 5, 0.5f, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetArtificialIntelligence()
                .SetSpawn()
                .SetCollision()
                .SetHealth()
                .BuildAndReturnId();

            var animationBindings = new SpriteAnimationBindingsBuilder()
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(1244, 311), new Point(622, 1244))
                        .StateConditions(State.WalkingForward)
                        .Length(60)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(933, 311))
                        .StateConditions(State.Dead, State.WalkingForward)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(933, 311))
                        .StateConditions(State.Dead, State.WalkingBackwards)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(933, 311))
                        .StateConditions(State.Dead)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Build();

            ComponentManager.Instance.AddComponentToEntity(animationBindings, monster);

            //TODO SEND STATE MANAGER A GAME TIME VALUE AND NOT 0
            StateManager.TryAddState(monster, State.WalkingForward, 0);
        }

        private void RespawnPlayers(uint playerEntity)
        {
            var playerComponents = ComponentManager.Instance.GetEntityComponentOrDefault<PlayerComponent>(playerEntity);
            //World
            var worldComponent = ComponentManager.Instance.GetEntitiesWithComponent(typeof(WorldComponent)).First();
            var world = worldComponent.Value as WorldComponent;

            var positionComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(playerEntity);
            positionComponent.Position = GetSpawnPositionBasedOnColorData(world, _random, Color.Red);
        }
    }
}