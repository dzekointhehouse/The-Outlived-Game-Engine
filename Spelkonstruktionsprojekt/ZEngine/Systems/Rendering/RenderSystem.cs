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
using Spelkonstruktionsprojekt.ZEngine.Managers;
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

        private RenderComponent renderComponent;
        private MoveComponent moveComponent;
        private RenderOffsetComponent offsetComponent;
        private CameraViewComponent cameraViewComponent;
        // _____________________________________________________________________________________________________________________ //


        // Render just gets the graphicsdevice and the spritebatch
        // so we can render the entities that are drawn in RenderEntities
        // method.
        public void Render(GameDependencies gameDependencies)
        {
            graphics = gameDependencies.GraphicsDeviceManager.GraphicsDevice;

            CameraViewComponent cameraEntities = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent)).First().Value as CameraViewComponent;

            graphics.Clear(Color.Black); // Maybe done outside

            Rectangle cameraView = cameraEntities.View;

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
            Matrix transform = Matrix.Identity *
                            Matrix.CreateTranslation(new Vector3(-cameraView.X, -cameraView.Y, 0)) *
                            Matrix.CreateRotationZ(0) *
                            Matrix.CreateScale(1);

            gameDependencies.SpriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, transform);
            DrawEntities(gameDependencies.SpriteBatch);
            gameDependencies.SpriteBatch.End();
        }

        // This method will render all the entities that are associated 
        // with the render component. 1. we use our Component manager instance
        // to get all the entities with RenderComponent and then we render them.
        // we use the spritebach to draw all the entities.
        private void DrawEntities(SpriteBatch spriteBatch)
        {
            Dictionary<int, IComponent> renderableEntities =
                ComponentManager.Instance.GetEntitiesWithComponent(typeof(RenderComponent));

            foreach (var entity in renderableEntities)
            {
                PositionComponent positionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                if (positionComponent == null) continue;

                SpriteComponent sprite = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(entity.Key);
                if (sprite == null) continue;

                renderComponent = entity.Value as RenderComponent;
                offsetComponent = ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(entity.Key);
                moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entity.Key);

                int zIndex = positionComponent.ZIndex;
                Vector2 offset = offsetComponent?.Offset ?? default(Vector2);
                float angle = moveComponent?.Direction ?? 0;
                Rectangle destinationRectangle =
                    new Rectangle(
                        (int) (positionComponent.Position.X + offset.X),
                        (int) (positionComponent.Position.Y + offset.Y),
                        (int) (RenderComponentHelper.GetDimensions(renderComponent).Width * sprite.Scale),
                        (int) (RenderComponentHelper.GetDimensions(renderComponent).Height * sprite.Scale)
                    );

                // render the sprite only if it's visible (sourceRectangle) intersects
                // with the viewport.
                KeyValuePair<int, IComponent> camera = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent)).First();
                cameraViewComponent = camera.Value as CameraViewComponent;
                if (cameraViewComponent.View.Intersects(destinationRectangle))
                {
                    var spriteCrop = new Rectangle(
                        sprite.Position,
                        new Point(sprite.TileWidth, sprite.TileHeight)
                    );

                    spriteBatch.Draw(
                        texture: sprite.Sprite,
                        destinationRectangle: destinationRectangle,
                        sourceRectangle: spriteCrop,
                        color: Color.White * sprite.Alpha,
                        rotation: (float) angle,
                        origin: new Vector2(x: sprite.TileWidth / 2, y: sprite.TileHeight / 2),
                        effects: SpriteEffects.None,
                        layerDepth: (float) zIndex / SystemConstants.LayerDepthMaxLimit
                        //layerDepth is a float between 0-1, as a result ZIndex will have a dividend (i.e. limit)
                    );
                }
            }
        }

    }
}