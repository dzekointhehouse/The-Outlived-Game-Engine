using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Game.Entities;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Game.Systems
{
    public class SpawnPointSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public SpawnPointSystem Start()
        {
            EventBus.Subscribe<SpawnPointEvent>(EventConstants.InitiateSpawnPoint, InitiateSpawnPoint);
            EventBus.Subscribe<InputEvent>(EventConstants.SpawnPointTest, Test);
            return this;
        }

        public SpawnPointSystem Stop()
        {
            EventBus.Unsubscribe<SpawnPointEvent>(EventConstants.InitiateSpawnPoint, InitiateSpawnPoint);
            return this;
        }

        public void Test(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var positionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(inputEvent.EntityId);
                if (positionComponent == null) return;
                
                EventBus.Publish(EventConstants.InitiateSpawnPoint, new SpawnPointEvent
                {
                    EventTime = inputEvent.EventTime,
                    Position = new Vector2(positionComponent.Position.X, positionComponent.Position.Y),
                    SpawnDelay = 5000,
                    ZombieCount = 5
                });
            }
        }

        public void InitiateSpawnPoint(SpawnPointEvent spawnPointEvent)
        {
            
            var sprite = GameEnemies.GetZombieSpriteComponentOrNull();
            if (sprite == null)
                throw new Exception("Cannot spawn zombies when global sprite component is not yet loaded!");

//            var spawnPoint = EntityManager.GetEntityManager().NewEntity();
            //TODO Remove before DEMO, only for testing
            var spawnPoint = new EntityBuilder()
                .SetRendering(50, 50)
                .SetPosition(spawnPointEvent.Position)
                .BuildAndReturnId();
            
            ComponentManager.AddComponentToEntity(sprite, spawnPoint);
            
            
            var animationComponent = GetOrCreateDefault(spawnPoint);
            var animation = new GeneralAnimation()
            {
                AnimationType = "SpawnPoint",
                StartOfAnimation = spawnPointEvent.EventTime,
                Length = spawnPointEvent.SpawnDelay,
                Unique = true
            };
            NewSpawnPointAnimation(animation, sprite, spawnPointEvent, spawnPoint);
            animationComponent.Animations.Add(animation);
        }


        public void NewSpawnPointAnimation(GeneralAnimation generalAnimation, SpriteComponent spriteComponent,
            SpawnPointEvent spawnPointEvent, uint spawnPoint)
        {
//            GameEnemies.NewZombie(spawnPointEvent.Position, spriteComponent);
            var zombiesSpawned = 0;
            generalAnimation.Animation = delegate(double currentTimeInMilliseconds)
            {
                if (currentTimeInMilliseconds - generalAnimation.StartOfAnimation > generalAnimation.Length)
                {
                    GameEnemies.NewZombie(spawnPointEvent.Position, spriteComponent);
                    zombiesSpawned++;
                    if (zombiesSpawned >= spawnPointEvent.ZombieCount)
                    {
                        var tagComponent = ComponentManager.GetEntityComponentOrDefault<TagComponent>(spawnPoint);
                        if (tagComponent == null)
                        {
                            throw new Exception(
                                "Entity does not have a tag component which is needed to remove the entity.");
                        }
                        tagComponent.Tags.Add(Tag.Delete);
                        generalAnimation.IsDone = true;
                    }
                    else
                    {
                        generalAnimation.StartOfAnimation = currentTimeInMilliseconds;
                    }
                }
            };
        }

        private AnimationComponent GetOrCreateDefault(uint entityId)
        {
            var animationComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<AnimationComponent>(entityId);
            if (animationComponent != null) return animationComponent;

            animationComponent = ComponentManager.Instance.ComponentFactory.NewComponent<AnimationComponent>();
            ComponentManager.Instance.AddComponentToEntity(animationComponent, entityId);
            return animationComponent;
        }

        public class SpawnPointEvent
        {
            public double EventTime { get; set; }
            public int ZombieCount { get; set; } = 10;
            public double SpawnDelay { get; set; } = 5000;
            public Vector2 Position { get; set; }
        }
    }
}