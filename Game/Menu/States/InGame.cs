using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Entities;
using Game.Services;
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
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;
using Spelkonstruktionsprojekt.ZEngine.Systems;

namespace Game.Menu.States
{
    class InGame : IMenu
    {

        private GameManager gameManager;
        private ControlsConfig controls;
        private bool isInitialized = false;
        private Boolean isIngame = true;
        private GamePlayers players = new GamePlayers();
        private GameEnemies enemies = new GameEnemies();
        private GamePickups pickups = new GamePickups();
        private HealthSystem life = new HealthSystem();

        // SOME BUG NEED THIS.
        private Vector2 viewportDimensions = new Vector2(1800, 1300);

        public InGame(GameManager gameManager)
        {
            this.gameManager = gameManager;
            controls = new ControlsConfig(gameManager);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!isInitialized)
            {

                InitializeGameContent();
                gameManager.Engine.LoadContent();
                isInitialized = true;
            }
            gameManager.Engine.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {            

            if (MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
            }
            controls.PauseButton();
            gameManager.Engine.Update(gameTime);

            if (life.CheckIfNotAlive())
            {
                gameManager.CurrentGameState = GameManager.GameState.GameOver;
            }
        }

        // To initialize game content when navigating
        // to this state.
        public void InitializeGameContent()
        {
            players.CreatePlayers(gameManager.gameConfig);
            enemies.CreateMonster("player_sprites");
            pickups.AddPickup("healthpickup", GamePickups.PickupType.Health, new Vector2(40, 40));
            pickups.AddPickup("healthpickup", GamePickups.PickupType.Health, new Vector2(70, 300));
            pickups.AddPickup("ammopickup", GamePickups.PickupType.Ammo, new Vector2(100, 200));            
            CreateGameEntities();
        }

        private void CreateGameEntities()
        {
            var cameraCageId = SetupCameraCage();
            SetupBackgroundTiles();
            SetupCamera();
            SetupHUD();
            CreateGlobalBulletSpriteEntity();
            SetupGameScoreEntity();
            // SetupTempPlayerDeadSpriteFlyweight();
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
                Fixed = true
            };

            var dimensionsComponent = new DimensionsComponent()
            {
                Width = (int)(viewportDimensions.X * 0.8),
                Height = (int)(viewportDimensions.Y * 0.8)
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


        public void SetupBackgroundTiles()
        {
            var tileTypes = new Dictionary<int, string>();

            //tileTypes.Add(0, "blue64");
            tileTypes.Add(2, "yellowwall64");
            tileTypes.Add(28, "grass");
            //tileTypes.Add(4, "yellowwall64");


            MapHelper mapcreator = new MapHelper(tileTypes);

            mapcreator.AddNumberToCollisionList(2);

            mapcreator.CreateMapTiles(MapPack.TheWallMap, 100);
        }

        public void SetupCamera()
        {
            var cameraEntity = EntityManager.GetEntityManager().NewEntity();
            var cameraViewComponent = new CameraViewComponent()
            {
                View = new Rectangle(0, 0, (int)viewportDimensions.X, (int)viewportDimensions.Y),
                MinScale = 0.5f
            };

            var position = new PositionComponent() { Position = new Vector2(0, 0), ZIndex = 500 };

            ComponentManager.Instance.AddComponentToEntity(cameraViewComponent, cameraEntity);
            ComponentManager.Instance.AddComponentToEntity(position, cameraEntity);
        }
    }
}
