using System;
using System.Collections.Generic;
using System.Linq;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Systems;

namespace ZEngine.Managers
{
    public class SystemManager
    {
        // _____________________________________________________________________________________________________________________ //

        // Singleton pattern strikes again! we contain an instance
        // of SystemManager which we can access by using Instance.
        public static SystemManager Instance => LazyInitializer.Value;
        private static readonly Lazy<SystemManager> LazyInitializer = new Lazy<SystemManager>(() => new SystemManager());

        // We have an dictionary with
        // the dictionary name as the key, and the system instance as value.
        private readonly Dictionary<Type, ISystem> _systems = new Dictionary<Type, ISystem>()
        {
            { typeof(RenderSystem), new RenderSystem() },
            { typeof(LoadContentSystem), new LoadContentSystem() },
            { typeof(InputHandler), new InputHandler() },
            { typeof(MoveSystem), new MoveSystem() },
            { typeof(TankMovementSystem), new TankMovementSystem() },
            { typeof(TitlesafeRenderSystem), new TitlesafeRenderSystem()},
            { typeof(LightSystem), new LightSystem()},
            { typeof(CollisionSystem), new CollisionSystem() }
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
            else
            {
                throw new Exception("No such system exist.");
            }
        }

        // Checks the systems dictionary if it contains the 
        // system that is specified as (T) type parameter when
        // this method is called, No big deal.
        private Boolean ContainsSystem<T>()
        {
            return _systems.Count(entry => entry.Value.GetType() == typeof(T)) == 1;
        }
    }
}