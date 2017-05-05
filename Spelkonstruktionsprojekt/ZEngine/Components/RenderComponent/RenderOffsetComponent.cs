using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent
{
    public class RenderOffsetComponent : IComponent
    {
        public Vector2 Offset = default(Vector2);
    }
}
