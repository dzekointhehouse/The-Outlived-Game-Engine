using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Helpers.DefaultMaps;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Systems;
using ZEngine.Systems.Collisions;
using ZEngine.Systems.InputHandler;
using ZEngine.Wrappers;
using static Spelkonstruktionsprojekt.ZEngine.Components.ActionBindings;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;

namespace Spelkonstruktionsprojekt.ZEngine.GameTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TestGame : Game
    {
        private static ComponentFactory ComponentFactory = ComponentManager.Instance.ComponentFactory;

        // private readonly GameDependencies Dependencies = new GameDependencies();
        private KeyboardState _oldKeyboardState = Keyboard.GetState();

        private Video video;
        private VideoPlayer player;
        private Vector2 viewportDimensions = new Vector2(1800, 1300);

        private Viewport defaultView;
        private Viewport leftView;
        private Viewport rightView;

        private PenumbraComponent penumbraComponent;

        // testing
        private FPS fps;

        public SpriteBatch spriteBatch;
        private Song musicTest;
        private SystemManager manager = SystemManager.Instance;
        private SpriteFont scoreFont;
        private Texture2D gameOver;

        private readonly FullSystemBundle gameBundle;

        // Game states
        private enum GameState
        {
            Start,
            InGame,
            GameOver
        };

        private GameState currentGameState = GameState.Start;

        public TestGame()
        {
            gameBundle = new FullSystemBundle();

            gameBundle.Dependencies.GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int) viewportDimensions.X,
                PreferredBackBufferHeight = (int) viewportDimensions.Y
            };
            Content.RootDirectory = "Content";

            // Create an instance of the FPS GameComponent


            // Turn off the fixed time step
            // and the synchronization with the vertical retrace
            // so the game's FPS can be measured
            fps = new FPS(this);


            IsFixedTimeStep = false;
            gameBundle.Dependencies.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;


        }

        protected override void Initialize()
        {
            //Init systems that require initialization
            gameBundle.InitializeSystems(this);
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            defaultView = GraphicsDevice.Viewport;
            leftView = defaultView;
            rightView = defaultView;

            // from left to middle
            leftView.Width = leftView.Width / 2;
            // from middle to right
            rightView.X = leftView.Width;
            rightView.Width = rightView.Width / 2;

            CreateTestEntities();
            base.Initialize();
        }

        private void CreateTestEntities()
        {
            // var button = new Button();
           // var cameraCageId = SetupCameraCage();
            InitPlayers();
            SetupBackgroundTiles(5, 5);
           // SetupCamera();
          //  SetupEnemy();
            SetupCamera();
            SetupHUD();
            CreateGlobalBulletSpriteEntity();
            CreateGlobalSpawnSpriteEntity();
            CreateGlobalSpawnEntity();
            CreateFlyweightHealthpickupEntity();
            SetupTempPlayerDeadSpriteFlyweight();
            CreateFlyweightAmmopickupEntity();
            SetupGameScoreEntity();
            // AddPickup();
        }

        public void AddPickup()
        {
            var entity = new EntityBuilder()
                .SetSound("pickup")
                .SetSprite("healthpickup")
                .SetPosition(new Vector2(40, 40), 100)
                .SetRendering(40, 40)
                .SetLight(new PointLight())
                .SetRectangleCollision()
                .BuildAndReturnId();

            ComponentManager.Instance.AddComponentToEntity(
                ComponentManager.Instance.ComponentFactory.NewComponent<HealthPickupComponent>(),
                entity
            );
        }

        private void SetupGameScoreEntity()
        {
            var gameScoreComponent = ComponentManager.Instance.ComponentFactory.NewComponent<GameScoreComponent>();

            ComponentManager.Instance.AddComponentToEntity(gameScoreComponent,
                EntityManager.GetEntityManager().NewEntity());
        }

        private void SetupHUD()
        {
            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(viewportDimensions.X * 0.3f, viewportDimensions.Y * 0.83f))
                .SetSprite("health3_small")
                .Build();

            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(viewportDimensions.X * 0.3f, viewportDimensions.Y * 0.88f))
                .SetSprite("medal")
                .Build();

            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(viewportDimensions.X * 0.28f, viewportDimensions.Y * 0.92f))
                .SetSprite("ammo")
                .Build();
        }

        private void SetupTempPlayerDeadSpriteFlyweight()
        {
            var tempEntity = EntityManager.GetEntityManager().NewEntity();
            var spriteComponent = ComponentFactory.NewComponent<SpriteComponent>();
            spriteComponent.SpriteName = "dot";
            var tempDeadSpriteComponent = ComponentFactory.NewComponent<TempPlayerDeadSpriteComponent>();
            ComponentManager.Instance.AddComponentToEntity(tempDeadSpriteComponent, tempEntity);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, tempEntity);
        }

        private static void CreateFlyweightHealthpickupEntity()
        {
            var entity = EntityManager.GetEntityManager().NewEntity();
            var HealthpickupSprite = ComponentFactory.NewComponent<SpriteComponent>();
            HealthpickupSprite.SpriteName = "healthpickup";
            var healthSpriteComponent = ComponentFactory.NewComponent<FlyweightPickupComponent>();
            var soundComponent = ComponentFactory.NewComponent<SoundComponent>();
            soundComponent.SoundEffectName = "pickup";
            ComponentManager.Instance.AddComponentToEntity(soundComponent, entity);
            ComponentManager.Instance.AddComponentToEntity(HealthpickupSprite, entity);
            ComponentManager.Instance.AddComponentToEntity(healthSpriteComponent, entity);
        }

        private static void CreateFlyweightAmmopickupEntity()
        {
            var entity = EntityManager.GetEntityManager().NewEntity();
            var ammopickupSprite = ComponentFactory.NewComponent<SpriteComponent>();
            ammopickupSprite.SpriteName = "knife";
            var ammoPickUpSprite = ComponentFactory.NewComponent<FlyweightPickupComponent>();
            var soundComponent = ComponentFactory.NewComponent<SoundComponent>();
            soundComponent.SoundEffectName = "pickup";
            ComponentManager.Instance.AddComponentToEntity(soundComponent, entity);
            ComponentManager.Instance.AddComponentToEntity(ammopickupSprite, entity);
            ComponentManager.Instance.AddComponentToEntity(ammoPickUpSprite, entity);
        }

        private static void CreateGlobalBulletSpriteEntity()
        {
            var entity = EntityManager.GetEntityManager().NewEntity();
            var bulletSprite = ComponentFactory.NewComponent<SpriteComponent>();
            bulletSprite.SpriteName = "dot";
            var bulletSpriteComponent = ComponentFactory.NewComponent<BulletFlyweightComponent>();
            var soundComponent = ComponentFactory.NewComponent<SoundComponent>();
            soundComponent.SoundEffectName = "bullet9mm";
            ComponentManager.Instance.AddComponentToEntity(soundComponent, entity);
            ComponentManager.Instance.AddComponentToEntity(bulletSprite, entity);
            ComponentManager.Instance.AddComponentToEntity(bulletSpriteComponent, entity);
        }

        private static void CreateGlobalSpawnSpriteEntity()
        {
            var spawnSprite = EntityManager.GetEntityManager().NewEntity();
            var spawnSpriteSprite = ComponentFactory.NewComponent<SpriteComponent>();
            spawnSpriteSprite.SpriteName = "Player_Sprites";
            var SpawnSpriteComponent = ComponentFactory.NewComponent<SpawnFlyweightComponent>();
            ComponentManager.Instance.AddComponentToEntity(spawnSpriteSprite, spawnSprite);
            ComponentManager.Instance.AddComponentToEntity(SpawnSpriteComponent, spawnSprite);
        }

        private static void CreateGlobalSpawnEntity()
        {
            var spawn = EntityManager.GetEntityManager().NewEntity();
            var spawncomponent = ComponentFactory.NewComponent<GlobalSpawnComponent>();
            // var spawnSpawnComponent = new GlobalSpawnComponent();
            ComponentManager.Instance.AddComponentToEntity(spawncomponent, spawn);
            //ComponentManager.Instance.AddComponentToEntity(spawnSpawnComponent, spawn);
        }

        //The camera cage keeps players from reaching the edge of the screen
        public uint SetupCameraCage()
        {
            var cameraCage = new EntityBuilder()
                .SetRendering((int) (viewportDimensions.X * 0.8), (int) (viewportDimensions.Y * 0.8))
                .SetRectangleCollision()
                .SetPosition(Vector2.Zero, 2)
                .BuildAndReturnId();

            var offsetComponent = new RenderOffsetComponent()
            {
                Offset = new Vector2((float) (viewportDimensions.X * 0.25), (float) (viewportDimensions.Y * 0.25))
            };
            ComponentManager.Instance.AddComponentToEntity(offsetComponent, cameraCage);
            return cameraCage;
        }

        public void SetupBackgroundTiles(int width, int height)
        {
            var tileTypes = new Dictionary<int, string>();

            //tileTypes.Add(0, "blue64");
            tileTypes.Add(28, "blue64");
            tileTypes.Add(2, "red64");
            //tileTypes.Add(4, "yellowwall64");


            MapHelper mapcreator = new MapHelper(tileTypes);

            mapcreator.CreateMap(MapPack.TheWallMap, 100);
        }

        public void SetupCamera()
        {
            var cameraEntity = EntityManager.GetEntityManager().NewEntity();
            var cameraViewComponent = ComponentFactory.NewComponent<CameraViewComponent>();
            cameraViewComponent.View = defaultView;
            cameraViewComponent.MinScale = 0.5f;
            cameraViewComponent.ViewportDimension = new Vector2(viewportDimensions.X, viewportDimensions.Y);

            var position = ComponentFactory.NewComponent<PositionComponent>();
            position.Position = Vector2.Zero;
            position.ZIndex = 500;

            ComponentManager.Instance.AddComponentToEntity(cameraViewComponent, cameraEntity);
            ComponentManager.Instance.AddComponentToEntity(position, cameraEntity);
        }

        public void SetupEnemy()
        {
            var x = new Random(DateTime.Now.Millisecond).Next(1000, 3000);
            var y = new Random(DateTime.Now.Millisecond).Next(1000, 3000);

            var monster = new EntityBuilder()
                .SetPosition(new Vector2(x, y), layerDepth: 20)
                .SetRendering(200, 200)
                .SetSprite("player_sprites", new Point(1252, 206), 313, 206)
                .SetSound("zombiewalking")
                .SetMovement(205, 5, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetArtificialIntelligence()
                .SetSpawn()
                .SetRectangleCollision()
                .SetHealth()
                //.SetHUD("hello")
                .BuildAndReturnId();

            var animationBindings = new SpriteAnimationBindingsBuilder()
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(1252, 206), new Point(0, 1030))
                        .StateConditions(State.WalkingForward)
                        .Length(40)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingForward)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingBackwards)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Build();

            ComponentManager.Instance.AddComponentToEntity(animationBindings, monster);
        }

        public void InitPlayers()
        {
            var actionBindings1 = new ActionBindingsBuilder()
                .SetAction(Keys.W, EventConstants.WalkForward) //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FirePistolWeapon)
                .SetAction(Keys.LeftShift, EventConstants.LightStatus)
                .SetAction(Keys.R, EventConstants.ReloadWeapon)
                .Build();

            var actionBindings2 = new ActionBindingsBuilder()
                .SetAction(Keys.I, EventConstants.WalkForward)
                .SetAction(Keys.K, EventConstants.WalkBackward)
                .SetAction(Keys.J, EventConstants.TurnLeft)
                .SetAction(Keys.L, EventConstants.TurnRight)
                .SetAction(Keys.O, EventConstants.FirePistolWeapon)
                .SetAction(Keys.U, EventConstants.TurnAround)
                .SetAction(Keys.H, EventConstants.LightStatus)
                .SetAction(Keys.P, EventConstants.ReloadWeapon)
                .Build();

            var actionBindings3 = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FirePistolWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();

            CreatePlayer(
                new Vector2(1650, 1100),
                name: "Carlos",
                actionBindings: actionBindings1,
                position: new Vector2(200, 200),
                cameraFollow: true,
                collision: true, 
                cageId: 1,
                view: leftView);

            CreatePlayer(
                new Vector2(1650, 1100),
                "Elvir", actionBindings2,
                position: new Vector2(400, 400),
                cameraFollow: true,
                collision: true,
                cageId: 2,
                disabled: false,
                view: rightView);
            //CreatePlayer("Markus", player3, actionBindings3, position: new Vector2(300, 300), cameraFollow: true,
            //    collision: true, isCaged: false, cageId: cageId, disabled: true);
        }

        //The multitude of options here is for easy debug purposes
        public void CreatePlayer(Vector2 HUDposition, string name, ActionBindings actionBindings,
            Vector2 position = default(Vector2), bool movable = true,
            MoveComponent customMoveComponent = null, bool cameraFollow = false, bool collision = false,
            bool disabled = false, uint cageId = 0, Viewport view = default(Viewport))
        {
            if (disabled) return;

            var light = new Spotlight()
            {
                Position = position,
                Scale = new Vector2(850f),
                Radius = (float) 0.0001,
                Intensity = (float) 0.6,
                ShadowType = ShadowType.Solid // Will not lit hulls themselves
            };
            IEntityBuilder playerEntity = new EntityBuilder()
                .SetPosition(position, 10)
                .SetRendering(100, 100)
                .SetInertiaDampening()
                .SetBackwardsPenalty()
                .SetSprite("player_sprites4", new Point(1252, 206), 313, 206)
                .SetLight(light)
                .SetSound("walking")
                .SetMovement(200, 380, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetRectangleCollision()
                .SetCameraFollow((int) cageId)
                .SetPlayer(name)
                .SetHealth()
                .SetScore()
                .SetHUD(false, showStats: true)
                .SetAmmo()
                .Build();


            var animationBindings = new SpriteAnimationBindingsBuilder()
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(1252, 206), new Point(0, 1030))
                        .StateConditions(State.WalkingForward)
                        .Length(40)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingForward)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingBackwards)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Build();

            ComponentManager.Instance.AddComponentToEntity(actionBindings, playerEntity.GetEntityKey());
            ComponentManager.Instance.AddComponentToEntity(animationBindings, playerEntity.GetEntityKey());

            var weaponComponent = ComponentFactory.NewComponent<WeaponComponent>();
            weaponComponent.Damage = 1000;
            weaponComponent.ClipSize = 100;
            ComponentManager.Instance.AddComponentToEntity(weaponComponent, playerEntity.GetEntityKey());

            var cameraViewComponent = new CameraViewComponent()
            {
                CameraId = (int) cageId,
                View = view,
                MinScale = 0.5f,
            };

            ComponentManager.Instance.AddComponentToEntity(cameraViewComponent, playerEntity.GetEntityKey());

        }
        //_________________________________________________________________________________//
        protected override void LoadContent()
        {
            scoreFont = Content.Load<SpriteFont>("Score");
            gameOver = Content.Load<Texture2D>("gameOver");

            //BUNDLE
            gameBundle.LoadContent();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            gameBundle.Update(gameTime);
            fps.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Texture2D videoTexture = null;

            HealthSystem life = new HealthSystem();

            switch (currentGameState)
            {
                case GameState.Start:
                    //if (player.State != MediaState.Stopped)
                    //    videoTexture = player.GetTexture();

                    //if (videoTexture != null)
                    //{
                    //    spriteBatch.Begin();
                    //    spriteBatch.Draw(videoTexture,
                    //        new Rectangle(0, 0, (int)viewportDimensions.X, (int)viewportDimensions.Y), Color.White);
                    //    spriteBatch.End();
                    currentGameState = GameState.InGame;
                    //}
                    break;

                case GameState.InGame:
                    //if (player.State == MediaState.Stopped)
                    //{
                    //BUNDLE
                    gameBundle.Draw(gameTime);
                    fps.Draw(gameTime);

                    if (life.CheckIfAllPlayersAreDead())
                    {
                        currentGameState = GameState.GameOver;
                    }

                    //}
                    break;

                case GameState.GameOver:

                    GraphicsDevice.Clear(Color.Black);

                    spriteBatch.Begin();
                    spriteBatch.Draw(gameOver, new Rectangle(0, 0, 1800, 1500), Color.White);
                    spriteBatch.End();

                    var GameScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent));
                    if (GameScoreList.Count <= 0) return;
                    var GameScore = (GameScoreComponent) GameScoreList.First().Value;

                    string yourScore = "Total score: " + GameScore.TotalGameScore;
                    string exit = "(Press ESCAPE to exit)";

                    spriteBatch.Begin();
                    spriteBatch.DrawString(scoreFont, yourScore, new Vector2(50, 100), Color.Red);
                    spriteBatch.DrawString(scoreFont, exit, new Vector2(50, 200), Color.Red);
                    spriteBatch.End();

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                        Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();

                    break;
            }
            base.Draw(gameTime);
        }
    }
}