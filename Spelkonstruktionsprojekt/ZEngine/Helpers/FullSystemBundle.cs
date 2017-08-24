using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Diagnostics;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using Spelkonstruktionsprojekt.ZEngine.Systems.Motion;
using Spelkonstruktionsprojekt.ZEngine.Systems.Rendering;
using ZEngine.Managers;

using ZEngine.Systems;
using ZEngine.Systems.Collisions;
using ZEngine.Systems.InputHandler;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    public class FullSystemBundle
    {
        private SystemManager manager = SystemManager.Instance;

        private KeyboardState _oldKeyboardState = Keyboard.GetState();
        private Vector2 viewportDimensions = new Vector2(1800, 1300);
        private PenumbraComponent penumbraComponent;
        private Game game;
        private SpriteBatch sb;

        public ZEngineLogger Logger { get; set; }
        
        public void Initialize(Game game, SpriteFont font)
        {
            sb = new SpriteBatch(game.GraphicsDevice);

            this.game = game;
            manager.InstantiateAllSystems();

            //Init systems that require initialization
            manager.Get<TankMovementSystem>().Start();
           // manager.Get<WallCollisionSystem>().Start();
           // manager.Get<EnemyCollisionSystem>().Start();
            manager.Get<BulletCollisionSystem>().Start();
            manager.Get<LightAbilitySystem>().Start();
            manager.Get<SpriteAnimationSystem>().Start();
            manager.Get<QuickTurnAbilitySystem>().Start();
            manager.Get<SprintAbilitySystem>().Start();
            manager.Get<ScoreSystem>().Start();
            manager.Get<PickupCollisionSystem>().Start();
            manager.Get<ReloadSystem>().Start();
            manager.Get<EntityRemovalSystem>().Start();
            manager.Get<PickupSpawnSystem>().Start();
            manager.Get<KillSwitchSystem>().Start();
            manager.Get<KillSwitchEventFactory>().Start();
           // manager.Get<AiWallCollisionSystem>().Start();
            manager.Get<RenderHUDSystem>().Start(font);
        }

        public void LoadContent()
        {
            //manager.Get<LoadContentSystem>().LoadContent(this.Dependencies.Game.Content);
            // Want to initialize penumbra after loading all the game content.
            penumbraComponent = manager.Get<FlashlightSystem>().LoadPenumbra(game);
        }

        private void Log(string label, long ticks)
        {
            Logger.LogSystemTicks("CoreSystems", label, ticks);
        }
        
        private const bool PROFILING = false;
        public async void Update(GameTime gameTime)
        {
            Stopwatch timer;
            if (PROFILING)
            {
                timer = Stopwatch.StartNew();
            }
            manager.Get<EnemyCollisionSystem>().GameTime = gameTime; //TODO system dependency
            manager.Get<InputHandler>().HandleInput(_oldKeyboardState, gameTime);
            manager.Get<InputHandler>().HandleGamePadInput(gameTime);
            _oldKeyboardState = Keyboard.GetState();
            manager.Get<GamePadMovementSystem>().WalkForwards(gameTime);
            if (PROFILING)
            {
                Log("Input systems", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<CollisionResolveSystem>().ResolveCollisions(ZEngineCollisionEventPresets.StandardCollisionEvents, gameTime);
            if (PROFILING)
            {
                Log("CollisionResolveSystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<EventZoneSystem>().Handle(gameTime);
            if (PROFILING)
            {
                Log("EventZoneSystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<CameraSceneSystem>().Update(gameTime);
            if (PROFILING)
            {
                Log("CameraSceneSystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<MoveSystem>().Move(gameTime);
            if (PROFILING)
            {
                Log("MoveSystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<AISystem>().Update(gameTime);
            if (PROFILING)
            {
                Log("AISystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            await manager.Get<CollisionSystem>().DetectCollisions();
            if (PROFILING)
            {
                Log("CollisionSystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<AnimationSystem>().UpdateAnimations(gameTime);
            if (PROFILING)
            {
                Log("AnimationSystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<SpriteAnimationSystem>().Update(gameTime);
            if (PROFILING)
            {
                Log("SpriteAnimationSystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<FlashlightSystem>().Update(gameTime, viewportDimensions);
            if (PROFILING)
            {
                Log("FlashlightSystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<FlickeringLightSystem>().Update(gameTime);
            if (PROFILING)
            {
                Log("FlickeringLight", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<HealthSystem>().Update(gameTime);
            manager.Get<InertiaDampenerSystem>().Apply(gameTime);
            manager.Get<BackwardsPenaltySystem>().Apply();
            manager.Get<ScoreSystem>().Update(gameTime);
            if (PROFILING)
            {
                Log("Misc.", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
            manager.Get<EntityRemovalSystem>().Update(gameTime);
            if (PROFILING)
            {
                Log("EntityRemovalSystem", timer.ElapsedTicks);
                timer = Stopwatch.StartNew();
            }
        }

        public void Draw(GameTime gameTime)
        {
            manager.Get<FlashlightSystem>().BeginDraw(penumbraComponent);
            manager.Get<RenderSystem>().Render(sb, gameTime); // lowers FPS by half (2000)
            manager.Get<FlashlightSystem>().EndDraw(penumbraComponent, gameTime);
            manager.Get<TextSystem>().Draw(sb);
            manager.Get<RenderHUDSystem>().Draw(sb); // not noticable
        }

        public void ClearCaches()
        {
            manager.Get<RenderSystem>().ClearCache();
            //manager.Get<CollisionSystem>().ClearCache();
        }
    }
}
