using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Wrappers;

namespace ZEngine.Components
{
    public class RenderComponent : IComponent
    {
        public PositionComponent PositionComponent { get; set; } = null;

        public DimensionsComponent DimensionsComponent { get; set; } = null;

        public double Radius { get; set; } = 0;

        public bool IsVisible { get; set; } = true;
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
    }

    public class RenderComponentBuilder
    {
        private readonly RenderComponent _renderComponent = new RenderComponent();

        public RenderComponent Build()
        {
            return _renderComponent;
        }

        public RenderComponentBuilder Position(int x, int y, int zIndex)
        {
            _renderComponent.PositionComponent = new PositionComponent()
            {
                Position = Vector2D.Create(x, y),
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

        public RenderComponentBuilder IsVisivle(bool isVisible)
        {
            _renderComponent.IsVisible = isVisible;
            return this;
        }
    }
}
