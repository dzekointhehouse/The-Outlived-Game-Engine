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

namespace Game.Menu.States
{
    class InGame : IMenu
    {

        private static ComponentFactory ComponentFactory = ComponentManager.Instance.ComponentFactory;
        private GameManager gameManager;
        private ControlsConfig controls;
        private bool isInitialized = false;
        private Boolean isIngame = true;
        private GamePlayers players;
        private GameMap maps = new GameMap();
        private GameEnemies enemies = new GameEnemies();
        private GamePickups pickups = new GamePickups();
        private HealthSystem life = new HealthSystem();
        private SoundSystem soundSystem;

        private GameViewports gameViewports;

        // SOME BUG NEED THIS.
        private Vector2 viewportDimensions = new Vector2(1800, 1300);

        private WeaponSystem weaponSystem = new WeaponSystem();
        private SpawnSystem SpawnSystem = new SpawnSystem();

        public InGame(GameManager gameManager)
        {
            this.gameManager = gameManager;

            // Initializing systems
            soundSystem = new SoundSystem();
            // other stuff
            controls = new ControlsConfig(gameManager);
            gameViewports = new GameViewports(gameManager.gameConfig, gameManager.Viewport);
            players = new GamePlayers(gameManager.gameConfig, gameViewports);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!isInitialized)
            {
                Initialize();
                gameManager.Engine.LoadContent();
                weaponSystem.Start();
                weaponSystem.LoadBulletSpriteEntity();
                isInitialized = true;
            }
            gameManager.Engine.Draw(gameTime);

            // Reset to default view
            OutlivedGame.Instance().GraphicsDevice.Viewport = gameViewports.defaultView;

            // Should move to HUD which should render defaultview
            var nCameras = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent)).Count;
            switch (nCameras)
            {
                case 2:
                  //  spriteBatch.Draw(OutlivedGame.Instance().Content.Load<Texture2D>("border"), Vector2.Zero, Color.White);
                    break;
                case 3:
                    break;
                default:
                    break;

            }
        }

        public void Update(GameTime gameTime)
        {            

            //if (MediaPlayer.State != MediaState.Stopped)
            //{
            //    MediaPlayer.Stop();
            //}
            controls.PauseButton();
           var bgMusic =  OutlivedGame.Instance().Content.Load<Song>("Sound/bg_music1");

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Volume = 0.7f;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(bgMusic);
            }

            if (gameManager.gameConfig.GameMode == GameModeMenu.GameModes.Survival)
            {
                SpawnSystem.HandleWaves();
            }

            gameManager.Engine.Update(gameTime);

            if (life.CheckIfNotAlive())
            {
                gameManager.CurrentGameState = GameManager.GameState.GameOver;
            }
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
//            enemies.CreateMonster("player_sprites");
            pickups.AddPickup("healthpickup", GamePickups.PickupType.Health, new Vector2(1400, 1200));
            pickups.AddPickup("healthpickup", GamePickups.PickupType.Health, new Vector2(70, 300));
            pickups.AddPickup("ammopickup", GamePickups.PickupType.Ammo, new Vector2(100, 200));            
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
           // SetupBackgroundTiles();
            //SetupCamera();
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
            ComponentManager.Instance.AddComponentToEntity(gameScoreComponent, EntityManager.GetEntityManager().NewEntity());
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
            offsetComponent.Offset = new Vector2((float) (viewportDimensions.X * 0.25), (float) (viewportDimensions.Y * 0.25));
            ComponentManager.Instance.AddComponentToEntity(offsetComponent, cameraCage);
            return cameraCage;
        }



        private static void CreateGlobalSpawnSpriteEntity()
        {
            var spawnSprite = EntityManager.GetEntityManager().NewEntity();
            var spawnSpriteSprite = ComponentFactory.NewComponent<SpriteComponent>();
            spawnSpriteSprite.SpriteName = "zombie1";
            var SpawnSpriteComponent = ComponentFactory.NewComponent<SpawnFlyweightComponent>();
            ComponentManager.Instance.AddComponentToEntity(spawnSpriteSprite, spawnSprite);
            ComponentManager.Instance.AddComponentToEntity(SpawnSpriteComponent, spawnSprite);
        }

        private static void CreateGlobalSpawnEntity()
        {
            var spawn = EntityManager.GetEntityManager().NewEntity();
            var spawncomponent = ComponentFactory.NewComponent<GlobalSpawnComponent>();
            //var spawnSpawnComponent = new GlobalSpawnComponent();
            ComponentManager.Instance.AddComponentToEntity(spawncomponent, spawn);
            //ComponentManager.Instance.AddComponentToEntity(spawnSpawnComponent, spawn);
        }

    }
}
