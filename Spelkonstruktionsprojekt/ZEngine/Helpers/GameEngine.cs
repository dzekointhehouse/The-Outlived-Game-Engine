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
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using Spelkonstruktionsprojekt.ZEngine.Systems.Motion;
using Spelkonstruktionsprojekt.ZEngine.Systems.Rendering;
using ZEngine.EventBus;
using ZEngine.Managers;

using ZEngine.Systems;
using ZEngine.Systems.Collisions;
using ZEngine.Systems.InputHandler;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    public class GameEngine
    {
        private SystemManager manager = SystemManager.Instance;
        private PenumbraComponent _penumbraComponent;
        private Game game;     

        public GameEngine(Game game)
        {
            this.game = game;

            var systems =  CreateSystems();
            manager.UpdateableSystems = systems.updateables;
            manager.DrawableSystems = systems.drawables;
        }



        public void Start(SpriteFont font)
        {

            //Init systems that require initialization
            manager.Get<TankMovementSystem>().Start();
            manager.Get<WallCollisionSystem>().Start();
            manager.Get<EnemyCollisionSystem>().Start();
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
            manager.Get<AiWallCollisionSystem>().Start();
            manager.Get<RenderHUDSystem>().Start(font);
        }

        public void LoadContent()
        {
            //manager.Get<LoadContentSystem>().LoadContent(this.Dependencies.Game.Content);
            // Want to initialize penumbra after loading all the game content.
            _penumbraComponent = manager.Get<FlashlightSystem>().LoadPenumbra(game);
        }

        public void Update(GameTime gameTime)
        {
            manager.Update(gameTime);
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            manager.Get<FlashlightSystem>().BeginDraw(_penumbraComponent);
            manager.Get<RenderSystem>().Render(sb, gameTime); // lowers FPS by half (2000)
            manager.Get<FlashlightSystem>().EndDraw(_penumbraComponent, gameTime);
            manager.Get<TextSystem>().Draw(sb);
            manager.Get<RenderHUDSystem>().Draw(sb); // not noticable
        }

        public void Reset()
        {
            EventBus.Instance.Clear();
            manager.Get<RenderSystem>().ClearCache();
            ComponentManager.Instance.Clear();
        }

        public void AddSystems(params IUpdateables[] systems)
        {
            foreach (var system in systems)
            {
                manager.UpdateableSystems.Add(system.GetType(), system);
            }
        }

        public void AddSystems(params IDrawables[] systems)
        {
            foreach (var system in systems)
            {
                manager.DrawableSystems.Add(system.GetType(), system);
            }
        }

        public T GetSystem<T>() where T : class, ISystem
        {
            return manager.Get<T>();
        }

        private (Dictionary<Type, IUpdateables> updateables, Dictionary<Type, IDrawables> drawables) CreateSystems()
        {
            var flashlightSystem = new FlashlightSystem();

            var drawableSystems = new Dictionary<Type, IDrawables>
            {
                { typeof(RenderSystem), new RenderSystem() },
                { typeof(RenderHUDSystem), new RenderHUDSystem()},
                { typeof(FlashlightSystem), flashlightSystem},
                { typeof(TextSystem), new TextSystem() },
            };

            var updateableSystems = new Dictionary<Type, IUpdateables>
            {
                // Ordered Systems.
                { typeof(EnemyCollisionSystem), new EnemyCollisionSystem() },
                { typeof(InputHandler), new InputHandler() },
                { typeof(GamePadMovementSystem), new GamePadMovementSystem() },
                // { typeof(EventZoneSystem), new EventZoneSystem() }
                { typeof(CameraSceneSystem), new CameraSceneSystem() },
                { typeof(MoveSystem), new MoveSystem() },
                { typeof(AISystem), new AISystem() },
                { typeof(CollisionSystem), new CollisionSystem() },
                { typeof(AnimationSystem), new AnimationSystem() },
                { typeof(SpriteAnimationSystem), new SpriteAnimationSystem() },
                { typeof(FlashlightSystem), flashlightSystem},
                { typeof(FlickeringLightSystem), new FlickeringLightSystem() },
                { typeof(HealthSystem), new HealthSystem() },
                { typeof(InertiaDampenerSystem), new InertiaDampenerSystem() },
                { typeof(BackwardsPenaltySystem), new BackwardsPenaltySystem() },
                { typeof(ScoreSystem), new ScoreSystem() },
                { typeof(EntityRemovalSystem), new EntityRemovalSystem() },

                // Subsystems or other
                // TODO: check if the should remain systems or should be included in others..
                { typeof(LoadContentSystem), new LoadContentSystem() },
                { typeof(TankMovementSystem), new TankMovementSystem() },
                { typeof(CollisionResolveSystem), new CollisionResolveSystem() },
                { typeof(WallCollisionSystem), new WallCollisionSystem() },
                { typeof(BulletCollisionSystem), new BulletCollisionSystem() },
                { typeof(LightAbilitySystem), new LightAbilitySystem() },
                { typeof(PickupCollisionSystem), new PickupCollisionSystem() },
                { typeof(SprintAbilitySystem), new SprintAbilitySystem() },
                { typeof(QuickTurnAbilitySystem), new QuickTurnAbilitySystem() },

                { typeof(ReloadSystem), new ReloadSystem() },
                { typeof(HighScoreSystem), new HighScoreSystem() },
                { typeof(AiWallCollisionSystem), new AiWallCollisionSystem() },
                { typeof(KillSwitchEventFactory), new KillSwitchEventFactory() },
                { typeof(KillSwitchSystem), new KillSwitchSystem() },
                { typeof(PickupSpawnSystem), new PickupSpawnSystem() },
            };

            return (updateableSystems, drawableSystems);
        }
    }
}
