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
        public double ElapsedTime = 0;
        public double LenghtInSeconds;
        public bool Loop = false;

        //Animation takes three parameters
        // <ElapsedTime>
        public Action<double> Animation = null;

    }
}
