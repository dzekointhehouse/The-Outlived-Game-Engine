using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Systems;
using ZEngine.EventBus;
using ZEngine.Wrappers;
using static Spelkonstruktionsprojekt.ZEngine.Components.ActionBindings;

namespace Spelkonstruktionsprojekt.ZEngine.GameTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TestGame : Game
    {
        private EventBus EventBus = EventBus.Instance;
        private RenderDependencies RenderDependencies = new RenderDependencies();
        private List<ISystem> systems = new List<ISystem>();
        private KeyboardState _oldKeyboardState = Keyboard.GetState();

        public TestGame()
        {
            RenderDependencies.GraphicsDeviceManager = new GraphicsDeviceManager(this);
            RenderDependencies.GraphicsDeviceManager.PreferredBackBufferWidth = 900;
            RenderDependencies.GraphicsDeviceManager.PreferredBackBufferHeight = 500;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            systems.Add(SystemManager.Instance.CreateSystem("Render").Start());
            systems.Add(SystemManager.Instance.CreateSystem("LoadContent").Start());
            systems.Add(SystemManager.Instance.CreateSystem("HandleInput").Start());

            CreatePlayer();

            base.Initialize();
        }

        private void CreatePlayer()
        {
            var entity = EntityManager.GetEntityManager().NewEntity();

            var renderComponent = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent() { Width = 100, Height = 100 },
                Position = new Vector2Component(100, 100)
            };
            ComponentManager.Instance.AddComponentToEntity(renderComponent, entity);

            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "java"
            };
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entity);

            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.W, KeyEvent.KeyPressed, "entityAccelerate")
                .SetAction(Keys.S, KeyEvent.KeyPressed, "entityDeccelerate")
                .SetAction(Keys.A, KeyEvent.KeyPressed, "entityTurnLeft")
                .SetAction(Keys.D, KeyEvent.KeyPressed, "entityTurnRight")
                .Build();
            ComponentManager.Instance.AddComponentToEntity(actionBindings, entity);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            RenderDependencies.SpriteBatch = new SpriteBatch(GraphicsDevice);
            EventBus.Publish("LoadContent", this.Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            EventBus.Publish("HandleInput", _oldKeyboardState);
            _oldKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            EventBus.Publish("Render", RenderDependencies);

            base.Draw(gameTime);
        }
    }
}