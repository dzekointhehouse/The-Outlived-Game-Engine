using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Constants;
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

        public static string SystemName = "Render";
        private ComponentManager ComponentManager = ComponentManager.Instance;
        private GraphicsDevice graphics;

        // _____________________________________________________________________________________________________________________ //


        // Render just gets the graphicsdevice and the spritebatch
        // so we can render the entities that are drawn in RenderEntities
        // method.
        public void Render(GameDependencies gm)
        {
            graphics = gm.GraphicsDeviceManager.GraphicsDevice;
            var spriteBatch = gm.SpriteBatch;

            var cameraEntities = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent)).First().Value as CameraViewComponent;

            graphics.Clear(Color.Black); // Maybe done outside

            var cameraView = cameraEntities.View;

            //    Matrix.CreateScale(new Vector3(camera.Scale, camera.Scale, camera.Scale));
            // Using a matrix makes it easier for us to move the camera
            // independently of all the sprites, which means that we easily can
            // rotate, scale, etc. without much effort. plus its recommended.
            // What we do when are multiplying matrices is that we combine them
            // so the result will be a matrix that does the combination of it's 
            // products. Now when we use this transform in the begindraw, it will
            // affect all the stuff that is drawn after it.
            // We create a translation matrix so we are able to move our points easily 
            // from one place to another. 
            // X,Y and Z, ofcourse Z will be 0.
            // We won't be having any rotation.
            // Our zoom effect will be doing its jobb here,
            // as this matrix will easily help us achieve it.
            var transform = Matrix.Identity *
                            Matrix.CreateTranslation(new Vector3(-cameraView.X, -cameraView.Y, 0)) *
                            Matrix.CreateRotationZ(0) *
                            Matrix.CreateScale(1);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, transform);
            DrawEntities(spriteBatch, cameraEntities.View);
            spriteBatch.End();
        }

        // This method will render all the entities that are associated 
        // with the render component. 1. we use our Component manager instance
        // to get all the entities with RenderComponent and then we render them.
        // we use the spritebach to draw all the entities.
        private void DrawEntities(SpriteBatch spriteBatch, Rectangle subsetView)
        {
            var renderableEntities =
                ComponentManager.Instance.GetEntitiesWithComponent(typeof(RenderComponent))
                    .Where(e =>
                    {
                        var renderComponent = e.Value as RenderComponent;
                        return InsideView(renderComponent, subsetView);
                    });

            foreach (var entity in renderableEntities)
            {
                var positionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                if(positionComponent == null) continue
                var sprite = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(entity.Key);
                if (sprite == null) continue;


                var zIndex = positionComponent.ZIndex;
                var renderComponent = entity.Value as RenderComponent;
                var renderBox = new Rectangle((int) positionComponent.Position.X,
                    (int) positionComponent.Position.Y,
                    RenderComponentHelper.GetDimensions(renderComponent).Width,
                    RenderComponentHelper.GetDimensions(renderComponent).Height);

                double angle = sprite.Angle;
                if (ComponentManager.EntityHasComponent<MoveComponent>(entity.Key))
                {
                    var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
                    angle = moveComponent.Direction;
                }

                var offset = ComponentManager.EntityHasComponent<RenderOffsetComponent>(entity.Key)
                    ? ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(entity.Key).Offset
                    : default(Vector2);

                //var offset = Vector2.Zero;
                var destinationRectangle = new Rectangle(
                    new Point((int) (renderBox.X + offset.X), (int) (renderBox.Y + offset.Y)),
                    new Point((int) (renderBox.Width * sprite.Scale), (int) (renderBox.Height * sprite.Scale))
                );


                // render the sprite only if it's visible (sourceRectangle) intersects
                // with the viewport.
                var camera = ComponentManager.Instance.GetEntitiesWithComponent(CameraViewComponent).First();
                var cameraViewComponent = camera.Value as CameraViewComponent;
                if (cameraViewComponent.View.Intersects(destinationRectangle))
                {
                    var spriteCrop = sprite.SourceRectangle;
                    if (spriteCrop == default(Rectangle))
                    {
                        spriteCrop = new Rectangle(
                            sprite.Position,
                            new Point(sprite.Width, sprite.Height)
                        );
                    }

                    // We have color so we can use transparency for instance.
                    var spriteColor = sprite.SpriteColor;
                    if (sprite.SpriteColor == default(Color))
                        spriteColor = Color.White;

                    // limit can be changed in SystemConstants
                    var zIndexMaxLimit = 1;


                    spriteBatch.Draw(
                        texture: sprite.Sprite,
                        destinationRectangle: destinationRectangle,
                        sourceRectangle: spriteCrop,
                        color: spriteColor * sprite.Alpha,
                        rotation: (float) angle,
                        origin: new Vector2(x: sprite.Width / 2, y: sprite.Height / 2),
                        effects: SpriteEffects.None,
                        layerDepth: (float) zIndex / zIndexMaxLimit
                        //layerDepth is a float between 0-1, as a result ZIndex will have a dividend (i.e. limit)
                    );
                }
            }
        }

        private bool InsideView(RenderComponent entity, Rectangle view)
        {
            return true;
            //var renderBox = new Rectangle((int)entity.positionComponent.Position.X, (int)entity.PositionComponent.Position.Y, RenderComponentHelper.GetDimensions(entity).Width, RenderComponentHelper.GetDimensions(entity).Height);
            //return view.Intersects(renderBox);
        }
    }
}