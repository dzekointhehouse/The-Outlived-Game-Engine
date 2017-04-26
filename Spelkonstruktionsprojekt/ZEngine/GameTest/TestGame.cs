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
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Systems;
using ZEngine.Systems.Collisions;
using ZEngine.Wrappers;
using static Spelkonstruktionsprojekt.ZEngine.Components.ActionBindings;

namespace Spelkonstruktionsprojekt.ZEngine.GameTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TestGame : Game
    {
        private readonly GameDependencies _gameDependencies = new GameDependencies();
        private KeyboardState _oldKeyboardState = Keyboard.GetState();

        private Video video;
        private VideoPlayer player;

        private Vector2 viewportDimensions = new Vector2(1800, 1300);
        private PenumbraComponent penumbraComponent;

        // testing
        private FPS fps;

        public SpriteBatch spriteBatch;

        private Song musicTest;

        public TestGame()
        {
            _gameDependencies.GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int) viewportDimensions.X,
                PreferredBackBufferHeight = (int) viewportDimensions.Y
            };
            Content.RootDirectory = "Content";

            // Create an instance of the FPS GameComponent
            fps = new FPS(this);

            // Turn off the fixed time step
            // and the synchronization with the vertical retrace
            // so the game's FPS can be measured
            IsFixedTimeStep = false;
            _gameDependencies.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Initialize()
        {
            //Init systems that require initialization
            _<TankMovementSystem>().Start();
            _<AbilitySystem>().Start();
            _<WallCollisionSystem>().Start();
            _<SoundSystem>().Start();
            _<WeaponSystem>().Start();
            _<EnemyCollisionSystem>().Start();
            _<BulletCollisionSystem>().Start();

            _gameDependencies.GameContent = this.Content;
            _gameDependencies.SpriteBatch = new SpriteBatch(GraphicsDevice);
            // just quickly done for FPS testing
            spriteBatch = _gameDependencies.SpriteBatch;
            _gameDependencies.Game = this;

            CreateTestEntities();

            base.Initialize();
        }

        private void CreateTestEntities()
        {
            // var button = new Button();
            var cameraCageId = SetupCameraCage();
            InitPlayers(cameraCageId);
            //SetupBackground();
            SetupBackgroundTiles(5, 5);
            SetupCamera();
//            SetupEnemy();
            CreateGlobalBulletSpriteEntity();
            SetupTempPlayerDeadSpriteFlyweight();
        }

        private void SetupTempPlayerDeadSpriteFlyweight()
        {
            var tempEntity = EntityManager.GetEntityManager().NewEntity();
            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "dot"
            };
            var tempDeadSpriteComponent = new TempPlayerDeadSpriteComponent();
            ComponentManager.Instance.AddComponentToEntity(tempDeadSpriteComponent, tempEntity);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, tempEntity);
        }

        private static void CreateGlobalBulletSpriteEntity()
        {
            var bulletSprite = EntityManager.GetEntityManager().NewEntity();
            var bulletSpriteSprite = new SpriteComponent()
            {
                SpriteName = "dot2"
            };
            var bulletSpriteComponent = new BulletFlyweightComponent();
            var soundComponent = new SoundComponent()
            {
                SoundEffectName = "bullet9mm"
            };
            ComponentManager.Instance.AddComponentToEntity(soundComponent, bulletSprite);
            ComponentManager.Instance.AddComponentToEntity(bulletSpriteSprite, bulletSprite);
            ComponentManager.Instance.AddComponentToEntity(bulletSpriteComponent, bulletSprite);
        }

        //The camera cage keeps players from reaching the edge of the screen
        public int SetupCameraCage()
        {
            var cameraCage = EntityManager.GetEntityManager().NewEntity();
            //            var renderComponentCage = new RenderComponentBuilder()
            ////                .Position((int)((int)viewportDimensions.X * 0.5), (int)(viewportDimensions.Y * 0.5), 2)
            //                .Position(0, 0, 2)
            //                .Dimensions((int)(viewportDimensions.X * 0.8), (int)(viewportDimensions.Y * 0.8))
            //                .Fixed(true).Build();

            var renderComponentCage = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent()
                {
                    Width = (int) (viewportDimensions.X * 0.8),
                    Height = (int) (viewportDimensions.Y * 0.8)
                },
                Fixed = true
            };

            var position = new PositionComponent()
            {
                Position = new Vector2(0,0),
                ZIndex = 2
            };

            var cageSprite = new SpriteComponent()
            {
                SpriteName = "dot"
            };
            var collisionComponentCage = new CollisionComponent()
            {
                IsCage = true,
            };
            var offsetComponent = new RenderOffsetComponent()
            {
                Offset = new Vector2((float) (viewportDimensions.X * 0.25), (float) (viewportDimensions.Y * 0.25))
            };
            ComponentManager.Instance.AddComponentToEntity(renderComponentCage, cameraCage);
