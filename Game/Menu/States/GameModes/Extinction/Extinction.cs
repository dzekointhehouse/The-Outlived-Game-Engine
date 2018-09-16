using System;
using System.Collections.Generic;
using System.Diagnostics;
using Game.Entities.Zones;
using Game.Services;
using Game.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Systems;
using static Game.Menu.OutlivedStates;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States.GameModes.Extinction
{
    public class Extinction : IMenu, ILifecycle
    {
        private GameConfig GameConfig { get; }
        private Viewport Viewport { get; }
        private GameEngine SystemsBundle { get; }
        private MenuNavigator MenuNavigator { get; }
        private VirtualGamePad MenuController { get; }

        private SoundSystem SoundSystem { get; set; } = new SoundSystem();
        private WeaponSystem WeaponSystem { get; set; } = new WeaponSystem();
        private HealthSystem HealthSystem { get; set; } = new HealthSystem();
        private SpawnPointSystem SpawnPointSystem { get; set; } = new SpawnPointSystem();
        private CarSystem CarSystem { get; set; } = new CarSystem();

        private DowntownZone DowntownZone { get; set; } = new DowntownZone();

        private BackgroundMusic BackgroundMusic { get; set; } = new BackgroundMusic();
        private StartTimer StartTimer { get; set; }
        private GameViewports GameViewports { get; set; }

        private ExtinctionInitializer ExtinctionInitializer { get; set; }

        public Extinction(GameModeDependencies dependencies)
        {
            GameConfig = dependencies.GameConfig;
            Viewport = dependencies.Viewport;
            SystemsBundle = dependencies.SystemsBundle;
            MenuNavigator = dependencies.MenuNavigator;
            MenuController = dependencies.VirtualInputs.PlayerOne();
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            //SystemsBundle.Draw(gameTime);
            StartTimer.Draw(sb);
            DrawCameras(sb);
        }

        private void DrawCameras(SpriteBatch spriteBatch)
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

        private const bool PROFILING = true;
        public void Update(GameTime gameTime)
        {
            if (MenuController.Is(Pause, Pressed))
            {
                MenuNavigator.Pause();
            }

            Stopwatch timer;
            if (PROFILING)
            {
                timer = Stopwatch.StartNew();
            }
            StartTimer.Update(gameTime);
            BackgroundMusic.PlayMusic();
            SystemsBundle.Update(gameTime);
            CarSystem.Update();
            if (HealthSystem.CheckIfAllPlayersAreDead())
            {
                BackgroundMusic.ClearList();
                MenuNavigator.GoTo(GameState.GameOverCredits);
            }
        }

        public void Reset()
        {
        }

        public void BeforeShow()
        {
            GameViewports = new GameViewports(GameConfig, Viewport);
            GameViewports.InitializeViewports();
            ExtinctionInitializer = new ExtinctionInitializer(GameViewports, GameConfig);
            StartTimer = new StartTimer(0, OutlivedGame.Instance().Get<SpriteFont>("Fonts/ZlargeFont"),
                GameViewports.defaultView);
            ExtinctionInitializer.InitializeEntities();

            // Loading this projects content to be used by the game engine.
            //SystemManager.Instance.Get<GameContent>().LoadContent(OutlivedGame.Instance().Content);
            SystemsBundle.LoadContent();
            BackgroundMusic.LoadSongs("bg_music1", "bg_music3", "bg_music3", "bg_music4");
            WeaponSystem.LoadBulletSpriteEntity();

            SoundSystem.Start();
            WeaponSystem.Start();
            SpawnPointSystem.Start();
            DowntownZone.Start();
            CarSystem.Start();
            // Game stuff
            //ystemManager.Instance.Get<GameContent>().LoadContent(OutlivedGame.Instance().Content);
        }

        public void BeforeHide()
        {
            SoundSystem.Stop();
            WeaponSystem.Stop();
            SpawnPointSystem.Stop();
            DowntownZone.Stop();
            CarSystem.Stop();
            //            foreach (var entity in EntityManager.GetEntityManager().GetListWithEntities())
            //            {
            //                ComponentManager.Instance.DeleteEntity(entity);
            //            }
            ComponentManager.Instance.Clear();
            GameConfig.Reset();
            SystemsBundle.Reset();

            MediaPlayer.Stop();

            Song song = OutlivedGame.Instance().Get<Song>("Sound/Clearwater");
            MediaPlayer.Play(song);

        }

        public void CreateEnemy()
        {
            Dictionary<SoundComponent.SoundBank, SoundEffectInstance> soundList = new Dictionary<SoundComponent.SoundBank, SoundEffectInstance>(1);

            soundList.Add(SoundComponent.SoundBank.Death, OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/Splash")
                .CreateInstance());

            var monster = new EntityBuilder()
                .SetPosition(new Vector2(300, 200), layerDepth: 20)
                .SetRendering(200, 200)
                .SetSprite("zombie1", new Point(1244, 311), 311, 311)
                .SetSound(soundList: soundList)
                .SetMovement(50, 5, 0.5f, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetArtificialIntelligence()
                .SetSpawn()
                .SetRectangleCollision()
                .SetHealth()
                .BuildAndReturnId();

            var animationBindings = new SpriteAnimationBindingsBuilder()
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(1244, 311), new Point(622, 1244))
                        .StateConditions(State.WalkingForward)
                        .Length(60)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(933, 311))
                        .StateConditions(State.Dead, State.WalkingForward)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(933, 311))
                        .StateConditions(State.Dead, State.WalkingBackwards)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(933, 311))
                        .StateConditions(State.Dead)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Build();

            ComponentManager.Instance.AddComponentToEntity(animationBindings, monster);

            //TODO SEND STATE MANAGER A GAME TIME VALUE AND NOT 0
            StateManager.TryAddState(monster, State.WalkingForward, 0);
        }
    }
}