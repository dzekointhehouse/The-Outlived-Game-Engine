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


        // Render just gets the graphicsdevice and the spritebatch
        // so we can render the entities that are drawn in RenderEntities
        // method.
        public void Render(RenderDependencies gm)
        {
            var graphics = gm.GraphicsDeviceManager.GraphicsDevice;
            var spriteBatch = gm.SpriteBatch;

            graphics.Clear(Color.CornflowerBlue); // Maybe done outside
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            DrawEntities(spriteBatch);
            spriteBatch.End();
        }


        // This method will render all the entities that are associated 
        // with the render component. 1. we use our Component manager instance
        // to get all the entities with RenderComponent and then we render them.
        // we use the spritebach to draw all the entities.
        private void DrawEntities(SpriteBatch spriteBatch)
        {
            var renderableEntities = ComponentManager.Instance.GetEntitiesWithComponent<RenderComponent>();

            foreach (var entity in renderableEntities)
            {
                var position = entity.Value.PositionComponent.Position;
                var zIndex = entity.Value.PositionComponent.ZIndex;

                if (ComponentManager.EntityHasComponent<SpriteComponent>(entity.Key))
                {
                    var sprite = ComponentManager.GetEntityComponent<SpriteComponent>(entity.Key);
                    sprite.Scale = 1;
                    var destinationRectangle = new Rectangle(
                        new Point((int) position.X, (int) position.Y),
                        new Point((int) (entity.Value.DimensionsComponent.Width * sprite.Scale), (int) (entity.Value.DimensionsComponent.Height * sprite.Scale))  
                    );
                    var spriteCrop = new Rectangle(
                        sprite.Position,
                        new Point(sprite.Width, sprite.Height)
                    );

                    var zIndexMaxLimit = 1000;
                    spriteBatch.Draw(
                        texture: sprite.Sprite,  
                        destinationRectangle:  destinationRectangle,                     
                        sourceRectangle: spriteCrop,                                               
                        color: Color.White,                                        
                        rotation: sprite.Angle,                                       
                        origin: new Vector2(x: sprite.Width / 2, y: sprite.Height / 2), 
                        effects: SpriteEffects.None,                                 
                        layerDepth: (float) zIndex / zIndexMaxLimit //layerDepth is a float between 0-1, as a result ZIndex will have a dividend (i.e. limit)
                    );                                                 
                }
            }
        }
    }
}
