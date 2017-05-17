using System;
using System.Collections.Generic;
using System.Linq;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using Spelkonstruktionsprojekt.ZEngine.Systems.Motion;
using Spelkonstruktionsprojekt.ZEngine.Systems.Rendering;
using ZEngine.Systems;
using ZEngine.Systems.Collisions;
using ZEngine.Systems.InputHandler;

namespace ZEngine.Managers
{
    public class SystemManager
    {
        // _____________________________________________________________________________________________________________________ //

        // Singleton pattern strikes again! we contain an instance
        // of SystemManager which we can access by using Instance.
        public static SystemManager Instance => LazyInitializer.Value;
        private static readonly Lazy<SystemManager> LazyInitializer = new Lazy<SystemManager>(() => new SystemManager());

        // We immediately put instances of systems in this
        // dictionary, now they can easily be accessed with it's
        // type used as the key.
        private readonly Dictionary<Type, ISystem> _systems = new Dictionary<Type, ISystem>
        {
            { typeof(RenderSystem), new RenderSystem() },
            { typeof(LoadContentSystem), new LoadContentSystem() },
            { typeof(InputHandler), new InputHandler() },
            { typeof(MoveSystem), new MoveSystem() },
            { typeof(TankMovementSystem), new TankMovementSystem() },
            { typeof(RenderHUDSystem), new RenderHUDSystem()},
            { typeof(FlashlightSystem), new FlashlightSystem()},
            { typeof(CollisionSystem), new CollisionSystem() },
            { typeof(CollisionResolveSystem), new CollisionResolveSystem() },
            { typeof(WallCollisionSystem), new WallCollisionSystem() },
            { typeof(EnemyCollisionSystem), new EnemyCollisionSystem() },
            { typeof(CameraSceneSystem), new CameraSceneSystem() },
            { typeof(AISystem), new AISystem() },
            { typeof(AnimationSystem), new AnimationSystem() },
            { typeof(SoundSystem), new SoundSystem() },
            { typeof(WeaponSystem), new WeaponSystem() },
            { typeof(BulletCollisionSystem), new BulletCollisionSystem() },
            { typeof(HealthSystem), new HealthSystem() },
            { typeof(VideoPlayerSystem), new VideoPlayerSystem() },
            { typeof(SpriteAnimationSystem), new SpriteAnimationSystem() },
            { typeof(EntityRemovalSystem), new EntityRemovalSystem() },
            { typeof(BackwardsPenaltySystem), new BackwardsPenaltySystem() },
            { typeof(InertiaDampenerSystem), new InertiaDampenerSystem() },
            { typeof(LightAbilitySystem), new LightAbilitySystem() },
            { typeof(PickupCollisionSystem), new PickupCollisionSystem() },
            { typeof(SprintAbilitySystem), new SprintAbilitySystem() },
            { typeof(QuickTurnAbilitySystem), new QuickTurnAbilitySystem() },
            { typeof(GamePadMovementSystem), new GamePadMovementSystem() },
            { typeof(ScoreSystem), new ScoreSystem() },
            { typeof(ReloadSystem), new ReloadSystem() }

        };

        // _____________________________________________________________________________________________________________________ //

        // Gets the instance of system that is specified
        // as a type parameter. The method checks by using ContainsSystem
        // if the systemtype is a valid system and then returns the 
        // instance from the dictionary above. https://msdn.microsoft.com/en-us/library/d5x73970.aspx  
        public T GetSystem<T>() where T : class, ISystem
        {
            if (ContainsSystem<T>())
            {
                return _systems[typeof(T)] as T;
            }
            throw new Exception("No such system exist.");
        }

        public ISystem GetSystem(Type systemType)
        {
            if (_systems.ContainsKey(systemType))
            {
                return _systems[systemType];
            }
            throw new Exception("No such system exist.");
        }

        // Checks the systems dictionary if it contains the 
        // system that is specified as (T) type parameter when
        // this method is called, No big deal.
        private bool ContainsSystem<T>()
        {
            return _systems.Count(system => system.Value.GetType() == typeof(T)) == 1;
        }

        public T Get<T>() where T : class, ISystem
        {
            return SystemManager.Instance.GetSystem(typeof(T)) as T;
        }
    }
}