using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Game.Menu;
using Microsoft.Xna.Framework.Content;
using ZEngine.Wrappers;
using System.Diagnostics;

namespace Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class OutlivedGame : Microsoft.Xna.Framework.Game
    {

        private Vector2 viewportDimensions = new Vector2(1920, 1080); // HD baby!
        public SpriteBatch spriteBatch;
        // private readonly GameEngine gameBundle;
        public GraphicsDeviceManager graphics;
        private GameManager _gameManager;
        private OutlivedContent _outlivedContent;

        private static OutlivedGame _outlived;
        
        public Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();


        public OutlivedGame()
        {
            _outlived = this;
           // gameBundle = new GameEngine();

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)viewportDimensions.X,
                PreferredBackBufferHeight = (int)viewportDimensions.Y,
                GraphicsProfile = GraphicsProfile.HiDef

            };
            graphics.IsFullScreen = false;

            Content.RootDirectory = "Content";
        }

        
        
        protected override void Initialize()
        {
            //Init systems that require initialization
            //gameBundle.Start(this);
            
            _outlivedContent = new OutlivedContent(this);
            _outlivedContent.LoadContent();
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            _gameManager = new GameManager();




            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                Debug.WriteLine($"Is Connected");
            _gameManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _gameManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        public static OutlivedGame Instance()
        {
            return _outlived;
        }

        /// <summary>
        /// Easy way to get an asset anywhere in
        /// this game.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public T Get<T>(string assetName)
        {
            var loaded = this.Content.Load<T>(assetName);
            return loaded;
        }

    }
}
