using Game.Services;
using Game.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States.GameModes
{
    public class Survival : IMenu, ILifecycle
    {

        private GameEngine GameEngine;
        private GameConfig GameConfig { get; }
        private Viewport Viewport { get; }      
        private MenuNavigator MenuNavigator { get; }
        private VirtualGamePad MenuController { get; }

        private SoundSystem SoundSystem { get; set; }
        private ProbabilitySystem ProbabilitySystem { get; set; }
        private SpawnSystem SpawnSystem { get; set; }
        private WeaponSystem WeaponSystem { get; set; }
        private HealthSystem HealthSystem { get; set; }

        private BackgroundMusic BackgroundMusic { get; set; }
        private StartTimer StartTimer { get; set; }
        private GameViewports GameViewports { get; set; }

        private SurvivalInitializer SurvivalInitializer { get; set; }

        private bool GameOver = false;

        public Survival(GameModeDependencies dependencies)
        {
            GameConfig = dependencies.GameConfig;
            Viewport = dependencies.Viewport;
            MenuNavigator = dependencies.MenuNavigator;
            MenuController = dependencies.VirtualInputs.PlayerOne();
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            GameEngine.Draw(sb, gameTime);
            StartTimer.Draw(sb);
            DrawHUDs(sb);
        }

        private void DrawHUDs(SpriteBatch spriteBatch)
        {
            // Clear to default view
            OutlivedGame.Instance().GraphicsDevice.Viewport = GameViewports.defaultView;

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
                        GameViewports.defaultView.TitleSafeArea, Color.White);
                    break;
                default:
                    spriteBatch.Draw(OutlivedGame.Instance().Content.Load<Texture2D>("Images/4border"),
                        GameViewports.defaultView.TitleSafeArea, Color.White);
                    break;
            }
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (MenuController.Is(Pause, Pressed))
            {
                MenuNavigator.Pause();
            }

            StartTimer.Update(gameTime);
            if (StartTimer.IsCounting)
            {
                return;
            }
            
            BackgroundMusic.PlayMusic();
            //SpawnSystem.Update(gameTime);
            GameEngine.Update(gameTime);
            //ProbabilitySystem.Update(gameTime);

            if (GameEngine.GetSystem<HealthSystem>().CheckIfAllPlayersAreDead())
            {
                GameOver = true;

                MenuNavigator.GoTo(OutlivedStates.GameState.GameOver);
            }
        }

        public void Reset()
        {
        }

        public void BeforeShow()
        {

            GameViewports = new GameViewports(GameConfig, Viewport);
            BackgroundMusic = new BackgroundMusic();

            var SoundSystem = new SoundSystem();
            var ProbabilitySystem = new ProbabilitySystem();
            var SpawnSystem = new SpawnSystem();
            var WeaponSystem = new WeaponSystem();
            //var HealthSystem = new HealthSystem();

            GameEngine = new GameEngine(OutlivedGame.Instance());
            GameEngine.AddSystems(SoundSystem, SpawnSystem, WeaponSystem, ProbabilitySystem);
            GameEngine.Start(AssetManager.Instance.Get<SpriteFont>("ZEone"));


            GameViewports.InitializeViewports();
            SurvivalInitializer = new SurvivalInitializer(GameViewports, GameConfig);
            StartTimer = new StartTimer(0, OutlivedGame.Instance().Get<SpriteFont>("Fonts/ZlargeFont"),
                GameViewports.defaultView);

            SurvivalInitializer.InitializeEntities();
            GameEngine.LoadContent();


            BackgroundMusic.LoadSongs("bg_music1", "bg_music3", "bg_music3", "bg_music4");
            WeaponSystem.LoadBulletSpriteEntity();

            SoundSystem.Start();
            WeaponSystem.Start();


            SystemManager.Instance.Get<LoadContentSystem>().LoadContent(OutlivedGame.Instance().Content);
        }

        public void BeforeHide()
        {
            //SoundSystem.Stop();
            //WeaponSystem.Stop();
            GameEngine.Reset();
            GameConfig.Reset();
            BackgroundMusic.ClearList();
            MediaPlayer.Stop();
        }
    }
}