//            ComponentManager.Instance.AddComponentToEntity(cageSprite, cameraCage);
            ComponentManager.Instance.AddComponentToEntity(position, cameraCage);
            ComponentManager.Instance.AddComponentToEntity(collisionComponentCage, cameraCage);
            ComponentManager.Instance.AddComponentToEntity(offsetComponent, cameraCage);
            return cameraCage;
        }

        public void SetupBackground()
        {
            var entityId3 = EntityManager.GetEntityManager().NewEntity();
            //var renderComponent3 = new RenderComponentBuilder()
            //    .Position(0, 0, 1)
            //    .Dimensions(1000, 1000).Build();
            var position = new PositionComponent() {Position = new Vector2(0, 0), ZIndex = 1};
            ComponentManager.Instance.AddComponentToEntity(position, entityId3);


            var renderComponent3 = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent() {Height = 1000, Width = 1000}
            };
            ComponentManager.Instance.AddComponentToEntity(renderComponent3, entityId3);
            var spriteComponent3 = new SpriteComponent()
            {
                SpriteName = "Grass"
            };
            ComponentManager.Instance.AddComponentToEntity(spriteComponent3, entityId3);
        }

        public void SetupBackgroundTiles(int width, int height)
        {
           var tileTypes = new Dictionary<int, string>();

            tileTypes.Add(2, "legotilesRed");
            tileTypes.Add(28, "grass");

            MapHelper mapcreator = new MapHelper(tileTypes);

            //int[,] matrix = new int[,]
            //{
            //    {2, 1, 1, 1, 1},
            //    {1, 1, 1, 1, 1},
            //    {1, 1, 2, 1, 1},
            //    {1, 1, 1, 1, 1},
            //};
            mapcreator.CreateMapTiles(MapPack.TheWallMap, 64);
        }

        public void SetupCamera()
        {
            var cameraEntity = EntityManager.GetEntityManager().NewEntity();
            var cameraViewComponent = new CameraViewComponent()
            {
                View = new Rectangle(0, 0, (int) viewportDimensions.X, (int) viewportDimensions.Y)
            };
            ComponentManager.Instance.AddComponentToEntity(cameraViewComponent, cameraEntity);
            //var cameraRenderable = new RenderComponentBuilder()
            //    .Position(0, 0, 500)
            //    .Dimensions(10, 10).Build();

            var cameraRenderable = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent()
                {
                    Height = 10,
                    Width = 10
                }
            };

            var position = new PositionComponent() {Position = new Vector2(0, 0), ZIndex = 500};


            var cameraSprite = new SpriteComponent()
            {
                SpriteName = "dot"
            };
            var light = new LightComponent()
            {
                Light = new PointLight()
                {
                    Position = new Vector2(150, 150),
                    Scale = new Vector2(500f),
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves
                }
            };
            //ComponentManager.Instance.AddComponentToEntity(light, cameraEntity);
            ComponentManager.Instance.AddComponentToEntity(cameraRenderable, cameraEntity);
            ComponentManager.Instance.AddComponentToEntity(position, cameraEntity);
            //ComponentManager.Instance.AddComponentToEntity(cameraSprite, cameraEntity);
        }

        public void SetupEnemy()
        {
            var x = new Random(DateTime.Now.Millisecond).Next(0, 5000);
            var y = new Random(DateTime.Now.Millisecond).Next(0, 5000);

            var entityId = EntityManager.GetEntityManager().NewEntity();
            //var renderComponent = new RenderComponentBuilder()
            //    .Position(x, y, 20)
            //    .Dimensions(300, 300)
            //    .Build();
            var renderComponent = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent()
                {
                    Height = 300,
                    Width = 300
                }
            };

            var position = new PositionComponent() {Position = new Vector2(x, y), ZIndex = 20};


            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "zombieSquare"
            };
            var light = new LightComponent()
            {
                Light = new Spotlight()
                {
                    Position = new Vector2(150, 150),
                    Scale = new Vector2(500f),
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves
                }
            };

            var sound = new SoundComponent()
            {
                SoundEffectName = "zombiewalking",
                Volume = 1f
            };
            ComponentManager.Instance.AddComponentToEntity(sound, entityId);

            var moveComponent = new MoveComponent()
            {
                MaxVelocitySpeed = 205,
                AccelerationSpeed = 5,
                RotationSpeed = 4,
                Direction = new Random(DateTime.Now.Millisecond).Next(0, 40) / 10
            };
            var aiComponent = new AIComponent();
            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(position, entityId);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId);
            //ComponentManager.Instance.AddComponentToEntity(light, entityId);
            ComponentManager.Instance.AddComponentToEntity(moveComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(aiComponent, entityId);
            var collisionComponent = new CollisionComponent()
            {
                //spriteBoundingRectangle = new Rectangle(50, 50, 200, 200)
            };
            ComponentManager.Instance.AddComponentToEntity(collisionComponent, entityId);
            //if (collision)
            //{
            //
            //}
        }

        public void InitPlayers(int cageId)
        {
            var player1 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings1 = new ActionBindingsBuilder()
                .SetAction(Keys.W, EventConstants.WalkForward) //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FireWeapon)
                .SetAction(Keys.LeftShift, EventConstants.Running)
                .Build();

            var player2 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings2 = new ActionBindingsBuilder()
                .SetAction(Keys.I, EventConstants.WalkForward)
                .SetAction(Keys.K, EventConstants.WalkBackward)
                .SetAction(Keys.J, EventConstants.TurnLeft)
                .SetAction(Keys.L, EventConstants.TurnRight)
                .SetAction(Keys.O, EventConstants.FireWeapon)
                .SetAction(Keys.U, EventConstants.TurnAround)
                .SetAction(Keys.H, EventConstants.Running)
                .Build();

            var player3 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings3 = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FireWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();

            CreatePlayer("Carlos", player1, actionBindings1, position: new Vector2(200, 200), cameraFollow: true,
                collision: true, isCaged: true, cageId: cageId);
            CreatePlayer("Elvir", player2, actionBindings2, position: new Vector2(400, 400), cameraFollow: true,
                collision: true, isCaged: true, cageId: cageId);
            CreatePlayer("Markus", player3, actionBindings3, position: new Vector2(300, 300), cameraFollow: true,
                collision: true, isCaged: true, cageId: cageId);
        }

        //The multitude of options here is for easy debug purposes
        public void CreatePlayer(string name, int entityId, ActionBindings actionBindings,
            Vector2 position = default(Vector2), bool movable = true, bool useDefaultMoveComponent = true,
            MoveComponent customMoveComponent = null, bool cameraFollow = false, bool collision = false,
            bool disabled = false, bool isCaged = false, int cageId = 0)
        {
            if (disabled) return;
            if (position == default(Vector2)) position = new Vector2(150, 150);
            //Initializing first, movable, entity
            //var renderComponent = new RenderComponentBuilder()
            //    //.Position(150 + new Random(DateTime.Now.Millisecond).Next(0, 500), 150, 10)
            //    .Position(position.X, position.Y, 10)
            //    //.Radius(60)
            //    .Dimensions(100, 100)
            //    .Build();


            var renderComponent = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent()
                {
                    Height = 100,
                    Width = 100
                },
            };

            var positionComponent = new PositionComponent() {Position = position, ZIndex = 10};

            var dampeningComponent = new InertiaDampeningComponent();
            var backwardsPenaltyComponent = new BackwardsPenaltyComponent();
            ComponentManager.Instance.AddComponentToEntity(dampeningComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(backwardsPenaltyComponent, entityId);

            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "topDownSoldier",
                Scale = 1
            };
            var light = new LightComponent()
            {
                Light = new Spotlight()
                {
                    Position = new Vector2(150, 150),
                    Scale = new Vector2(850f),
                    Radius = (float) 0.0001,
                    Intensity = (float) 0.6,
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves
                }
            };

            var sound = new SoundComponent()
            {
                SoundEffectName = "walking"
            };

            var animation = new SpriteAnimationComponent()
            {
                Spritesheet = Content.Load<Texture2D>("blood"),
                SpritesheetSize = new Point(3, 3),
                MillisecondsPerFrame = 30
            };
            ComponentManager.Instance.AddComponentToEntity(animation, entityId);

            ComponentManager.Instance.AddComponentToEntity(sound, entityId);
            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(positionComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(actionBindings, entityId);
            ComponentManager.Instance.AddComponentToEntity(light, entityId);

            if (movable && useDefaultMoveComponent)
            {
                var moveComponent = new MoveComponent()
                {
                    MaxVelocitySpeed = 200,
                    AccelerationSpeed = 380,
                    RotationSpeed = 4,
                    Direction = new Random(DateTime.Now.Millisecond).Next(0, 40) / 10
                };
                ComponentManager.Instance.AddComponentToEntity(moveComponent, entityId);
            }
            else if (movable && customMoveComponent != null)
            {
                ComponentManager.Instance.AddComponentToEntity(customMoveComponent, entityId);
            }

            if (collision)
            {
                var collisionComponent = new CollisionComponent()
                {
                    spriteBoundingRectangle = new Rectangle(30, 20, 70, 60)
                };
                ComponentManager.Instance.AddComponentToEntity(collisionComponent, entityId);
            }
            if (cameraFollow)
            {
                var cameraFollowComponent = new CameraFollowComponent();
                ComponentManager.Instance.AddComponentToEntity(cameraFollowComponent, entityId);
            }

            if (isCaged)
            {
                var cageComponent = new CageComponent()
                {
                    CageId = cageId
                };
                ComponentManager.Instance.AddComponentToEntity(cageComponent, entityId);
            }

            var playerComponent = new PlayerComponent()
            {
                Name = name
            };
            ComponentManager.Instance.AddComponentToEntity(playerComponent, entityId);
            var healthComponent = new HealthComponent()
            {
                CurrentHealth = new Random().Next(0, 100)
            };
            ComponentManager.Instance.AddComponentToEntity(healthComponent, entityId);
            var weaponComponent = new WeaponComponent()
            {
                Damage = 10
            };
            ComponentManager.Instance.AddComponentToEntity(weaponComponent, entityId);
        }

        protected override void LoadContent()
        {
            video = Content.Load<Video>("ZEngine-intro");

            player = new VideoPlayer();
            // musicTest = Content.Load<Song>("assassins");
            _<LoadContentSystem>().LoadContent(this.Content);
            penumbraComponent = _<FlashlightSystem>().Initialize(_gameDependencies);
            //MediaPlayer.Play(musicTest);

//            if (player.State == MediaState.Stopped)
//            {
//                player.Play(video);
//            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (player.State == MediaState.Stopped)
            {
                _<EnemyCollisionSystem>().GameTime = gameTime; //TODO system dependency
                _<InputHandler>().HandleInput(_oldKeyboardState, gameTime);
                _oldKeyboardState = Keyboard.GetState();

                _<AISystem>().Update(gameTime);
                _<AnimationSystem>().RunAnimations(gameTime);
                _<SpriteAnimationSystem>().Update(gameTime);
                _<CollisionSystem>().DetectCollisions();
                _<CollisionResolveSystem>().ResolveCollisions(ZEngineCollisionEventPresets.StandardCollisionEvents,
                    gameTime);

                _<CameraSceneSystem>().Update(gameTime);
                _<FlashlightSystem>().Update(gameTime, viewportDimensions);
                _<HealthSystem>().Update();
                _<EntityRemovalSystem>().Update(gameTime);
                _<InertiaDampenerSystem>().Apply(gameTime);
                _<BackwardsPenaltySystem>().Apply();
                _<MoveSystem>().Move(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Texture2D videoTexture = null;

            if (player.State != MediaState.Stopped)
                videoTexture = player.GetTexture();

            if (videoTexture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(videoTexture,
                    new Rectangle(0, 0, (int) viewportDimensions.X, (int) viewportDimensions.Y), Color.White);
                spriteBatch.End();
            }
            if (player.State == MediaState.Stopped)
            {
                _<FlashlightSystem>().BeginDraw(penumbraComponent);
                _<RenderSystem>().Render(_gameDependencies); // lowers FPS by half (2000)
                _<FlashlightSystem>().EndDraw(penumbraComponent, gameTime);
                _<TitlesafeRenderSystem>().Draw(_gameDependencies); // not noticable
            }
            base.Draw(gameTime);
        }

        private T _<T>() where T : class, ISystem
        {
            return SystemManager.Instance.GetSystem(typeof(T)) as T;
        }
    }
}