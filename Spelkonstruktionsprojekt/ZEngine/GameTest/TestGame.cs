using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Systems;
using ZEngine.Wrappers;
using static Spelkonstruktionsprojekt.ZEngine.Components.ActionBindings;

namespace Spelkonstruktionsprojekt.ZEngine.GameTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TestGame : Game
    {
        private readonly RenderDependencies _renderDependencies = new RenderDependencies();
        private KeyboardState _oldKeyboardState = Keyboard.GetState();

        private RenderSystem RenderSystem;
        private LoadContentSystem LoadContentSystem;
        private InputHandler InputHandlerSystem;
        public TestGame()
        {
            _renderDependencies.GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 900,
                PreferredBackBufferHeight = 500
            };
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            RenderSystem = SystemManager.Instance.GetSystem<RenderSystem>();
            LoadContentSystem = SystemManager.Instance.GetSystem<LoadContentSystem>();
            InputHandlerSystem = SystemManager.Instance.GetSystem<InputHandler>();

            _renderDependencies.GameContent = this.Content;
            _renderDependencies.SpriteBatch = new SpriteBatch(GraphicsDevice);

            CreateTestEntities();

            base.Initialize();
        }

        private void CreateTestEntities()
        {
            //Initializing first, movable, entity
            var entityId1 = EntityManager.GetEntityManager().NewEntity();
            var renderComponent = new RenderComponentBuilder()
                .Position(150, 150, 2)
                .Dimensions(100, 100).Build();
            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "java"
            };
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.W, KeyEvent.KeyPressed, "entityAccelerate")
                .SetAction(Keys.S, KeyEvent.KeyPressed, "entityDeccelerate")
                .SetAction(Keys.A, KeyEvent.KeyPressed, "entityTurnLeft")
                .SetAction(Keys.D, KeyEvent.KeyPressed, "entityTurnRight")
                .Build();
            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId1);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId1);
            ComponentManager.Instance.AddComponentToEntity(actionBindings, entityId1);

            //Initializing a second, imovable, entity
            var entityId2 = EntityManager.GetEntityManager().NewEntity();
            var renderComponent2 = new RenderComponentBuilder()
                .Position(0, 0, 1)
                .Dimensions(1800, 1000).Build();
            ComponentManager.Instance.AddComponentToEntity(renderComponent2, entityId2);
            var spriteComponent2 = new SpriteComponent()
            {
                SpriteName = "Atlantis Nebula UHD"
            };
            ComponentManager.Instance.AddComponentToEntity(spriteComponent2, entityId2);
        }

        protected override void LoadContent()
        {
            LoadContentSystem.LoadContent(this.Content);
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            InputHandlerSystem.HandleInput(_oldKeyboardState);
            _oldKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            RenderSystem.Render(_renderDependencies);
            base.Draw(gameTime);
        }
    }
}