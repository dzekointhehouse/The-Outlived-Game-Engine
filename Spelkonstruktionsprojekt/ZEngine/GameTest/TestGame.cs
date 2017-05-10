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

namespace Spelkonstruktionsprojekt.ZEngine.GameTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TestGame : Game
    {
        // private readonly GameDependencies Dependencies = new GameDependencies();
        private KeyboardState _oldKeyboardState = Keyboard.GetState();
        private Video video;
        private VideoPlayer player;
        private Vector2 viewportDimensions = new Vector2(1800, 1300);
        private PenumbraComponent penumbraComponent;
        // testing
        private FPS fps;
        public SpriteBatch spriteBatch;
        private Song musicTest;
        private SystemManager manager = SystemManager.Instance;

        private readonly FullZengineBundle gameBundle;

        public TestGame()
        {
            gameBundle = new FullZengineBundle();

            gameBundle.Dependencies.GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)viewportDimensions.X,
                PreferredBackBufferHeight = (int)viewportDimensions.Y
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

            CreateTestEntities();
            base.Initialize();
        }

        private void CreateTestEntities()
        {
            // var button = new Button();
            var cameraCageId = SetupCameraCage();
            InitPlayers(cameraCageId);
            SetupBackgroundTiles(5, 5);
            SetupCamera();
            SetupEnemy();
            SetupHUD();
            CreateGlobalBulletSpriteEntity();
            SetupTempPlayerDeadSpriteFlyweight();
        }

        private void SetupHUD()
        {
            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(10, 1100))
                .SetSprite("XboxController")
                .Build();

            // Health Icons
            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(1680, 1150))
                .SetSprite("health3_small")
                .Build();
            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(1680, 1210))
                .SetSprite("health3_small")
                .Build();
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
                SpriteName = "dot"
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

            var renderComponentCage = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent()
                {
                    Width = (int)(viewportDimensions.X * 0.8),
                    Height = (int)(viewportDimensions.Y * 0.8)
                },
                Fixed = true
            };

            var position = new PositionComponent()
            {
                Position = new Vector2(0, 0),
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
                Offset = new Vector2((float)(viewportDimensions.X * 0.25), (float)(viewportDimensions.Y * 0.25))
            };
            ComponentManager.Instance.AddComponentToEntity(renderComponentCage, cameraCage);
            //            ComponentManager.Instance.AddComponentToEntity(cageSprite, cameraCage);
            ComponentManager.Instance.AddComponentToEntity(position, cameraCage);
            ComponentManager.Instance.AddComponentToEntity(collisionComponentCage, cameraCage);
            ComponentManager.Instance.AddComponentToEntity(offsetComponent, cameraCage);
            return cameraCage;
        }


        public void SetupBackgroundTiles(int width, int height)
        {
            var tileTypes = new Dictionary<int, string>();

            //tileTypes.Add(0, "blue64");
            tileTypes.Add(1, "grass");
            //tileTypes.Add(2, "red64");
            //tileTypes.Add(4, "yellowwall64");


            MapHelper mapcreator = new MapHelper(tileTypes);

            mapcreator.CreateMapTiles(MapPack.Minimap, 2000);
        }

        public void SetupCamera()
        {
            var cameraEntity = EntityManager.GetEntityManager().NewEntity();
            var cameraViewComponent = new CameraViewComponent()
            {
                View = new Rectangle(0, 0, (int)viewportDimensions.X, (int)viewportDimensions.Y)
            };

            var position = new PositionComponent() { Position = new Vector2(0, 0), ZIndex = 500 };

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
                .SetCollision(new Rectangle(50, 50, 200, 200))
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
                .SetAction(Keys.LeftShift, EventConstants.LightStatus)
                .SetAction(Keys.R, EventConstants.Running)
                .Build();

            var player2 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings2 = new ActionBindingsBuilder()
                .SetAction(Keys.I, EventConstants.WalkForward)
                .SetAction(Keys.K, EventConstants.WalkBackward)
                .SetAction(Keys.J, EventConstants.TurnLeft)
                .SetAction(Keys.L, EventConstants.TurnRight)
                .SetAction(Keys.O, EventConstants.FireWeapon)
                .SetAction(Keys.U, EventConstants.TurnAround)
                .SetAction(Keys.H, EventConstants.LightStatus)
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

            CreatePlayer(new Vector2(1650, 1100), name: "Carlos", actionBindings: actionBindings1,
                position: new Vector2(200, 200), cameraFollow: true,
                collision: true, isCaged: true, cageId: cageId);
            CreatePlayer(new Vector2(1650, 1100), "Elvir", actionBindings2, position: new Vector2(400, 400), cameraFollow: true,
                collision: true, isCaged: true, cageId: cageId, disabled: false);
            //CreatePlayer("Markus", player3, actionBindings3, position: new Vector2(300, 300), cameraFollow: true,
            //    collision: true, isCaged: false, cageId: cageId, disabled: true);
        }

        //The multitude of options here is for easy debug purposes
        public void CreatePlayer(Vector2 HUDposition, string name, ActionBindings actionBindings,
            Vector2 position = default(Vector2), bool movable = true,
            MoveComponent customMoveComponent = null, bool cameraFollow = false, bool collision = false,
            bool disabled = false, bool isCaged = false, int cageId = 0)
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
                .SetSprite("player_sprites", new Point(1252, 206), 313, 206)
                .SetLight(light)
                .SetSound("walking")
                .SetMovement(200, 380, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetCollision(new Rectangle(30, 20, 70, 60))
                .SetCameraFollow()
                .SetPlayer(name)
                .SetHealth()
                .SetHUD(false, showStats:true)
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


            if (isCaged)
            {
                var cageComponent = new CageComponent()
                {
                    CageId = cageId
                };
                ComponentManager.Instance.AddComponentToEntity(cageComponent, playerEntity.GetEntityKey());
            }

            var weaponComponent = new WeaponComponent()
            {
                Damage = 10
            };
            ComponentManager.Instance.AddComponentToEntity(weaponComponent, playerEntity.GetEntityKey());
        }

        protected override void LoadContent()
        {
            video = Content.Load<Video>("ZEngine-intro");

            player = new VideoPlayer();
            //MediaPlayer.Play(musicTest);

            //            if (player.State == MediaState.Stopped)
            //            {
            //                player.Play(video);
            //            }

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
            Texture2D videoTexture = null;

            if (player.State != MediaState.Stopped)
                videoTexture = player.GetTexture();

            if (videoTexture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(videoTexture,
                    new Rectangle(0, 0, (int)viewportDimensions.X, (int)viewportDimensions.Y), Color.White);
                spriteBatch.End();
            }
            if (player.State == MediaState.Stopped)
            {
                //BUNDLE
                gameBundle.Draw(gameTime);
                fps.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}