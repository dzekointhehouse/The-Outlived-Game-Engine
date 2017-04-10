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
        private GameDependencies gameDependencies = new GameDependencies();
        private List<ISystem> _systems = new List<ISystem>();
        private KeyboardState _oldKeyboardState = Keyboard.GetState();

        public TestGame()
        {
            gameDependencies.GraphicsDeviceManager = new GraphicsDeviceManager(this);
            gameDependencies.GraphicsDeviceManager.PreferredBackBufferWidth = 900;
            gameDependencies.GraphicsDeviceManager.PreferredBackBufferHeight = 500;
            // We add the spritebatch and the game content we get from the content 
            // pipeline to our gameDependencies, so we can use them in our systems.


            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {


            // We call the method that creates a player.
            CreatePlayer();

            base.Initialize();
        }

        private void CreatePlayer()
        {
            var entityId1 = EntityManager.GetEntityManager().NewEntity();
            var entityId2 = EntityManager.GetEntityManager().NewEntity();

            // Creates Render component
            var renderComponent = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent() { Width = 100, Height = 100 },
                Position = new Vector2(100, 100)
            };
            var renderComponent2 = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent() { Width = 200, Height = 200 },
                Position = new Vector2(100, 100)
            };
            // Adds the component to the entity
            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId1);
            ComponentManager.Instance.AddComponentToEntity(renderComponent2, entityId2);

            // Creates Sprite component
            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "java"
            };

            var spriteComponent2 = new SpriteComponent()
            {
                SpriteName = "Atlantis Nebula UHD"
            };
            // Adds it to the entity
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId1);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent2, entityId2);

            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.W, KeyEvent.KeyPressed, "entityAccelerate")
                .SetAction(Keys.S, KeyEvent.KeyPressed, "entityDeccelerate")
                .SetAction(Keys.A, KeyEvent.KeyPressed, "entityTurnLeft")
                .SetAction(Keys.D, KeyEvent.KeyPressed, "entityTurnRight")
                .Build();
            ComponentManager.Instance.AddComponentToEntity(actionBindings, entityId1);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            gameDependencies.SpriteBatch = new SpriteBatch(GraphicsDevice);
            gameDependencies.GameContent = this.Content;
            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SystemManager.Instance.CreateSystem<LoadContentSystem>().StartSystem(gameDependencies);

            // EventBus.Publish("HandleInput", _oldKeyboardState);
            _oldKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SystemManager.Instance.CreateSystem<RenderSystem>().StartSystem(gameDependencies);
            //EventBus.Publish("Render", gameDependencies);

            base.Draw(gameTime);
        }
    }
}