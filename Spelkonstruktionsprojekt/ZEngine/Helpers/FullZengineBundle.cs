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
using Spelkonstruktionsprojekt.ZEngine.Systems.Motion;
using Spelkonstruktionsprojekt.ZEngine.Systems.Rendering;
using ZEngine.Managers;

using ZEngine.Systems;
using ZEngine.Systems.Collisions;
using ZEngine.Systems.InputHandler;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    // Optimus prime
    public class FullZengineBundle
    {
        private SystemManager manager = SystemManager.Instance;

        private KeyboardState _oldKeyboardState = Keyboard.GetState();
        private Vector2 viewportDimensions = new Vector2(1800, 1300);
        private PenumbraComponent penumbraComponent;

        public GameDependencies Dependencies = GameDependencies.Instance;

        public void InitializeSystems(Game game)
        {
            Dependencies.GameContent = game.Content;
            Dependencies.SpriteBatch = new SpriteBatch(game.GraphicsDevice);
            Dependencies.Game = game;

            //Init systems that require initialization
            manager.Get<TankMovementSystem>().Start();
            manager.Get<WallCollisionSystem>().Start();
            manager.Get<SoundSystem>().Start();
            manager.Get<EnemyCollisionSystem>().Start();
            manager.Get<BulletCollisionSystem>().Start();
            manager.Get<LightAbilitySystem>().Start();
            manager.Get<SpriteAnimationSystem>().Start();
            manager.Get<QuickTurnAbilitySystem>().Start();
            manager.Get<SprintAbilitySystem>().Start();
            manager.Get<ScoreSystem>().Start();
            manager.Get<PickupCollisionSystem>().Start();
            manager.Get<ReloadSystem>().Start();

        }

        public void LoadContent()
        {
            manager.Get<LoadContentSystem>().LoadContent(this.Dependencies.Game.Content);
            // Want to initialize penumbra after loading all the game content.
            penumbraComponent = manager.Get<FlashlightSystem>().Initialize(Dependencies);

        }

        public void Update(GameTime gameTime)
        {
            manager.Get<EnemyCollisionSystem>().GameTime = gameTime; //TODO system dependency
            manager.Get<InputHandler>().HandleInput(_oldKeyboardState, gameTime);
            manager.Get<InputHandler>().HandleGamePadInput(gameTime);
            _oldKeyboardState = Keyboard.GetState();
            manager.Get<GamePadMovementSystem>().WalkForwards(gameTime);
            manager.Get<CollisionResolveSystem>().ResolveCollisions(ZEngineCollisionEventPresets.StandardCollisionEvents, gameTime);

            manager.Get<CameraSceneSystem>().Update(gameTime);
            manager.Get<MoveSystem>().Move(gameTime);
            manager.Get<AISystem>().Update(gameTime);
            manager.Get<CollisionSystem>().DetectCollisions();
            manager.Get<AnimationSystem>().UpdateAnimations(gameTime);
            manager.Get<SpriteAnimationSystem>().Update(gameTime);
            manager.Get<FlashlightSystem>().Update(gameTime, viewportDimensions);
            manager.Get<HealthSystem>().Update(gameTime);
            manager.Get<EntityRemovalSystem>().Update(gameTime);
            manager.Get<InertiaDampenerSystem>().Apply(gameTime);
            manager.Get<BackwardsPenaltySystem>().Apply();
            manager.Get<SpawnSystem>().HandleWaves();
            manager.Get<ScoreSystem>().Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            manager.Get<FlashlightSystem>().BeginDraw(penumbraComponent);
            manager.Get<RenderSystem>().Render(Dependencies); // lowers FPS by half (2000)
            manager.Get<FlashlightSystem>().EndDraw(penumbraComponent, gameTime);
            manager.Get<RenderHUDSystem>().Draw(Dependencies); // not noticable
        }
    }
}
