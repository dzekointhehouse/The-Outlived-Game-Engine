using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class CameraViewComponent : IComponent
    {
        // We store the cameras dimension in
        // this view rectangle.
        public Viewport View { get; set; }

        public Vector2 ViewportDimension { get; set; }

        public Vector2 Center { get; set; }

        // Scale of all the stuff rendered.
        public float Scale { get; set; }

        // A zoom effect can be achieved with the scale
        // value, these max limit is needed so we do not
        // zoom infinetly. We might open a black hole.
        public float MaxScale { get; set; }

        // We also need a min scale limit.
        public float MinScale { get; set; } = 0.1f;

        public Matrix Transform { get; set; }
        public int CameraId { get; set; }

        public CameraViewComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            Center = Vector2.Zero;
            ViewportDimension = Vector2.Zero;
            View = default(Viewport);
            Scale = 1f;
            MaxScale = 1f;
            MinScale = 0.1f;
            CameraId = 0;
            Transform = default(Matrix);
            return this;
        }
    }
}
