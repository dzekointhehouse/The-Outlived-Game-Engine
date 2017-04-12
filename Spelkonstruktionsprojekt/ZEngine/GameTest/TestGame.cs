using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Components;
using ZEngine.Components.MoveComponent;
using ZEngine.Managers;
using ZEngine.Systems;
using ZEngine.Wrappers;
using static Spelkonstruktionsprojekt.ZEngine.Components.ActionBindings;
using ZEngine.Components.CollisionComponent;

namespace Spelkonstruktionsprojekt.ZEngine.GameTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TestGame : Game
    {
        private readonly RenderDependencies _renderDependencies = new RenderDependencies();
        private KeyboardState _oldKeyboardState = Keyboard.GetState();

        private SpriteComponent spriteTest;

        private RenderSystem RenderSystem;
        private LoadContentSystem LoadContentSystem;
        private InputHandler InputHandlerSystem;
        private MoveSystem MoveSystem;
        private TankMovementSystem TankMovementSystem;
        private TitlesafeRenderSystem TitlesafeRenderSystem;
        private CollisionSystem Collision;

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
            TankMovementSystem = SystemManager.Instance.GetSystem<TankMovementSystem>();
            TitlesafeRenderSystem = SystemManager.Instance.GetSystem<TitlesafeRenderSystem>();
            Collision = SystemManager.Instance.GetSystem<CollisionSystem>();

            TankMovementSystem.Start();
            MoveSystem = SystemManager.Instance.GetSystem<MoveSystem>();

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

            spriteTest = spriteComponent;

            var healthComponent = new HealthComponent()
            {
                CurrentHealth = 70,
                MaxHealth = 100
            };

            var moveComponent = new MoveComponent()
            {
                Velocity = Vector2D.Create(0,0),
                MaxVelocity = Vector2D.Create(100,100),
                Acceleration = Vector2D.Create(0,0),
                MaxAcceleration = Vector2D.Create(80, 80),
                RotationSpeed = 0.1
            };
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.W, KeyEvent.KeyDown, "entityAccelerate")
                .SetAction(Keys.S, KeyEvent.KeyDown, "entityDeccelerate")
                .SetAction(Keys.A, KeyEvent.KeyPressed, "entityTurnLeft")
                .SetAction(Keys.D, KeyEvent.KeyPressed, "entityTurnRight")
                .Build();


            var collisionComponent = new CollisionComponent()
            {
                spriteBoundingRectangle = new Rectangle(0, 0, 5, 5)
            };


            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId1);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId1);
            ComponentManager.Instance.AddComponentToEntity(moveComponent, entityId1);
            ComponentManager.Instance.AddComponentToEntity(actionBindings, entityId1);
            ComponentManager.Instance.AddComponentToEntity(healthComponent, entityId1);
            ComponentManager.Instance.AddComponentToEntity(collisionComponent, entityId1);

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

            // Aeroplane sprite
            var entityId3 = EntityManager.GetEntityManager().NewEntity();
            var renderComponent3 = new RenderComponentBuilder()
                .Position(750, 200, 3)
                .Dimensions(400, 400).Build();
            ComponentManager.Instance.AddComponentToEntity(renderComponent3, entityId3);
            var spriteComponent3 = new SpriteComponent()
            {
                SpriteName = "bmpPlaneUpRight"
            };
            ComponentManager.Instance.AddComponentToEntity(spriteComponent3, entityId3);
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
            //var javas = Content.Load<Texture2D>(@"Images/java");

            //ComponentManager instance = ComponentManager.Instance;
            //var entityList = instance.GetEntitiesWithComponent<CollisionComponent>();
            //var entity = entityList.First();
            //foreach(var key in entityList.Keys)
            //{
            //    var component = entityList[key];
            //}                     

            InputHandlerSystem.HandleInput(_oldKeyboardState);
            _oldKeyboardState = Keyboard.GetState();
            MoveSystem.Move(gameTime);
            Collision.Boundering(spriteTest, _renderDependencies.GraphicsDeviceManager);
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            RenderSystem.Render(_renderDependencies);
            TitlesafeRenderSystem.Render(_renderDependencies);
            base.Draw(gameTime);
        }
    }
}