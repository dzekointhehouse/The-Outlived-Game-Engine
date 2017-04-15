using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class CameraViewComponent : IComponent
    {
        public Rectangle View { get; set; }

        public float Scale { get; set; } = 1f;

        public float MaxScale { get; set; } = 1f;

        public float MinScale { get; set; } = 0.1f;
    }
}
