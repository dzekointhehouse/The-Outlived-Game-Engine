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
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public GameTime GameTime { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
    }
}
