using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class CameraViewComponent : IComponent
    {
        // We store the cameras dimension in
        // this view rectangle.
        public Rectangle View { get; set; }

        // Scale of all the stuff rendered.
        public float Scale { get; set; } = 1f;

        // A zoom effect can be achieved with the scale
        // value, these max limit is needed so we do not
        // zoom infinetly. We might open a black hole.
        public float MaxScale { get; set; } = 1f;

        // We also need a min scale limit.
        public float MinScale { get; set; } = 0.1f;
    }
}
