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

namespace Game.Menu.States
{
    class SurvivalGame : IMenu
    {

        private GameManager gameManager;
        private ControlsConfig controls;
        private bool isInitialized = false;
        private Boolean isIngame = true;
        private CreatePlayers playerCreator = new CreatePlayers();

        // SOME BUG NEED THIS.
        private Vector2 viewportDimensions = new Vector2(1800, 1300);
        public SurvivalGame(GameManager gameManager)
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
        }

        // To initialize game content when navigating
        // to this state.
        public void InitializeGameContent()
        {
            CreateGameEntities();
        }


        // TEST CODE
        private void InitializeGamePlayers()
        {
            foreach (var player in gameManager.gameConfig.Players)
            {
                if (player.Team == MultiplayerMenu.TeamState.TeamOne)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            playerCreator.InitPlayerOne(1);
                            break;
                        case PlayerIndex.Two:
                            playerCreator.InitPlayerTwo(1);
                            break;
                        case PlayerIndex.Three:
                            playerCreator.InitPlayerThree(1);
                            break;
                        //case PlayerIndex.Four:
                        //    CreatePlayers.i(1);
                        //    break;
                    };  
                }
                if (player.Team == MultiplayerMenu.TeamState.TeamTwo)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            playerCreator.InitPlayerOne(1);
                            break;
                        case PlayerIndex.Two:
                            playerCreator.InitPlayerTwo(1);
                            break;
                        case PlayerIndex.Three:
                            playerCreator.InitPlayerThree(1);
                            break;
                            //case PlayerIndex.Four:
                            //    CreatePlayers.i(1);
                            //    break;
                    };
                }

            }


        }
        

        private void CreateGameEntities()
        {
            var cameraCageId = SetupCameraCage();
            SetupBackgroundTiles(5, 5);
            SetupCamera();
            InitializeGamePlayers();
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

        
    }
}
