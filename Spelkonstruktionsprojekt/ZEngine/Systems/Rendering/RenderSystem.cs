using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.EventBus;
using ZEngine.Wrappers;
using IComponent = ZEngine.Components.IComponent;

namespace ZEngine.Systems
{
    public class RenderSystem : ISystem, IDrawables
    {
        // _____________________________________________________________________________________________________________________ //

        // We have our singleton instance of the eventbus.
        // The name of the system.
        // The component manager singleton instance.
        public bool Enabled { get; set; } = true;
        public int DrawOrder { get; set; }
        public void Draw()
        {
        }
        public static string SystemName = "Render";
        private ComponentManager ComponentManager = ComponentManager.Instance;
        //private GraphicsDevice graphicsDevice;

        private RenderComponent renderComponent;

        private MoveComponent moveComponent;

        //private RenderOffsetComponent offsetComponent;
        private DimensionsComponent dimensionsComponent;

        public float GameScale { get; set; } = 1.0f;
        private CameraViewComponent cameraViewComponent;

        // _____________________________________________________________________________________________________________________ //

        private readonly Dictionary<uint, Tuple<PositionComponent, DimensionsComponent, SpriteComponent, MoveComponent>> _cache =
            new Dictionary<uint, Tuple<PositionComponent, DimensionsComponent, SpriteComponent, MoveComponent>>();

        public Tuple<PositionComponent, DimensionsComponent, SpriteComponent, MoveComponent>
            GetOrRetrieveComponents(uint entityId)
        {
            Tuple<PositionComponent, DimensionsComponent, SpriteComponent, MoveComponent> data;
            var foundDataInCache = _cache.TryGetValue(entityId, out data);
            if (foundDataInCache) return data;

            var positionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entityId);
            if (positionComponent == null) return null;

            var sprite = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(entityId);
            if (sprite == null) return null;

            moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entityId);

            dimensionsComponent = ComponentManager.GetEntityComponentOrDefault<DimensionsComponent>(entityId);
            if (dimensionsComponent == null) return null;

            data =
                new Tuple<PositionComponent, DimensionsComponent, SpriteComponent, MoveComponent>(
                    positionComponent,
                    dimensionsComponent,
                    sprite,
                    moveComponent
                );

            _cache[entityId] = data;
            return data;
        }

        // Render just gets the graphicsdevice and the spritebatch
        // so we can render the entities that are drawn in RenderEntities
        // method.
        public void Render(SpriteBatch sb, GameTime gameTime)
        {

            sb.GraphicsDevice.Clear(Color.Black); // Maybe done outside
          //  var timer = Stopwatch.StartNew();

            var cameraComponents = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent));

            foreach (var cameraComponent in cameraComponents)
            {
                var camera = cameraComponent.Value as CameraViewComponent;
                sb.GraphicsDevice.Viewport = camera.View;

               // penumbra.GraphicsDevice.Viewport = camera.View;
                //penumbra.Transform = camera.Transform;
                //gameDependencies.GraphicsDeviceManager.ApplyChanges();
                //penumbra.BeginDraw();

                sb.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null,
                    camera.Transform);
                //var border = GameDependencies.Instance.Game.Content.Load<Texture2D>("border");
                //gameDependencies.SpriteBatch.Draw(border, Vector2.Zero, Color.White);
                DrawEntities(sb);
                sb.End();

               // penumbra.Draw(gameTime);

              //  Debug.WriteLine("DrawEntities TOTAL: " + timer.ElapsedTicks);
            }

            //timer.Stop();
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
                var components = GetOrRetrieveComponents(entity.Key);
                if (components == null) continue;
                if(!(entity.Value as RenderComponent).IsVisible) continue;
                
                var zIndex = components.Item1.ZIndex;

                //var offset = offsetComponent?.Offset ?? default(Vector2);
                var angle = components.Item4?.Direction ?? 0;
                var destinationRectangle =
                    new Rectangle(
                        (int) (components.Item1.Position.X),
                        (int) (components.Item1.Position.Y),
                        (int) (components.Item2.Width * components.Item3.Scale),
                        (int) (components.Item2.Height * components.Item3.Scale)
                    );

                var spriteCrop = new Rectangle(
                    components.Item3.Position,
                    new Point(components.Item3.TileWidth, components.Item3.TileHeight)
                );

                spriteBatch.Draw(
                    texture: components.Item3.Sprite,
                    destinationRectangle: destinationRectangle,
                    sourceRectangle: spriteCrop,
                    color: Color.White * components.Item3.Alpha,
                    rotation: (float) angle,
                    origin: new Vector2(x: (float) (components.Item3.TileWidth * 0.5), y: (float) (components.Item3.TileHeight * 0.5)),
                    effects: SpriteEffects.None,
                    layerDepth: (float) zIndex / SystemConstants.LayerDepthMaxLimit
                    //layerDepth is a float between 0-1, as a result ZIndex will have a dividend (i.e. limit)
                );
            }
        }

        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}