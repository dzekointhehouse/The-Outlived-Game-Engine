using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using Game.Entities;
using Game.Services;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Game.Menu.States.GameModes
{
    public class SurvivalInitializer : IInitialize
    {
        private static ComponentFactory ComponentFactory = ComponentManager.Instance.ComponentFactory;
        
        public GameConfig GameConfig { get; }
        public GamePlayers GamePlayers { get; set; }
        public GameViewports GameViewports { get; }

        private GameMap GameMaps = new GameMap();
        private GameEnemies GameEnemies { get; set; }
        
        private PickupFactory PickupFactory { get; set; } = new PickupFactory();

        public SurvivalInitializer(GameViewports gameViewports, GameConfig gameConfig)
        {
            GameConfig = gameConfig;
            GameViewports = gameViewports;
        }
        
        public void InitializeEntities()
        {
            // Loading this projects content to be used by the game engine.
            GamePlayers = new GamePlayers(GameConfig, GameViewports);
            GameEnemies = new GameEnemies();
            // Game stuff
            GameMaps.SetupMap(GameConfig);
            GamePlayers.CreatePlayers(GameMaps);
            PickupFactory.CreatePickups();
            CreateGameEntities();
            CreateDefaultViewport();
        }
        
        private void CreateDefaultViewport()
        {
            var entity = EntityManager.GetEntityManager().NewEntity();

            var def = new DefaultViewport()
            {
                Viewport = GameViewports.defaultView
            };
            ComponentManager.Instance.AddComponentToEntity(def, entity);
        }

        private void CreateGameEntities()
        {
            var cameraCageId = SetupCameraCage();
            SetupHUD();

            if (GameConfig.GameMode == GameModeMenu.GameModes.Survival)
            {
                CreateGlobalSpawnSpriteEntity();
                CreateGlobalSpawnEntity();
            }
            CreateGlobalBulletSpriteEntity();
            SetupGameScoreEntity();
            SetupHighScoreEntity();
            // SetupTempPlayerDeadSpriteFlyweight();
        }

        private void SetupGameScoreEntity()
        {
            var gameScoreComponent = ComponentManager.Instance.ComponentFactory.NewComponent<GameScoreComponent>();
            ComponentManager.Instance.AddComponentToEntity(gameScoreComponent,
                EntityManager.GetEntityManager().NewEntity());
        }
        private void SetupHighScoreEntity()
        {
            var highScoreComponent = ComponentManager.Instance.ComponentFactory.NewComponent<HighScoreComponent>();
            ComponentManager.Instance.AddComponentToEntity(highScoreComponent,
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
                .SetRendering((int)(GameViewports.defaultView.X * 0.8), (int)(GameViewports.defaultView.Y * 0.8), isFixed: true)
                .SetRectangleCollision(isCage: true)
                .SetPosition(Vector2.Zero, 2)
                .Build()
                .GetEntityKey();

            var offsetComponent = ComponentFactory.NewComponent<RenderOffsetComponent>();
            offsetComponent.Offset = new Vector2((float)(GameViewports.defaultView.X * 0.25),
                (float)(GameViewports.defaultView.Y * 0.25));
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

    }
}