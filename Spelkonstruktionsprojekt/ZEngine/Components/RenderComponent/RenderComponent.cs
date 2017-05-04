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
    }
}
