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
        private readonly GameDependencies _gameDependencies = new GameDependencies();
        private KeyboardState _oldKeyboardState = Keyboard.GetState();

        private RenderSystem RenderSystem;
        private LoadContentSystem LoadContentSystem;
        private InputHandler InputHandlerSystem;
        private MoveSystem MoveSystem;
        private TankMovementSystem TankMovementSystem;
        private TitlesafeRenderSystem TitlesafeRenderSystem;
        private CollisionSystem CollisionSystem;
        private CameraSceneSystem CameraFollowSystem;
        private Vector2 viewportDimensions = new Vector2(900, 500);

        public TestGame()
        {
            _gameDependencies.GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)viewportDimensions.X,
                PreferredBackBufferHeight = (int)viewportDimensions.Y
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
            CollisionSystem = SystemManager.Instance.GetSystem<CollisionSystem>();
            CameraFollowSystem = SystemManager.Instance.GetSystem<CameraSceneSystem>();

            TankMovementSystem.Start();
            MoveSystem = SystemManager.Instance.GetSystem<MoveSystem>();

            _gameDependencies.GameContent = this.Content;
            _gameDependencies.SpriteBatch = new SpriteBatch(GraphicsDevice);

            CreateTestEntities();

            base.Initialize();
        }

        private void CreateTestEntities()
        {
            var cameraCage = EntityManager.GetEntityManager().NewEntity();
            var renderComponentCage = new RenderComponentBuilder()
                .Position(0, 0, 999)
                .Dimensions((int)viewportDimensions.X, (int)viewportDimensions.Y).Build();
            var collisionComponentCage = new CollisionComponent()
            {
                CageMode = true
            };
            ComponentManager.Instance.AddComponentToEntity(renderComponentCage, cameraCage);
            ComponentManager.Instance.AddComponentToEntity(collisionComponentCage, cameraCage);

            InitPlayers(cameraCage);

            //Initializing a second, imovable, entity
            var entityId3 = EntityManager.GetEntityManager().NewEntity();
            var renderComponent3 = new RenderComponentBuilder()
                .Position(0, 0, 1)
                .Dimensions(1800, 1000).Build();
            ComponentManager.Instance.AddComponentToEntity(renderComponent3, entityId3);
            var spriteComponent3 = new SpriteComponent()
            {
                SpriteName = "Atlantis Nebula UHD"
            };
            ComponentManager.Instance.AddComponentToEntity(spriteComponent3, entityId3);

            var cameraEntity = EntityManager.GetEntityManager().NewEntity();
            var cameraViewComponent = new CameraViewComponent()
            {
                View = new Rectangle(0, 0, (int)viewportDimensions.X, (int)viewportDimensions.Y)
            };
            ComponentManager.Instance.AddComponentToEntity(cameraViewComponent, cameraEntity);
            var cameraRenderable = new RenderComponentBuilder()
                .Position(0, 0, 500)
                .Dimensions(10, 10).Build();
            var cameraSprite = new SpriteComponent()
            {
                SpriteName = "dot"
            };
            ComponentManager.Instance.AddComponentToEntity(cameraRenderable, cameraEntity);
            ComponentManager.Instance.AddComponentToEntity(cameraSprite, cameraEntity);
        }

        public void InitPlayers(int cageId)
        {
            var player1 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings1 = new ActionBindingsBuilder()
                .SetAction(Keys.W, "entityWalkForwards")
                .SetAction(Keys.S, "entityWalkBackwards")
                .SetAction(Keys.A, "entityTurnLeft")
                .SetAction(Keys.D, "entityTurnRight")
                .Build();
            var cameraFollow1 = new CameraFollowComponent();
            var cageComponent = new CageComponent()
            {
                CageId = cageId
            };
            ComponentManager.Instance.AddComponentToEntity(cameraFollow1, player1);
            //ComponentManager.Instance.AddComponentToEntity(cageComponent, player1);


            //var player2 = EntityManager.GetEntityManager().NewEntity();
            //var actionBindings2 = new ActionBindingsBuilder()
            //    .SetAction(Keys.I, "entityWalkForwards")
            //    .SetAction(Keys.K, "entityWalkBackwards")
            //    .SetAction(Keys.J, "entityTurnLeft")
            //    .SetAction(Keys.L, "entityTurnRight")
            //    .Build();

            //var player3 = EntityManager.GetEntityManager().NewEntity();
            //var actionBindings3 = new ActionBindingsBuilder()
            //    .SetAction(Keys.Up, "entityWalkForwards")
            //    .SetAction(Keys.Down, "entityWalkBackwards")
            //    .SetAction(Keys.Left, "entityTurnLeft")
            //    .SetAction(Keys.Right, "entityTurnRight")
            //    .Build();
            //var cameraFollow2 = new CameraFollowComponent();
            //ComponentManager.Instance.AddComponentToEntity(cameraFollow2, player2);

            CreatePlayer(player1, actionBindings1);
            //CreatePlayer(player2, actionBindings2);
            //CreatePlayer(player3, actionBindings3);
        }

        public void CreatePlayer(int entityId, ActionBindings actionBindings)
        {
            //Initializing first, movable, entity
            var renderComponent = new RenderComponentBuilder()
                .Position(150, 150, 2)
                .Dimensions(100, 100).Build();
            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "topDownSoldier"
            };
            var moveComponent = new MoveComponent()
            {
                Velocity = Vector2D.Create(0, 0),
                Acceleration = Vector2D.Create(0, 0),
                MaxAcceleration = Vector2D.Create(80, 80),
                MaxVelocitySpeed = 200,
                AccelerationSpeed = 380,
                RotationSpeed = 4
            };

            CollisionComponent collisionComponent = new CollisionComponent();

            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(moveComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(actionBindings, entityId);
            ComponentManager.Instance.AddComponentToEntity(collisionComponent, entityId);


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
            MoveSystem.Move(gameTime);
            CollisionSystem.CheckInsideAndOutsideCollision();
            CameraFollowSystem.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            RenderSystem.Render(_gameDependencies);
            //TitlesafeRenderSystem.Render(_gameDependencies);
            base.Draw(gameTime);
        }
    }
}