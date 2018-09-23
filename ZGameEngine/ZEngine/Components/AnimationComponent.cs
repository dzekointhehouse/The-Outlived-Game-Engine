using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class AnimationComponent : IComponent
    {
        public List<GeneralAnimation> Animations = new List<GeneralAnimation>();

        public IComponent Reset()
        {
            Animations.Clear();
            return this;
        }
    }
}
