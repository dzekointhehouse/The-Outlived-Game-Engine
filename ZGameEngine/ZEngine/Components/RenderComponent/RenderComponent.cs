using System.Dynamic;

namespace ZEngine.Components
{
    public class RenderComponent : IComponent
    {
        public bool IsVisible { get; set; }
        public bool Fixed { get; set; }

        public RenderComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            IsVisible = true;
            Fixed = false;
            return this;
        }
    }
}
