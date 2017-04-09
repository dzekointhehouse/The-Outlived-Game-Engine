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
            _systemAction = new Action<RenderDependencies>(Render);
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
            graphics.Clear(Color.CornflowerBlue);

            renderDependencies.SpriteBatch.Begin();
            //Texture2D texture = new Texture2D(graphics, 100, 90);
            //Color[] data = new Color[100*90];
            //for (var i = 0; i < data.Length; i++)
            //{
            //    data[i] = Color.Tomato;
            //}
            //texture.SetData(data);
            //Vector2 coor = new Vector2(100, 100);
            //renderDependencies.SpriteBatch.Draw(texture, coor, Color.Chocolate);

            var renderableEntities = ComponentManager.Instance.GetEntitiesWithComponent<RenderComponent>();
            foreach (var entity in renderableEntities)
            {
                var position = entity.Value.PositionComponent;
                if (ComponentManager.EntityHasComponent<SpriteComponent>(entity.Key))
                {
                    var sprite = ComponentManager.GetEntityComponent<SpriteComponent>(entity.Key);
                    spriteBatch.Draw(sprite.Sprite, new Vector2(position.X, position.Y), Color.Transparent);
                }
            }

            renderDependencies.SpriteBatch.End();
        }

        public bool IsRenderable(Dictionary<string, IComponent> entityComponents)
        {
            if (entityComponents.ContainsKey("Render"))
            {
                RenderComponent renderComponent = (RenderComponent) entityComponents["Render"];
                return renderComponent.PositionComponent != null &&
                       (renderComponent.DimensionsComponent != null || renderComponent.Radius > 0);
            }
            return false;
        }
    }
}
