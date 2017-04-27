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
    class SpriteAnimationComponent : IComponent
    {
        // Maybe have a dictionary here for animations
        // on different occasions.
        public Texture2D Spritesheet { get; set; }
        public Point FrameSize { get; set; } = default(Point);// animation frame in pixels (width * height)
        public Point CurrentFrame { get; set; }= new Point(0, 0);   // current animation frame index (X, Y)
        public Point SpritesheetSize { get; set; }     // number of frames (X * Y)
        public int TimeSinceLastFrame { get; set; } = 0;     // elapsed time since last animation frame
        public int MillisecondsPerFrame { get; set; }= 50;  // time to show each animation frame
    }
}
