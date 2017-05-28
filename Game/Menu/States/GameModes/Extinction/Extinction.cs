using System;
using System.Collections.Generic;
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
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States.GameModes.Extinction
{
    public class Extinction : IMenu, ILifecycle
    {
        private GameConfig GameConfig { get; }
        private Viewport Viewport { get; }
        private FullSystemBundle SystemsBundle { get; }
        private MenuNavigator MenuNavigator { get; }
        private VirtualGamePad MenuController { get; }

        private SoundSystem SoundSystem { get; set; } = new SoundSystem();
        private WeaponSystem WeaponSystem { get; set; } = new WeaponSystem();
        private HealthSystem HealthSystem { get; set; } = new HealthSystem();
        private SpawnPointSystem SpawnPointSystem { get; set; } = new SpawnPointSystem();

        private BackgroundMusic BackgroundMusic { get; set; } = new BackgroundMusic();
        private Timer Timer { get; set; }
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SystemsBundle.Draw(gameTime);
            Timer.Draw(spriteBatch);
            DrawCameras(spriteBatch);
        }

        private void DrawCameras(SpriteBatch spriteBatch)
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
            SystemsBundle.Update(gameTime);
            
            if (HealthSystem.CheckIfAllPlayersAreDead())
            {

                BackgroundMusic.ClearList();
                MenuNavigator.GoTo(GameManager.GameState.GameOverCredits);
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
            Timer = new Timer(0, OutlivedGame.Instance().Get<SpriteFont>("Fonts/ZlargeFont"),
                GameViewports.defaultView);
            ExtinctionInitializer.InitializeEntities();
            
            // Loading this projects content to be used by the game engine.
            SystemManager.Instance.GetSystem<LoadContentSystem>().LoadContent(OutlivedGame.Instance().Content);
            SystemsBundle.LoadContent();
            BackgroundMusic.LoadSongs("bg_music1", "bg_music3", "bg_music3", "bg_music4");
//            CreateEnemy();
            WeaponSystem.LoadBulletSpriteEntity();

            SoundSystem.Start();
            WeaponSystem.Start();
            SpawnPointSystem.Start();
            // Game stuff
            SystemManager.Instance.GetSystem<LoadContentSystem>().LoadContent(OutlivedGame.Instance().Content);
        }

        public void BeforeHide()
        {
            SoundSystem.Stop();
            WeaponSystem.Stop();
            SpawnPointSystem.Stop();
//            foreach (var entity in EntityManager.GetEntityManager().GetListWithEntities())
//            {
//                ComponentManager.Instance.DeleteEntity(entity);
//            }
            ComponentManager.Instance.Clear();
            GameConfig.Reset();
            SystemsBundle.ClearCaches();

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