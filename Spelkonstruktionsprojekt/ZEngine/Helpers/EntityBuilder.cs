using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using UnityEngine;
using ZEngine.Components;
using ZEngine.Managers;
using Light = Penumbra.Light;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    // Amazingly this builder was not created by August,
    // ... the creator is....EL Optimus Prime Dzeko
    public class EntityBuilder : IEntityCreator
    {
        // We store a list of all the components that we add,
        // later when we call build all the components will be 
        // added to the new entity, whiltst creating it's id.
        private readonly List<IComponent> components = new List<IComponent>();

        public EntityBuilder AddPosition(Vector2 position, int layerDepth = 400)
        {
            PositionComponent component = new PositionComponent()
            {
                Position = position,
                ZIndex = layerDepth
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder AddSprite(Point position, string spriteName,float scale = 1f, float alpha = 1f)
        {
            SpriteComponent component = new SpriteComponent()
            {
               Alpha = alpha,
               Position = position,
               Scale = scale,
               SpriteName = spriteName,
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

        public EntityBuilder SetArtificialIntelligence(float followingDistance = 250, bool isWandering = true)
        {
            AIComponent component = new AIComponent()
            {
                FollowDistance = followingDistance,
                Wander = isWandering
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetPlayer(string name)
        {
            PlayerComponent component = new PlayerComponent()
            {
                Name = "player name",
            };
            components.Add(component);
            return this;
        }

        public EntityBuilder SetCollision(Rectangle boundingRectangle, bool isCage = false)
        {
            CollisionComponent component = new CollisionComponent()
            {
                IsCage = isCage,
                SpriteBoundingRectangle = boundingRectangle
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

        public EntityBuilder SetTeam(int maxhealth = 100, int currentHealth = 100, bool alive = true)
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

        // The list with all the components is returned
        // now the user doesn't need to redo the whole process
        // and is able to create an entity that is almost the same.
        public List<IComponent> Build()
        {
            int key = EntityManager.GetEntityManager().NewEntity();

            foreach (var component in components)
            {
                ComponentManager.Instance.AddComponentToEntity(component, key);
            }
            return components;
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

