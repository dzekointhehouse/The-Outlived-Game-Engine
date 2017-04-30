using System;
using ZEngine.Components;

namespace Zenu.components
{
    public class ButtonComponent : IComponent
    {
        public Action Link { get; set; }
    }
}