using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class SpriteAnimationComponent : IComponent
    {
        // Maybe have a dictionary here for animations
        // on different occasions.
        public Texture2D Spritesheet { get; set; }

        //THIS SHOULD BE REMOVED WHEN POSSIBLE
        public Point FrameSize { get; set; } // animation frame in pixels (width * height)
        public Point CurrentFrame { get; set; }   // current animation frame index (X, Y)
        public Point SpritesheetSize { get; set; }     // number of frames (X * Y)
        public int TimeSinceLastFrame { get; set; }     // elapsed time since last animation frame
        public int MillisecondsPerFrame { get; set; }  // time to show each animation frame
        ///////////////////////////


        public double AnimationStarted { get; set; }        
        public SpriteAnimationBinding CurrentAnimatedState { get; set; }
        public SpriteAnimationBinding NextAnimatedState { get; set; }

        public SpriteAnimationComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            Spritesheet = null;
            FrameSize = Point.Zero;
            CurrentFrame = Point.Zero;
            SpritesheetSize = Point.Zero;
            TimeSinceLastFrame = 0;
            MillisecondsPerFrame = 50;

            AnimationStarted = 0;
            CurrentAnimatedState = null;
            NextAnimatedState = null;
            return this;
        }
    }
}
