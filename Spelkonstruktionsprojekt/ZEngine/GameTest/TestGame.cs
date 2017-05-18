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
        private SpriteFont scoreFont;
        private Texture2D gameOver;

        private readonly FullZengineBundle gameBundle;

        // Game states
        private enum GameState { Start, InGame, GameOver };
        private GameState currentGameState = GameState.Start;

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
          //  SetupEnemy();
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
            var entity = EntityManager.GetEntityManager().NewEntity();
            var coll = new CollisionComponent();
            var dim = new DimensionsComponent()
            {
                Height = 40,
                Width = 40
            };
            var render = new RenderComponent()
            {
                IsVisible = true,
            };
            var pick = new HealthPickupComponent();
            var pos = new PositionComponent()
            {
                Position = new Vector2(40, 40),
                ZIndex = 100
            };
            var sprite = new SpriteComponent()
            {
                SpriteName = "healthpickup",
            };
            var sound = new SoundComponent()
            {
                SoundEffectName = "pickup"
            };
            var ligh = new LightComponent()
            {
                Light = new PointLight() { },
            };
            ComponentManager.Instance.AddComponentToEntity(sound, entity);
            ComponentManager.Instance.AddComponentToEntity(ligh, entity);
            ComponentManager.Instance.AddComponentToEntity(coll, entity);
            ComponentManager.Instance.AddComponentToEntity(pick, entity);
            ComponentManager.Instance.AddComponentToEntity(pos, entity);
            ComponentManager.Instance.AddComponentToEntity(dim, entity);
            ComponentManager.Instance.AddComponentToEntity(render, entity);
            ComponentManager.Instance.AddComponentToEntity(sprite, entity);

        }
        private void SetupGameScoreEntity()
        {
            var gameScoreComponent = new GameScoreComponent();

            ComponentManager.Instance.AddComponentToEntity(gameScoreComponent, EntityManager.GetEntityManager().NewEntity());
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
            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "dot"
            };
            var tempDeadSpriteComponent = new TempPlayerDeadSpriteComponent();
            ComponentManager.Instance.AddComponentToEntity(tempDeadSpriteComponent, tempEntity);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, tempEntity);
        }
        private static void CreateFlyweightHealthpickupEntity()
        {
            var HealthpickupSprite = EntityManager.GetEntityManager().NewEntity();
            var HealthpickupSpriteSprite = new SpriteComponent()
            {
                SpriteName = "healthpickup"
            };
            var healthSpriteComponent = new FlyweightPickupComponent();
            var soundComponent = new SoundComponent()
            {
                SoundEffectName = "pickup"
            };
             ComponentManager.Instance.AddComponentToEntity(soundComponent, HealthpickupSprite);
            ComponentManager.Instance.AddComponentToEntity(HealthpickupSpriteSprite, HealthpickupSprite);
            ComponentManager.Instance.AddComponentToEntity(healthSpriteComponent, HealthpickupSprite);
        }
        private static void CreateFlyweightAmmopickupEntity()
        {
            var ammopickupSprite = EntityManager.GetEntityManager().NewEntity();
            var ammopickupSpriteSprite = new SpriteComponent()
            {
                SpriteName = "knife"
            };
            var ammoPickUpSprite = new FlyweightPickupComponent();
            var soundComponent = new SoundComponent()
            {
                SoundEffectName = "pickup"
            };
            ComponentManager.Instance.AddComponentToEntity(soundComponent, ammopickupSprite);
            ComponentManager.Instance.AddComponentToEntity(ammopickupSpriteSprite, ammopickupSprite);
            ComponentManager.Instance.AddComponentToEntity(ammoPickUpSprite, ammopickupSprite);
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
        private static void CreateGlobalSpawnSpriteEntity()
        {
            var spawnSprite = EntityManager.GetEntityManager().NewEntity();
            var spawnSpriteSprite = new SpriteComponent()
            {
                SpriteName = "Player_Sprites"
            };
            var SpawnSpriteComponent = new SpawnFlyweightComponent();
           
            ComponentManager.Instance.AddComponentToEntity(spawnSpriteSprite, spawnSprite);
            ComponentManager.Instance.AddComponentToEntity(SpawnSpriteComponent, spawnSprite);
        }
        private static void CreateGlobalSpawnEntity()
        {
            var spawn = EntityManager.GetEntityManager().NewEntity();
            var spawncomponent = new GlobalSpawnComponent()
            {    
            };
           // var spawnSpawnComponent = new GlobalSpawnComponent();
            ComponentManager.Instance.AddComponentToEntity(spawncomponent, spawn);
            //ComponentManager.Instance.AddComponentToEntity(spawnSpawnComponent, spawn);
        }

        //The camera cage keeps players from reaching the edge of the screen
        public int SetupCameraCage()
        {
            var cameraCage = EntityManager.GetEntityManager().NewEntity();

            var renderComponentCage = new RenderComponent()
            {
                Fixed = true
            };

            var dimensionsComponent = new DimensionsComponent()
            {
                Width = (int) (viewportDimensions.X * 0.8),
                Height = (int) (viewportDimensions.Y * 0.8)
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
            ComponentManager.Instance.AddComponentToEntity(dimensionsComponent, cameraCage);
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
            tileTypes.Add(1, "blue64");
            //tileTypes.Add(2, "red64");
            //tileTypes.Add(4, "yellowwall64");


            MapHelper mapcreator = new MapHelper(tileTypes);

            mapcreator.CreateMapTiles(MapPack.Minimap, 500);
        }

        public void SetupCamera()
        {
            var cameraEntity = EntityManager.GetEntityManager().NewEntity();
            var cameraViewComponent = new CameraViewComponent()
            {
                View = new Rectangle(0, 0, (int)viewportDimensions.X, (int)viewportDimensions.Y),
                MinScale = 0.5f,
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

            CreatePlayer(
                new Vector2(1650, 1100), 
                name: "Carlos", 
                actionBindings: actionBindings1,
                position: new Vector2(200, 200),
                cameraFollow: true,
                collision: true, 
                isCaged: true, 
                cageId: cageId);

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
                .SetSprite("player_sprites3", new Point(1252, 206), 313, 206)
                .SetLight(light)
                .SetSound("walking")
                .SetMovement(200, 380, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetRectangleCollision()
                .SetCameraFollow()
                .SetPlayer(name)
                .SetHealth()
                .SetAmmo()
                .SetScore()
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
                Damage = 1000
            };
            ComponentManager.Instance.AddComponentToEntity(weaponComponent, playerEntity.GetEntityKey());

            /*
            if(name == "Elvir")
            {
                ComponentManager.Instance.AddComponentToEntity(new HealthPickupComponent(), playerEntity.GetEntityKey());
            }*/
        }

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

                    if (life.CheckIfNotAlive())
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
                    var GameScore = (GameScoreComponent)GameScoreList.First().Value;

                    string yourScore = "Total score: " + GameScore.TotalGameScore;
                    string exit = "(Press ESCAPE to exit)";

                    spriteBatch.Begin();
                    spriteBatch.DrawString(scoreFont, yourScore, new Vector2(50, 100), Color.Red);
                    spriteBatch.DrawString(scoreFont, exit, new Vector2(50, 200), Color.Red);
                    spriteBatch.End();

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();

                    break;
            }
            base.Draw(gameTime);
        }
    }
}