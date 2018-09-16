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

namespace Game.Menu.States.GameModes.DeathMatch
{
    public class DeathMatch : IMenu, ILifecycle
    {
        private GameEngine GameEngine { get; set; }
        private GameConfig GameConfig { get; }
        private Viewport Viewport { get; }
        private MenuNavigator MenuNavigator { get; }
        private VirtualGamePad MenuController { get; }

        private SoundSystem SoundSystem { get; set; }
        private SpawnSystem SpawnSystem { get; set; }
        private WeaponSystem WeaponSystem { get; set; }
        private HealthSystem HealthSystem { get; set; }
        private ProbabilitySystem ProbabilitySystem { get; set; }

        private BackgroundMusic BackgroundMusic { get; set; } = new BackgroundMusic();
        private StartTimer StartTimer { get; set; }
        private CountdownTimer CountdownTimer { get; set; }

        private GameViewports GameViewports { get; set; }
        
        private DeathMatchInitializer DeathMatchInitializer { get; set; }

        
        public DeathMatch(GameModeDependencies dependencies)
        {
            GameConfig = dependencies.GameConfig;
            Viewport = dependencies.Viewport;
            MenuNavigator = dependencies.MenuNavigator;
            MenuController = dependencies.VirtualInputs.PlayerOne();
            CountdownTimer = new CountdownTimer(3);
            CountdownTimer.StartCounter();

            SoundSystem = new SoundSystem();
            ProbabilitySystem = new ProbabilitySystem();
            SpawnSystem = new SpawnSystem();
            WeaponSystem = new WeaponSystem();
            //var HealthSystem = new HealthSystem();

            GameEngine = new GameEngine(OutlivedGame.Instance());
            GameEngine.AddSystems(SoundSystem, SpawnSystem, WeaponSystem, ProbabilitySystem);
            GameEngine.Start(AssetManager.Instance.Get<SpriteFont>("ZEone"));

        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            GameEngine.Draw(sb, gameTime);
            StartTimer.Draw(sb);
            DrawCameras(sb);
        }

        private void DrawCameras(SpriteBatch spriteBatch)
        {
            // Clear to default view
            OutlivedGame.Instance().GraphicsDevice.Viewport = GameViewports.defaultView;

            if (StartTimer.IsCounting)
            {
                return;
            }
            // Should move to HUD which should render defaultview
            spriteBatch.Begin();
            var nCameras = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent)).Count;


            spriteBatch.DrawString(AssetManager.Instance.Get<SpriteFont>("Fonts/ZMenufont"), 
                CountdownTimer.GetFormatedTime(), 
                new Vector2(((GameViewports.defaultView.Width - AssetManager.Instance.Get<SpriteFont>("Fonts/ZMenufont").MeasureString(CountdownTimer.GetFormatedTime()).X) * 0.5f), GameViewports.defaultView.Y * 0.5f), 
                Color.White);

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

            CountdownTimer.StartCounter();
            CountdownTimer.UpdateTimer(gameTime);
            BackgroundMusic.PlayMusic();
            GameEngine.Update(gameTime);


            if (CountdownTimer.IsDone || GameEngine.GetSystem<HealthSystem>().CheckIfAllPlayersAreDead())
            {
                // TODO score state!!
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

            SoundSystem = new SoundSystem();
            ProbabilitySystem = new ProbabilitySystem();
            SpawnSystem = new SpawnSystem();
            WeaponSystem = new WeaponSystem();
            //var HealthSystem = new HealthSystem();

            GameEngine = new GameEngine(OutlivedGame.Instance());
            GameEngine.AddSystems(SoundSystem, SpawnSystem, WeaponSystem, ProbabilitySystem);
            GameEngine.Start(AssetManager.Instance.Get<SpriteFont>("ZEone"));

            GameViewports.InitializeViewports();
            DeathMatchInitializer = new DeathMatchInitializer(GameViewports, GameConfig);
            StartTimer = new StartTimer(0, OutlivedGame.Instance().Get<SpriteFont>("Fonts/ZlargeFont"),
                GameViewports.defaultView);

            // Loading this projects content to be used by the game engine.
            //SystemManager.Instance.Get<GameContent>().LoadContent(OutlivedGame.Instance().Content);
            DeathMatchInitializer.InitializeEntities();
            GameEngine.LoadContent();

            BackgroundMusic.LoadSongs("bg_actionmusic1", "bg_actionmusic1", "bg_actionmusic1", "bg_actionmusic1");
            WeaponSystem.LoadBulletSpriteEntity();

            // Specific systems activated
            SoundSystem.Start();
            WeaponSystem.Start();

            // Load content from this game
            SystemManager.Instance.Get<LoadContentSystem>().LoadContent(OutlivedGame.Instance().Content);
        }

        public void BeforeHide()
        {            
            SoundSystem.Stop();
            WeaponSystem.Stop();
            ComponentManager.Instance.Clear();
            GameConfig.Reset();
            GameEngine.Reset(); ;
            BackgroundMusic.ClearList();
            MediaPlayer.Stop();
        }
    }
}