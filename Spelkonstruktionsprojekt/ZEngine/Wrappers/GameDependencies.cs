using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine.Wrappers
{
    public class GameDependencies
    {
        // Singleton pattern is used here for the ComponentManager,
        // Instance is used heer to get that only instance.
        public static GameDependencies Instance => LazyInitializer.Value;
        private static readonly Lazy<GameDependencies> LazyInitializer = new Lazy<GameDependencies>(() => new GameDependencies());

        // These are som of the dependencies that we will need for our
        // fine monogame engine.
        // they will probably be used in the systems we have created.
        // For instance, we need spritebach for our render component.
        public GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public Object GameContent { get; set; }

        public Game Game { get; set; }
    }
}   
