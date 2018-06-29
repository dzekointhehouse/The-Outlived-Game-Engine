using Game.Services;
using Game.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Managers;
using ZEngine.Systems;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States.GameModes
{
    public class Survival : IMenu, ILifecycle
    {
        private GameConfig GameConfig { get; }
        private Viewport Viewport { get; }
        private FullSystemBundle EngineGameSystems;
        private MenuNavigator MenuNavigator { get; }
        private VirtualGamePad MenuController { get; }

        private SoundSystem SoundSystem { get; set; } = new SoundSystem();

        private ProbabilitySystem ProbabilitySystem { get; set; } = new ProbabilitySystem();
        private SpawnSystem SpawnSystem { get; set; } = new SpawnSystem();
        private WeaponSystem WeaponSystem { get; set; } = new WeaponSystem();
        private HealthSystem HealthSystem { get; set; } = new HealthSystem();

        private BackgroundMusic BackgroundMusic { get; set; } = new BackgroundMusic();
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
            EngineGameSystems.Draw(gameTime);
            StartTimer.Draw(sb);
            DrawHUDs(sb);
        }

        private void DrawHUDs(SpriteBatch spriteBatch)
        {
            // Reset to default view
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
            SpawnSystem.HandleWaves(gameTime);
            EngineGameSystems.Update(gameTime);
            ProbabilitySystem.Generate();

            if (HealthSystem.CheckIfAllPlayersAreDead())
            {
                GameOver = true;

                MenuNavigator.GoTo(GameManager.GameState.GameOver);
            }
        }

        public void Reset()
        {
        }

        public void BeforeShow()
        {
            EngineGameSystems = new FullSystemBundle();
            GameViewports = new GameViewports(GameConfig, Viewport);
            GameViewports.InitializeViewports();
            SurvivalInitializer = new SurvivalInitializer(GameViewports, GameConfig);
            StartTimer = new StartTimer(0, OutlivedGame.Instance().Get<SpriteFont>("Fonts/ZlargeFont"),
                GameViewports.defaultView);

            EngineGameSystems.Initialize(OutlivedGame.Instance(), OutlivedGame.Instance().Fonts["ZEone"]);
            SurvivalInitializer.InitializeEntities();
            EngineGameSystems.LoadContent();
            BackgroundMusic.LoadSongs("bg_music1", "bg_music3", "bg_music3", "bg_music4");
            WeaponSystem.LoadBulletSpriteEntity();

            SoundSystem.Start();
            WeaponSystem.Start();
            // Game stuff
            SystemManager.Instance.GetSystem<LoadContentSystem>().LoadContent(OutlivedGame.Instance().Content);
        }

        public void BeforeHide()
        {
            SoundSystem.Stop();
            WeaponSystem.Stop();
            GameConfig.Reset();
            EngineGameSystems.ClearCaches();
            ComponentManager.Instance.Clear();
            BackgroundMusic.ClearList();
            MediaPlayer.Stop();
        }
    }
}