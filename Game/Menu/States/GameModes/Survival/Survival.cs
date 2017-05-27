using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Game.Entities;
using Game.Services;
using Game.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Managers;
using ZEngine.Systems;
using ZEngine.Wrappers;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States.GameModes
{
    public class Survival : IMenu, ILifecycle
    {
        private GameConfig GameConfig { get; }
        private Viewport Viewport { get; }
        private FullSystemBundle SystemsBundle { get; }
        private MenuNavigator MenuNavigator { get; }
        private VirtualGamePad MenuController { get; }

        private SoundSystem SoundSystem { get; set; } = new SoundSystem();
        private SpawnSystem SpawnSystem { get; set; } = new SpawnSystem();
        private WeaponSystem WeaponSystem { get; set; } = new WeaponSystem();
        private HealthSystem HealthSystem { get; set; } = new HealthSystem();

        private BackgroundMusic BackgroundMusic { get; set; } = new BackgroundMusic();
        private Timer Timer { get; set; }
        private GameViewports GameViewports { get; set; }

        private SurvivalInitializer SurvivalInitializer { get; set; }

        private bool GameOver = false;
        
        public Survival(GameModeDependencies dependencies)
        {
            GameConfig = dependencies.GameConfig;
            Viewport = dependencies.Viewport;
            SystemsBundle = dependencies.SystemsBundle;
            MenuNavigator = dependencies.MenuNavigator;
            MenuController = dependencies.VirtualInputs.PlayerOne();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SystemsBundle.Draw(gameTime);
            Timer.Draw(spriteBatch);
            DrawHUDs(spriteBatch);
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
            
            Timer.Update(gameTime);
            BackgroundMusic.PlayMusic();
            SpawnSystem.HandleWaves();
            SystemsBundle.Update(gameTime);
            
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
            GameViewports = new GameViewports(GameConfig, Viewport);
            GameViewports.InitializeViewports();
            SurvivalInitializer = new SurvivalInitializer(GameViewports, GameConfig);
            Timer = new Timer(0, OutlivedGame.Instance().Get<SpriteFont>("Fonts/ZlargeFont"),
                GameViewports.defaultView);
            
            SurvivalInitializer.InitializeEntities();
            SystemsBundle.LoadContent();
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
            SystemsBundle.ClearCaches();
            if (!GameOver)
            {
                ComponentManager.Instance.Clear();
            }
        }
    }
}