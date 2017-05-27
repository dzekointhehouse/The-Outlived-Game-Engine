using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Entities;
using Game.Services;
using Game.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Systems;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    class InGame : IMenu, ILifecycle
    {
        public MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }

        private static ComponentFactory ComponentFactory = ComponentManager.Instance.ComponentFactory;

        private GameManager gameManager;

        //private ControlsConfig controls;
        private bool isInitialized = false;

        private Boolean isIngame = true;
        private GamePlayers players;
        private GameMap maps = new GameMap();

        private GameEnemies enemies = new GameEnemies();

        //  private GamePickups pickups = new GamePickups();
        private HealthSystem life = new HealthSystem();

        private PickupFactory pickups = new PickupFactory();

        // Systems
        private SoundSystem soundSystem;

        private SpawnSystem spawnSystem;
        private WeaponSystem weaponSystem = new WeaponSystem();
        private Timer timer;
        private BackgroundMusic backgroundMusic;
        private GameViewports gameViewports;

        // SOME BUG NEED THIS.
        private Vector2 viewportDimensions = new Vector2(1800, 1300);

        private float timeSincelastCount;

        public InGame(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator; //
            VirtualGamePad = virtualGamePad; //
            this.gameManager = gameManager; 

            // Initializing systems
            soundSystem = new SoundSystem(); //
            spawnSystem = new SpawnSystem(); //
            // other stuff
            gameViewports = new GameViewports(gameManager.gameConfig, gameManager.Viewport); //
            players = new GamePlayers(gameManager.gameConfig, gameViewports); //
            timer = new Timer(0, OutlivedGame.Instance().Get<SpriteFont>("Fonts/ZlargeFont"), 
                gameViewports.defaultView); //
            backgroundMusic = new BackgroundMusic();///
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!isInitialized)
            {
                Initialize();
                gameManager.Engine.LoadContent();
                backgroundMusic.LoadSongs("bg_music1", "bg_music3", "bg_music3", "bg_music4");
                weaponSystem.Start();
                weaponSystem.LoadBulletSpriteEntity();
                isInitialized = true;
            }
            gameManager.Engine.Draw(gameTime);
            timer.Draw(spriteBatch);

            // Reset to default view
            OutlivedGame.Instance().GraphicsDevice.Viewport = gameViewports.defaultView;

            // Should move to HUD which should render defaultview
            spriteBatch.Begin();
            var nCameras = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent)).Count;
            switch (nCameras)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    spriteBatch.Draw(OutlivedGame.Instance().Content.Load<Texture2D>("border"),
                        gameViewports.defaultView.TitleSafeArea, Color.White);
                    break;
                default:
                    spriteBatch.Draw(OutlivedGame.Instance().Content.Load<Texture2D>("Images/4border"),
                        gameViewports.defaultView.TitleSafeArea, Color.White);
                    break;
            }
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (VirtualGamePad.Is(Pause, Pressed))
            {
                MenuNavigator.Pause();
            }
            timer.Update(gameTime);
            // Waiting for the countdown to finnish
            //if (!timer.IsCounting)
            //{
            backgroundMusic.PlayMusic();

            gameManager.Engine.Update(gameTime);

            if (gameManager.gameConfig.GameMode == GameModeMenu.GameModes.Survival)
            {
                spawnSystem.HandleWaves();
            }


            if (life.CheckIfAllPlayersAreDead())
            {
                gameManager.CurrentGameState = GameManager.GameState.GameOver;
            }
            // }
        }

        // To initialize game content when navigating
        // to this state.
        public void Initialize()
        {
            // Loading this projects content to be used by the game engine.
            SystemManager.Instance.GetSystem<LoadContentSystem>().LoadContent(OutlivedGame.Instance().Content);

            gameViewports.InitializeViewports();
            soundSystem.Start();
            // Game stuff
            maps.SetupMap(gameManager.gameConfig);
            players.CreatePlayers(maps); 
            pickups.CreatePickups();
            CreateGameEntities();
            CreateDefaultViewport();
        }

        private void CreateDefaultViewport()
        {
            var entity = EntityManager.GetEntityManager().NewEntity();

            var def = new DefaultViewport()
            {
                Viewport = gameViewports.defaultView
            };
            ComponentManager.Instance.AddComponentToEntity(def, entity);
        }

        private void CreateGameEntities()
        {
            var cameraCageId = SetupCameraCage();
            SetupHUD();

            if (gameManager.gameConfig.GameMode == GameModeMenu.GameModes.Survival)
            {
                CreateGlobalSpawnSpriteEntity();
                CreateGlobalSpawnEntity();
            }
            CreateGlobalBulletSpriteEntity();
            SetupGameScoreEntity();
            // SetupTempPlayerDeadSpriteFlyweight();
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
                .SetPosition(new Vector2(590, 900))
                .SetSprite("health3_small")
                .Build();

            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(590, 950))
                .SetSprite("medal")
                .Build();

            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(550, 1000))
                .SetSprite("ammo")
                .Build();

            //new EntityBuilder()
            //   .SetHUD(true)
            //   .SetPosition(new Vector2(0, 0))
            //   .SetSprite("bg_hud")
            //   .Build();
        }

        private static void CreateGlobalBulletSpriteEntity()
        {
            var entityId = EntityManager.GetEntityManager().NewEntity();
            var bulletSprite = ComponentFactory.NewComponent<SpriteComponent>();
            bulletSprite.SpriteName = "dot";
            var bulletSpriteComponent = ComponentFactory.NewComponent<BulletFlyweightComponent>();
            var soundComponent = ComponentFactory.NewComponent<SoundComponent>();
            //soundComponent.SoundEffectName = "winchester_fire";
            // ComponentManager.Instance.AddComponentToEntity(soundComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(bulletSprite, entityId);
            ComponentManager.Instance.AddComponentToEntity(bulletSpriteComponent, entityId);
        }

        //The camera cage keeps players from reaching the edge of the screen
        public uint SetupCameraCage()
        {
            var cameraCage = new EntityBuilder()
                .SetRendering((int) (viewportDimensions.X * 0.8), (int) (viewportDimensions.Y * 0.8), isFixed: true)
                .SetRectangleCollision(isCage: true)
                .SetPosition(Vector2.Zero, 2)
                .Build()
                .GetEntityKey();

            var offsetComponent = ComponentFactory.NewComponent<RenderOffsetComponent>();
            offsetComponent.Offset = new Vector2((float) (viewportDimensions.X * 0.25),
                (float) (viewportDimensions.Y * 0.25));
            ComponentManager.Instance.AddComponentToEntity(offsetComponent, cameraCage);
            return cameraCage;
        }


        private void CreateGlobalSpawnSpriteEntity()
        {
            var spawnSprite = EntityManager.GetEntityManager().NewEntity();
            var spawnSpriteSprite = ComponentFactory.NewComponent<SpriteComponent>();
            spawnSpriteSprite.SpriteName = "zombie1";
            var SpawnSpriteComponent = ComponentFactory.NewComponent<SpawnFlyweightComponent>();
            ComponentManager.Instance.AddComponentToEntity(spawnSpriteSprite, spawnSprite);
            ComponentManager.Instance.AddComponentToEntity(SpawnSpriteComponent, spawnSprite);
        }

        private void CreateGlobalSpawnEntity()
        {
            //var spawn = EntityManager.GetEntityManager().NewEntity();

            var global = new EntityBuilder()
                .SetHUD(true, "Zlarge", "")
                .SetPosition(new Vector2(100, 8))
                .Build();

            var spawncomponent = ComponentFactory.NewComponent<GlobalSpawnComponent>();
            ComponentManager.Instance.AddComponentToEntity(spawncomponent, global.GetEntityKey());
        }

        public void Reset()
        {
        }

        public void BeforeShow()
        {
            
        }

        public void BeforeHide()
        {
            foreach (var entity in EntityManager.GetEntityManager().GetListWithEntities())
            {
                ComponentManager.Instance.DeleteEntity(entity);
            }
            gameManager.gameConfig.Reset();
        }
    }
}