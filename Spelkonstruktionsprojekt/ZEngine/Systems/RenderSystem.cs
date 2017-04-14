using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.Components.MoveComponent;
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

        public static string SystemName = "Render";
        private ComponentManager ComponentManager = ComponentManager.Instance;

        // _____________________________________________________________________________________________________________________ //


        // Render just gets the graphicsdevice and the spritebatch
        // so we can render the entities that are drawn in RenderEntities
        // method.
        public void Render(GameDependencies gm)
        {
            var graphics = gm.GraphicsDeviceManager.GraphicsDevice;
            var spriteBatch = gm.SpriteBatch;

            graphics.Clear(Color.CornflowerBlue); // Maybe done outside
            spriteBatch.Begin(SpriteSortMode.FrontToBack);


            var cameraEntities = ComponentManager.GetEntitiesWithComponent<CameraViewComponent>();
            DrawEntities(spriteBatch, cameraEntities.First().Value.View);
            spriteBatch.End();
        }


        // This method will render all the entities that are associated 
        // with the render component. 1. we use our Component manager instance
        // to get all the entities with RenderComponent and then we render them.
        // we use the spritebach to draw all the entities.
        private void DrawEntities(SpriteBatch spriteBatch, Rectangle subsetView)
        {

            var renderableEntities = 
                ComponentManager.Instance.GetEntitiesWithComponent<RenderComponent>()
                    .Where(e => InsideView(e.Value, subsetView));

            foreach (var entity in renderableEntities)
            {
                var zIndex = entity.Value.PositionComponent.ZIndex;
                var renderBox = new Rectangle((int) entity.Value.PositionComponent.Position.X, (int) entity.Value.PositionComponent.Position.Y, entity.Value.DimensionsComponent.Width, entity.Value.DimensionsComponent.Height);

                if (ComponentManager.EntityHasComponent<SpriteComponent>(entity.Key))
                {
                    var sprite = ComponentManager.GetEntityComponent<SpriteComponent>(entity.Key);

                    double angle = sprite.Angle;
                    if (ComponentManager.EntityHasComponent<MoveComponent>(entity.Key))
                    {
                        var moveComponent = ComponentManager.GetEntityComponent<MoveComponent>(entity.Key);
                        angle = moveComponent.Direction;
                    }

                    sprite.Scale = 1; // For testing, will be removed once feature is actually implemented
                    var destinationRectangle = new Rectangle(
                        new Point(renderBox.X - subsetView.X, renderBox.Y - subsetView.Y),
                        new Point((int) (renderBox.Width * sprite.Scale), (int) (renderBox.Height * sprite.Scale))  
                    );
                    var spriteCrop = new Rectangle(
                        sprite.Position,
                        new Point(sprite.Width, sprite.Height)
                    );

                    var zIndexMaxLimit = 1000;
                    spriteBatch.Draw(
                        texture: sprite.Sprite,
                        destinationRectangle: destinationRectangle,
                        sourceRectangle: spriteCrop,
                        color: Color.White,
                        rotation: (float)angle,
                        origin: new Vector2(x: sprite.Width / 2, y: sprite.Height / 2),
                        effects: SpriteEffects.None,
                        layerDepth: (float)zIndex / zIndexMaxLimit //layerDepth is a float between 0-1, as a result ZIndex will have a dividend (i.e. limit)
                    );                                              
                }
            }


        }

        private bool InsideView(RenderComponent entity, Rectangle view)
        {
            return true;
            var renderBox = new Rectangle((int) entity.PositionComponent.Position.X, (int) entity.PositionComponent.Position.Y, entity.DimensionsComponent.Width, entity.DimensionsComponent.Height);
            return view.Intersects(renderBox);
        }
    }
}
