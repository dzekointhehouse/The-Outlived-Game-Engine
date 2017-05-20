
using System.ComponentModel;

namespace ZEngine.Components
{
    public class DimensionsComponent : IComponent
    {
        public int Width { get; set; }
        public int Height { get; set; }
        
        public IComponent Reset()
        {
            Width = 0;
            Height = 0;
            return this;
        }
    }
}
