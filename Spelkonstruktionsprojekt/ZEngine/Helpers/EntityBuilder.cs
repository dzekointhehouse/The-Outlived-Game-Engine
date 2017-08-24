using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using Light = Penumbra.Light;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    // Amazingly this builder was not created by August,
    // ... the creator is....EL Optimus Prime Dzeko
    [System.Runtime.InteropServices.Guid("443EA012-93C3-4E0B-871A-12AF81FD70D6")]
    public class EntityBuilder : IEntityBuilder
    {
        // We store a list of all the components that we add,
        // later when we call build all the components will be 
        // added to the new entity, whiltst creating it's id.
        protected readonly List<IComponent> components = new List<IComponent>();

        private readonly uint _key = EntityManager.GetEntityManager().NewEntity();

        protected ComponentFactory ComponentFactory = ComponentManager.Instance.ComponentFactory;

        public uint GetEntityKey()
        {
            return _key;
        }

        public EntityBuilder SetPosition(Vector2 position, int layerDepth = 400)
        {
            var component = ComponentFactory.NewComponent<PositionComponent>();
            component.Position = new Vector2(position.X, position.Y);
            component.ZIndex = layerDepth;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetHUD(bool isOnlyHUD, string spritefont = "ZEone", string text = null,
            bool showStats = false)
        {
            var component = ComponentFactory.NewComponent<RenderHUDComponent>();
            component.HUDtext = text;
            component.ShowStats = showStats;
            component.IsOnlyHUD = isOnlyHUD;
            components.Add(component);
            component.SpriteFont = spritefont;
            return this;
        }

        public EntityBuilder SetHull()
        {
            var component = ComponentFactory.NewComponent<HullComponent>();
            component.Hull = new Hull(
                new Vector2(1.0f), 
                new Vector2(-1.0f, 1.0f), 
                new Vector2(-1.0f),
                new Vector2(1.0f, -1.0f)
            );
            component.Hull.Scale = new Vector2(25f);
            component.Hull.Position = new Vector2(600, 600);
            components.Add(component);
            return this;
        }
        
        public EntityBuilder SetZone(string[] events)
        {
            var component = ComponentFactory.NewComponent<EventZoneComponent>();
            component.Events = new List<string>(events);
            component.Inhabitants = new HashSet<uint>();
            components.Add(component);
            return this;
        }

        public EntityBuilder SetText(string text, int size, string fontName)
        {
            var component = ComponentFactory.NewComponent<TextComponent>();
            component.Text = text;
            component.Size = size;
            component.SpriteFontName = fontName;
            components.Add(component);
            return this;
        }
        
        public EntityBuilder SetSprite(string spriteName, Point startPosition = default(Point), int tileWidth = 0,
            int tileHeight = 0, float scale = 1f, float alpha = 1f, Texture2D sprite = null)
        {
            var component = ComponentFactory.NewComponent<SpriteComponent>();
            component.Alpha = alpha;
            component.Position = startPosition;
            component.Sprite = sprite;
            component.Scale = scale;
            component.SpriteName = spriteName;
            component.TileHeight = tileHeight;
            component.TileWidth = tileWidth;
            components.Add(component);
            return this;
        }

        public EntityBuilder FromLoadedSprite(Texture2D sprite, string spriteName, Point startPosition = default(Point),
            int tileWidth = 0,
            int tileHeight = 0, float scale = 1f, float alpha = 1f)
        {
            var component = ComponentFactory.NewComponentFromLoadedSprite(sprite, spriteName);
            component.Alpha = alpha;
            component.Position = startPosition;
            component.Scale = scale;
            component.TileHeight = tileHeight;
            component.TileWidth = tileWidth;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetLight(Light light)
        {
            var component = ComponentFactory.NewComponent<LightComponent>();
            component.Light = light;
            light.ShadowType = ShadowType.Solid;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetLightFlickering(double flickerSpeed, double flickerChance)
        {
            var component = ComponentFactory.NewComponent<FlickeringLight>();
            component.FlickerChance = flickerChance;
            component.FlickerSpeed = flickerSpeed;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetDimensions(int width, int height)
        {
            var component = ComponentFactory.NewComponent<DimensionsComponent>();
            component.Width = width;
            component.Height = height;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetArtificialIntelligence(float chasingDistance = 250)
        {
            var component = ComponentFactory.NewComponent<AIComponent>();
            component.FollowDistance = chasingDistance;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetSpawn(int Wavesize)
        {
            var component = ComponentFactory.NewComponent<SpawnComponent>();
            component.WaveSize = Wavesize;
            component.EnemiesDead = 0;
            components.Add(component);
            return this;
        }


        public EntityBuilder SetPlayer(string name)
        {
            var component = ComponentFactory.NewComponent<PlayerComponent>();
            component.Name = name;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetRectangleCollision(bool isCage = false)
        {
            var component = ComponentFactory.NewComponent<CollisionComponent>();
            component.IsCage = isCage;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetHealth(int maxhealth = 100, int currentHealth = 100, bool alive = true)
        {
            var component = ComponentFactory.NewComponent<HealthComponent>();
            component.Alive = alive;
            component.CurrentHealth = currentHealth;
            component.MaxHealth = maxhealth;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetScore()
        {
            var component = ComponentFactory.NewComponent<EntityScoreComponent>();
            component.score = 0;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetAmmo(int cap = 12, int startSpare = 100)
        {
            var component = ComponentFactory.NewComponent<AmmoComponent>();
            component.Amount = cap;
            component.SpareAmmoAmount = startSpare;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetTeam(int teamId)
        {
            var component = ComponentFactory.NewComponent<TeamComponent>();
            component.TeamId = teamId;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetSpawn(int WaveSize = 10, int EnemiesDead = 0, bool FirstRound = true)
        {
            var component = ComponentFactory.NewComponent<SpawnComponent>();
            component.WaveSize = WaveSize;
            component.EnemiesDead = EnemiesDead;
            component.FirstRound = FirstRound;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetSound(string soundName = null,
            Dictionary<SoundComponent.SoundBank, SoundEffectInstance> soundList = null, float volume = 1, SoundEffect soundEffect = null)
        {
            var component = ComponentFactory.NewComponent<SoundComponent>();
            component.SoundList = soundList;
            component.SoundEffectName = soundName;
            component.SoundEffect = soundEffect;
            component.Volume = volume;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetRendering(int width, int height, bool isFixed = false)
        {
            var component = ComponentFactory.NewComponent<RenderComponent>();
            SetDimensions(width, height);
            component.Fixed = isFixed;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetMovement(float maxVelocity, float acceleration, float rotationSpeed, float direction)
        {
            var component = ComponentFactory.NewComponent<MoveComponent>();
            component.MaxVelocitySpeed = maxVelocity;
            component.AccelerationSpeed = acceleration;
            component.RotationSpeed = rotationSpeed;
            component.Direction = direction;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetInertiaDampening(int stabilisingSpeed = 1000)
        {
            var component = ComponentFactory.NewComponent<InertiaDampeningComponent>();
            component.StabilisingSpeed = stabilisingSpeed;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetBackwardsPenalty()
        {
            var component = ComponentFactory.NewComponent<BackwardsPenaltyComponent>();
            components.Add(component);
            return this;
        }

        public EntityBuilder SetCameraFollow(int id)
        {
            var component = ComponentFactory.NewComponent<CameraFollowComponent>();
            component.CameraId = id;
            components.Add(component);
            return this;
        }

        public EntityBuilder SetCameraView(Viewport view, float minScale, int CameraId)
        {
            var component = ComponentFactory.NewComponent<CameraViewComponent>();
            component.View = view;
            component.MinScale = minScale;
            component.CameraId = CameraId;
            component.ViewportDimension = new Vector2(view.Width, view.Height);
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

        public uint BuildAndReturnId()
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