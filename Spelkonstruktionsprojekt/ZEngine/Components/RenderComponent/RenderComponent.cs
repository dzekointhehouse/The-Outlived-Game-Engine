using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace ZEngine.Components
{
    public class RenderComponent : IComponent
    {
        //public PositionComponent PositionComponent = null;

        public DimensionsComponent DimensionsComponent = null;

        public double Radius = 0;

        public bool IsVisible = true;

        public bool Fixed = false;
    }

    public class RenderComponentHelper
    {
        public static DimensionsComponent GetDimensions(RenderComponent renderComponent)
        {
            return renderComponent.DimensionsComponent ?? new DimensionsComponent()
            {
                Width = (int) renderComponent.Radius * 2,
                Height = (int) renderComponent.Radius * 2
            };
        }
/*
        public static Vector2 GetPosition(int entityId, RenderComponent renderComponent = null)
        {
            if (renderComponent == null && ComponentManager.Instance.EntityHasComponent<RenderComponent>(entityId))
            {
                renderComponent = ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(entityId);
            }
            if (ComponentManager.Instance.EntityHasComponent<RenderOffsetComponent>(entityId))
            {
                var offsetComponent = ComponentManager.Instance.GetEntityComponentOrDefault<RenderOffsetComponent>(entityId);
                return new Vector2(
                    (float) (renderComponent.PositionComponent.Position.X + offsetComponent.Offset.X),
                    (float) (renderComponent.PositionComponent.Position.Y + offsetComponent.Offset.Y)
                );
            }
            return default(Vector2);
        }
    }
*/
/*    public class RenderComponentBuilder
    {
        private readonly RenderComponent _renderComponent = new RenderComponent();

        public RenderComponent Build()
        {
            if (_renderComponent.PositionComponent == null)
            {
                _renderComponent.PositionComponent = new PositionComponent();
            }
            return _renderComponent;
        }

        public RenderComponentBuilder Position(int x, int y, int zIndex)
        {
            _renderComponent.PositionComponent = new PositionComponent()
            {
                Position = new Vector2(x,y),
                ZIndex = zIndex
            };
            return this;
        }

        public RenderComponentBuilder Position(float x, float y, int zIndex)
        {
            _renderComponent.PositionComponent = new PositionComponent()
            {
                Position = new Vector2(x, y),
                ZIndex = zIndex
            };
            return this;
        }

        public RenderComponentBuilder Dimensions(int width, int height)
        {
            _renderComponent.DimensionsComponent = new DimensionsComponent()
            {
                Width = width,
                Height = height
            };
            return this;
        }

        public RenderComponentBuilder Radius(int radius)
        {
            _renderComponent.Radius = radius;
            return this;
        }

        public RenderComponentBuilder IsVisible(bool isVisible)
        {
            _renderComponent.IsVisible = isVisible;
            return this;
        }

        public RenderComponentBuilder Fixed(bool isFixed)
        {
            _renderComponent.Fixed = isFixed;
            return this;
        }
    */}
}
