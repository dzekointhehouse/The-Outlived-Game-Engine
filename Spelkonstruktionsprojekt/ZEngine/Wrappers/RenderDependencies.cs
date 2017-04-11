using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine.Wrappers
{
    public class RenderDependencies
    { 
        // These are som of the dependencies that we will need for our
        // fine monogame engine.
        // they will probably be used in the systems we have created.
        // For instance, we need spritebach for our render component.
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public GameTime GameTime { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteFont SpritFont { get; set; }
        public Object GameContent { get; set; }
    }
}   
