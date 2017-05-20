using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.Input;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.GameTest;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Helpers.DefaultMaps;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class OutlivedGame : Microsoft.Xna.Framework.Game
    {
        private readonly GameDependencies _gameDependencies = GameDependencies.Instance;
        private KeyboardState _oldKeyboardState = Keyboard.GetState();

        private Vector2 viewportDimensions = new Vector2(1920, 1080); // HD baby!
        public SpriteBatch spriteBatch;

        private readonly FullSystemBundle gameBundle;
        private GameManager _gameManager;

        public OutlivedGame()
        {
            gameBundle = new FullSystemBundle();

            gameBundle.Dependencies.GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int) viewportDimensions.X,
                PreferredBackBufferHeight = (int) viewportDimensions.Y,
            };
            gameBundle.Dependencies.GraphicsDeviceManager.IsFullScreen = false;
            Content.RootDirectory = "Content";
          
        }

        protected override void Initialize()
        {
            //Init systems that require initialization
            gameBundle.InitializeSystems(this);
            _gameManager = new GameManager(gameBundle);

            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            _gameManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _gameManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
