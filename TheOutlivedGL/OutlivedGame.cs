using Game;
using Game.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace TheOutlivedGL
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class OutlivedGame : Microsoft.Xna.Framework.Game
    {
        public SpriteBatch spriteBatch;
        public GraphicsDeviceManager graphics;

        private Vector2 viewportDimensions = new Vector2(1920, 1080); // HD baby!
        private GameManager _gameManager;
        private OutlivedContent _outlivedContent;
        private static OutlivedGame _outlived;

        public Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();


        public OutlivedGame()
        {
            _outlived = this;

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)viewportDimensions.X,
                PreferredBackBufferHeight = (int)viewportDimensions.Y,
                GraphicsProfile = GraphicsProfile.HiDef

            };

#if (DEBUG == true)
            graphics.IsFullScreen = false;
#else
            graphics.IsFullScreen = true;
#endif
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
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
