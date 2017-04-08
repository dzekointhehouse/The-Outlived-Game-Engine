using System;
using System.Collections.Generic;
using System.Linq;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.EventBus;
using ZEngine.Wrappers;
using IComponent = ZEngine.Components.IComponent;

namespace ZEngine.Systems
{
    public class RenderSystem : ISystem
    {
        public static string SystemName = "Render";
        private EntityManager EntityManager { get; set; }
        public RenderSystem(EntityManager entityManager)
        {
            EntityManager = entityManager;
        }

        public void Start()
        {
            EventBus.EventBus._.Subscribe<RenderDependencies>("Render", Render);
        }

        public void Stop()
        {
            EventBus.EventBus._.Unsubscribe<RenderDependencies>("Render", Render);
        }

        public void Render(RenderDependencies renderDependencies)
        {
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
            else
            {
                return false;
            }
        }
    }

}
