using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
        private GraphicsDevice graphicsDevice;

        private RenderComponent renderComponent;
        private MoveComponent moveComponent;
        //private RenderOffsetComponent offsetComponent;
        private DimensionsComponent dimensionsComponent;
        public float GameScale { get; set; } = 1.0f;
        private CameraViewComponent cameraViewComponent;

        // _____________________________________________________________________________________________________________________ //


        // Render just gets the graphicsdevice and the spritebatch
        // so we can render the entities that are drawn in RenderEntities
        // method.
        public void Render(GameDependencies gameDependencies)
        {

            graphicsDevice = gameDependencies.GraphicsDeviceManager.GraphicsDevice;
            graphicsDevice.Clear(Color.Black); // Maybe done outside

            var cameraComponents = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent));

            foreach (var cameraComponent in cameraComponents)
            {
                var camera = cameraComponent.Value as CameraViewComponent;
                graphicsDevice.Viewport = camera.View;

                gameDependencies.SpriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null,
                    camera.Transform);

                //Temporary

                var border = GameDependencies.Instance.Game.Content.Load<Texture2D>("border");

                gameDependencies.SpriteBatch.Draw(border, Vector2.Zero, Color.White);
                //---------

                DrawEntities(gameDependencies.SpriteBatch);

                gameDependencies.SpriteBatch.End();
            }
        }



        // This method will render all the entities that are associated 
        // with the render component. 1. we use our Component manager instance
        // to get all the entities with RenderComponent and then we render them.
        // we use the spritebach to draw all the entities.
        private void DrawEntities(SpriteBatch spriteBatch)
        {
            var renderableEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(RenderComponent));

            foreach (var entity in renderableEntities)
            {
                var positionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                if (positionComponent == null) continue;

                var sprite = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(entity.Key);
                if (sprite == null) continue;

                renderComponent = entity.Value as RenderComponent;
                //offsetComponent = ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(entity.Key);
                moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
                dimensionsComponent = ComponentManager.GetEntityComponentOrDefault<DimensionsComponent>(entity.Key);
                
                int zIndex = positionComponent.ZIndex;
                //var offset = offsetComponent?.Offset ?? default(Vector2);
                float angle = moveComponent?.Direction ?? 0;
                var destinationRectangle =
                    new Rectangle(
                        (int) (positionComponent.Position.X),
                        (int) (positionComponent.Position.Y),
                        (int) (dimensionsComponent.Width * sprite.Scale),
                        (int) (dimensionsComponent.Width * sprite.Scale)
                    );

                if ( true)
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
                        origin: new Vector2(x: (float) (sprite.TileWidth * 0.5), y: (float) (sprite.TileHeight * 0.5)),
                        effects: SpriteEffects.None,
                        layerDepth: (float) zIndex / SystemConstants.LayerDepthMaxLimit
                        //layerDepth is a float between 0-1, as a result ZIndex will have a dividend (i.e. limit)
                    );
                }
            }
        }
    }
}