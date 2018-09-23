using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class DefaultViewport : IComponent
    {
        public Viewport Viewport { get; set; }
        public IComponent Reset()
        {
            return this;
        }
    }
}
