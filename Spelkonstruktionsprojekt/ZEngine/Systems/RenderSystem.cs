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
        // _____________________________________________________________________________________________________________________ //

        // We have our singleton instance of the eventbus.
        // The name of the system.
        // The component manager singleton instance.

        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        public static string SystemName = "Render";
        private ComponentManager ComponentManager = ComponentManager.Instance;

        private readonly Action<RenderDependencies> _systemAction;

        // _____________________________________________________________________________________________________________________ //

        public RenderSystem()
        {
            _systemAction = Render;
        }

        // _____________________________________________________________________________________________________________________ //

        // Start the system will subscribe it to the eventbus.
        public ISystem Start()
        {
            EventBus.Subscribe<RenderDependencies>("Render", _systemAction);
            return this;
        }

        // will stop it.
        public ISystem Stop()
        {
            EventBus.Unsubscribe<RenderDependencies>("Render", _systemAction);
            return this;
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
                var position = entity.Value.Position;

                if (ComponentManager.EntityHasComponent<SpriteComponent>(entity.Key))
                {

        

                    var sprite = ComponentManager.GetEntityComponent<SpriteComponent>(entity.Key);
                    
                    spriteBatch.Draw(
                        sprite.Sprite,                                      // texture
                        new Vector2(position.X, position.Y),                // position
                        null,                                               // Source rectangle
                        Color.White,                                        // color
                        sprite.Angle,                                       // rotation
                        new Vector2(x: sprite.Width/2, y: sprite.Height/2), // origin for rotation
                        sprite.Scale,                                       // Scale
                        SpriteEffects.None,                                 // effects
                        1);                                                 // layerdepth
                }
            }
        }

        // Render just gets the graphicsdevice and the spritebatch
        // so we can render the entities that are drawn in RenderEntities
        // method.
        public void Render(RenderDependencies renderDependencies)
        {
            var graphics = renderDependencies.GraphicsDeviceManager.GraphicsDevice;
            var spriteBatch = renderDependencies.SpriteBatch;

            graphics.Clear(Color.CornflowerBlue); // Maybe done outside
            spriteBatch.Begin();
            RenderEntities(spriteBatch);
            spriteBatch.End();
        }
    }
}
