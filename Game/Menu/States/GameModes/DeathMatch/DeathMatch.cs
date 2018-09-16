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
        public VirtualGamePad MenuController { get; set; }
        public GameEngine GameSystems { get; set; }
        public MenuNavigator MenuNavigator { get; set; }
        public GameConfig GameConfig { get; set; }
        public Viewport Viewport { get; set; }
        
        private SoundSystem SoundSystem { get; set; } = new SoundSystem();

        private WeaponSystem WeaponSystem { get; set; } = new WeaponSystem();
        private HealthSystem HealthSystem { get; set; } = new HealthSystem();

        private SpawnSystem SpawnSystem { get; set; } = new SpawnSystem();

        private BackgroundMusic BackgroundMusic { get; set; } = new BackgroundMusic();
        private StartTimer StartTimer { get; set; }
        private GameViewports GameViewports { get; set; }
        
        private DeathMatchInitializer DeathMatchInitializer { get; set; }

        private CountdownTimer countdownTimer;
        
        public DeathMatch(GameModeDependencies dependencies)
        {
            GameConfig = dependencies.GameConfig;
            Viewport = dependencies.Viewport;
            //GameSystems = new GameEngine();
            MenuNavigator = dependencies.MenuNavigator;
            MenuController = dependencies.VirtualInputs.PlayerOne();
            countdownTimer = new CountdownTimer(3);

            SpawnSystem.Start();
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            //GameSystems.Draw(gameTime);
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


            spriteBatch.DrawString(OutlivedGame.Instance().Fonts["ZMenufont"], 
                countdownTimer.GetFormatedTime(), 
                new Vector2(((GameViewports.defaultView.Width - OutlivedGame.Instance().Fonts["ZMenufont"].MeasureString(countdownTimer.GetFormatedTime()).X) * 0.5f), GameViewports.defaultView.Y * 0.5f), 
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

            countdownTimer.StartCounter();
            countdownTimer.UpdateTimer(gameTime);
            BackgroundMusic.PlayMusic();
//            SpawnSystem.Update();
            GameSystems.Update(gameTime);


            if (countdownTimer.IsDone)
            {
                MenuNavigator.GoTo(OutlivedStates.GameState.GameOver);
            }
        }

        public void Reset()
        {
        }

        public void BeforeShow()
        {
            GameSystems.Start(OutlivedGame.Instance().Fonts["ZEone"]);
            GameViewports = new GameViewports(GameConfig, Viewport);
            GameViewports.InitializeViewports();
            DeathMatchInitializer = new DeathMatchInitializer(GameViewports, GameConfig);
            StartTimer = new StartTimer(0, OutlivedGame.Instance().Get<SpriteFont>("Fonts/ZlargeFont"),
                GameViewports.defaultView);

            // Loading this projects content to be used by the game engine.
            //SystemManager.Instance.Get<GameContent>().LoadContent(OutlivedGame.Instance().Content);
            GameSystems.LoadContent();
            DeathMatchInitializer.InitializeEntities();
            BackgroundMusic.LoadSongs("bg_actionmusic1", "bg_actionmusic1", "bg_actionmusic1", "bg_actionmusic1");
            WeaponSystem.LoadBulletSpriteEntity();

            // Specific systems activated
            SoundSystem.Start();
            WeaponSystem.Start();

            // Load content from this game
            //SystemManager.Instance.Get<GameContent>().LoadContent(OutlivedGame.Instance().Content);
        }

        public void BeforeHide()
        {            
            SoundSystem.Stop();
            WeaponSystem.Stop();
            ComponentManager.Instance.Clear();
            GameConfig.Reset();
            GameSystems.Reset();
            BackgroundMusic.ClearList();
            MediaPlayer.Stop();
        }
    }
}