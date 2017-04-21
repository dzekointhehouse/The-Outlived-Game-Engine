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
    class SpritesheetComponent : IComponent
    {
        public Texture2D Texture { get; set; }
        public Point FrameSize { get; set; }
        public Point CurrentFrame { get; set; } = new Point(0, 0);
        public Point SheetSize { get; set; }
        public Rectangle AnimationFrame { get; set; }

        public int TimeSinceLastFrame { get; set; } = 0;    // elapsed time since last animation frame
        public int MillisecondsPerFrame { get; set; }= 50;

    }
}
