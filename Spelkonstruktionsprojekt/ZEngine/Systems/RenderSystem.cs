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
        private EntityManager EntityManager { get; set; }
        public RenderSystem(EntityManager entityManager)
        {
            EntityManager = entityManager;
        }

        public void Start()
        {
            EventBus.Subscribe<RenderDependencies>("Render", Render);
        }

        public void Stop()
        {
            EventBus.Unsubscribe<RenderDependencies>("Render", Render);
        }

        public void Render(RenderDependencies renderDependencies)
        {
            var graphics = renderDependencies.GraphicsDeviceManager.GraphicsDevice;

            graphics.Clear(Color.CornflowerBlue);

            var renderableEntities = EntityManager.GetEntities().Where(entry => IsRenderable(entry.Value));
            foreach (var entity in renderableEntities)
            {

            }
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
