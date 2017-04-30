using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Managers;
using ZEngine.Systems;
using ZEngine.Systems.Collisions;
using ZEngine.Systems.InputHandler;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    class FullZengineBundle
    {
        private RenderSystem RenderSystem;
        private LoadContentSystem LoadContentSystem;
        private InputHandler InputHandlerSystem;
        private MoveSystem MoveSystem;
        private TankMovementSystem TankMovementSystem;
        private TitlesafeRenderSystem TitlesafeRenderSystem;
        private CollisionSystem CollisionSystem;
        private CameraSceneSystem CameraFollowSystem;
        private FlashlightSystem LightSystems;
        private CollisionResolveSystem CollisionResolveSystem;
        private WallCollisionSystem WallCollisionSystem;
        private EnemyCollisionSystem EnemyCollisionSystem;
        private BulletCollisionSystem BulletCollisionSystem;
        private AISystem AISystem;
        private AnimationSystem AnimationSystem;
        private SoundSystem SoundSystem;
        private WeaponSystem WeaponSystem;
        private HealthSystem HealthSystem;

        private KeyboardState _oldKeyboardState = Keyboard.GetState();
        private Vector2 viewportDimensions = new Vector2(1800, 1300);
        private PenumbraComponent penumbraComponent;

        private readonly GameDependencies _gameDependencies = new GameDependencies();

        public void Initialize()
        {
            RenderSystem = SystemManager.Instance.GetSystem<RenderSystem>();
            LoadContentSystem = SystemManager.Instance.GetSystem<LoadContentSystem>();
            InputHandlerSystem = SystemManager.Instance.GetSystem<InputHandler>();
            TankMovementSystem = SystemManager.Instance.GetSystem<TankMovementSystem>();
            TitlesafeRenderSystem = SystemManager.Instance.GetSystem<TitlesafeRenderSystem>();
            CollisionSystem = SystemManager.Instance.GetSystem<CollisionSystem>();
            CameraFollowSystem = SystemManager.Instance.GetSystem<CameraSceneSystem>();
            LightSystems = SystemManager.Instance.GetSystem<FlashlightSystem>();
            MoveSystem = SystemManager.Instance.GetSystem<MoveSystem>();
            CollisionResolveSystem = SystemManager.Instance.GetSystem<CollisionResolveSystem>();
            WallCollisionSystem = SystemManager.Instance.GetSystem<WallCollisionSystem>();
            AISystem = SystemManager.Instance.GetSystem<AISystem>();
            EnemyCollisionSystem = SystemManager.Instance.GetSystem<EnemyCollisionSystem>();
            AnimationSystem = SystemManager.Instance.GetSystem<AnimationSystem>();
            SoundSystem = SystemManager.Instance.GetSystem<SoundSystem>();
            WeaponSystem = SystemManager.Instance.GetSystem<WeaponSystem>();
            BulletCollisionSystem = SystemManager.Instance.GetSystem<BulletCollisionSystem>();
            HealthSystem = SystemManager.Instance.GetSystem<HealthSystem>();
        }

        public void LoadSystems(Game game)
        {
            _gameDependencies.GameContent = game.Content;
            _gameDependencies.SpriteBatch = new SpriteBatch(game.GraphicsDevice);
            _gameDependencies.Game = game;

            TankMovementSystem.Start();
            WallCollisionSystem.Start();
            SoundSystem.Start();
            WeaponSystem.Start();
            BulletCollisionSystem.Start();
        }

        public void Update(GameTime gameTime)
        {
            EnemyCollisionSystem.GameTime = gameTime;
            InputHandlerSystem.HandleInput(_oldKeyboardState, gameTime);
            _oldKeyboardState = Keyboard.GetState();

            AISystem.Update(gameTime);
            MoveSystem.Move(gameTime);
            AnimationSystem.RunAnimations(gameTime);

            CollisionSystem.DetectCollisions();
            CollisionResolveSystem.ResolveCollisions(ZEngineCollisionEventPresets.StandardCollisionEvents, gameTime);

            CameraFollowSystem.Update(gameTime);
            LightSystems.Update(gameTime, viewportDimensions);
           // HealthSystem.TempEndGameIfDead(TempGameEnder);
        }

        public void Draw(GameTime gameTime)
        {
            LightSystems.BeginDraw(penumbraComponent);
            RenderSystem.Render(_gameDependencies);
            LightSystems.EndDraw(penumbraComponent, gameTime);
            TitlesafeRenderSystem.Draw(_gameDependencies);
        }
    }
}
