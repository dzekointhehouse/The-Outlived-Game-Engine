using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using Light = Penumbra.Light;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    // Amazingly this builder was not created by August,
    // ... the creator is....EL Optimus Prime Dzeko
    public class EntityBuilder : IEntityBuilder
    {
        // We store a list of all the components that we add,
        // later when we call build all the components will be 
        // added to the new entity, whiltst creating it's id.
        private readonly List<IComponent> components = new List<IComponent>();
        private readonly int _key = EntityManager.GetEntityManager().NewEntity();

        public int GetEntityKey()
        {
            return _key;
        }

        public EntityBuilder SetPosition(Vector2 position, int layerDepth = 400)
        {
            PositionComponent component = new PositionComponent()
            {
                Position = new Vector2(position.X, position.Y),
                ZIndex = layerDepth
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetHUD(bool isOnlyHUD,string text = null, bool showStats = false)
        {
            RenderHUDComponent component = new RenderHUDComponent()
            {
                HUDtext = text,
                ShowStats = showStats,
                IsOnlyHUD = isOnlyHUD
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetSprite(string spriteName, Point startPosition = default(Point), int tileWidth = 0,
            int tileHeight = 0, float scale = 1f, float alpha = 1f, Texture2D sprite = null)
        {
            SpriteComponent component = new SpriteComponent()
            {
                Alpha = alpha,
                Position = startPosition,
                Sprite = sprite,
                Scale = scale,
                SpriteName = spriteName,
                TileHeight = tileHeight,
                TileWidth = tileWidth
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetLight(Light light)
        {
            LightComponent component = new LightComponent()
            {
                Light = light,
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetDimensions(int width, int height)
        {
            DimensionsComponent component = new DimensionsComponent()
            {
                Width = width,
                Height = height,
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetArtificialIntelligence(float followingDistance = 250)
        {
            AIComponent component = new AIComponent()
            {
                FollowDistance = followingDistance,
            };
            components.Add(component);
            return this;
        }
        public EntityBuilder SetSpawn(int Wavesize)
        {
            SpawnComponent component = new SpawnComponent()
            {
               WaveSize = Wavesize,
               EnemiesDead = 0,

            };
            components.Add(component);
            return this;
        }


        public EntityBuilder SetPlayer(string name)
        {
            PlayerComponent component = new PlayerComponent()
            {
                Name = name,
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetRectangleCollision(bool isCage = false)
        {
            CollisionComponent component = new CollisionComponent()
            {
                IsCage = isCage,
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetHealth(int maxhealth = 100, int currentHealth = 100, bool alive = true)
        {
            HealthComponent component = new HealthComponent()
            {
                Alive = alive,
                CurrentHealth = currentHealth,
                MaxHealth = maxhealth
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetScore()
        {
            EntityScoreComponent component = new EntityScoreComponent()
            {
                score = 0,
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetAmmo()
        {
            AmmoComponent component = new AmmoComponent()
            {
                Amount = 100,
                SpareAmmoAmount = 10,
                OutOfAmmo = 0,
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetTeam(int maxhealth = 100, int currentHealth = 100, bool alive = true)
        {
            TeamComponent component = new TeamComponent()
            {
            };
            components.Add(component);
            return this;
        }
        public EntityBuilder SetSpawn(int WaveSize = 10, int EnemiesDead = 0, bool FirstRound = true)
        {
            SpawnComponent component = new SpawnComponent()
            {
                WaveSize = WaveSize,
                EnemiesDead = EnemiesDead,
                FirstRound = FirstRound
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetSound(string soundname, float volume = 1)
        {
            SoundComponent component = new SoundComponent()
            {
                SoundEffectName = soundname,
                Volume = volume
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetRendering(int width, int height)
        {
            RenderComponent component = new RenderComponent();
            this.SetDimensions(width, height);
            components.Add(component);
            return this;
        }

        public EntityBuilder SetMovement(float maxVelocity, float acceleration, float rotationSpeed, float direction)
        {
            MoveComponent component = new MoveComponent()
            {
                MaxVelocitySpeed = maxVelocity,
                AccelerationSpeed = acceleration,
                RotationSpeed = rotationSpeed,
                Direction = direction
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetInertiaDampening()
        {
            InertiaDampeningComponent component = new InertiaDampeningComponent();
            components.Add(component);
            return this;
        }

        public EntityBuilder SetBackwardsPenalty()
        {
            BackwardsPenaltyComponent component = new BackwardsPenaltyComponent();
            components.Add(component);
            return this;
        }

        public EntityBuilder SetCameraFollow()
        {
            CameraFollowComponent component = new CameraFollowComponent();
            components.Add(component);
            return this;
        }

        // The list with all the components is returned
        // now the user doesn't need to redo the whole process
        // and is able to create an entity that is almost the same.
        public EntityBuilder Build()
        {
            foreach (var component in components)
            {
                ComponentManager.Instance.AddComponentToEntity(component, _key);
            }
            return this;
        }

        public int BuildAndReturnId()
        {
            return Build()
                .GetEntityKey();
        }

        // Here you can set the same components from a previous build
        public EntityBuilder SetCreatedComponents(List<IComponent> alreadyCreatedComponents)
        {
            components.Clear();
            components.AddRange(alreadyCreatedComponents);
            return this;
        }
    }
}