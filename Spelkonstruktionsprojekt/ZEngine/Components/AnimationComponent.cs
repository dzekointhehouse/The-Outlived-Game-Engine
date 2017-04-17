using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class AnimationComponent : IComponent
    {
        public Action<double> Animation = null;

        //Animation takes one parameter
        // <CurrentTimeInMilliseconds>
        public List<Action<double>> Animations = new List<Action<double>>();
    }
}
