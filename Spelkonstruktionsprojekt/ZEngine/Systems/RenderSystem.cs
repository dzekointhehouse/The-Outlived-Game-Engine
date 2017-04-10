using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.EventBus;
using ZEngine.Wrappers;
using IComponent = ZEngine.Components.IComponent;

namespace ZEngine.Systems
{
    public class RenderSystem : ISystem
    {
        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        public static string SystemName = "Render";
        private EntityManager EntityManager = EntityManager.GetEntityManager();
        private ComponentManager ComponentManager = ComponentManager.Instance;

        private readonly Action<RenderDependencies> _systemAction;

        public RenderSystem()
        {
            _systemAction = Render;
        }

        public ISystem Start()
        {
            EventBus.Subscribe<RenderDependencies>("Render", _systemAction);
            return this;
        }

        public ISystem Stop()
        {
            EventBus.Unsubscribe<RenderDependencies>("Render", _systemAction);
            return this;
        }

        public void Render(RenderDependencies renderDependencies)
        {
            var graphics = renderDependencies.GraphicsDeviceManager.GraphicsDevice;
            var spriteBatch = renderDependencies.SpriteBatch;

            graphics.Clear(Color.CornflowerBlue); // Maybe done outside
            spriteBatch.Begin();
            RenderEntities(spriteBatch);
            spriteBatch.End();
        }

        // This method will render all the entities that are associated 
        // with the render component. 1. we use our Component manager instance
        // to get all the entities with RenderComponent and then we render them.
        // we use the spritebach to draw all the entities.
        private void RenderEntities(SpriteBatch spriteBatch)
        {
            var renderableEntities = ComponentManager.Instance.GetEntitiesWithComponent<RenderComponent>();
            foreach (var entity in renderableEntities)
            {
                var position = entity.Value.Position.Vectors;
                if (ComponentManager.EntityHasComponent<SpriteComponent>(entity.Key))
                {
                    var sprite = ComponentManager.GetEntityComponent<SpriteComponent>(entity.Key);
                    spriteBatch.Draw(sprite.Sprite, new Vector2(position.X, position.Y), Color.White);
                }
            }
        }
    }
}
