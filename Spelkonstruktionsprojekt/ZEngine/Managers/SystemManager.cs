using System;
using System.Collections.Generic;
using System.Linq;
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

        // And of course as August likes it, we have an dictionary with
        // the dictionary name as the key, and the system instance as value.
        private readonly Dictionary<string, ISystem> _systems = new Dictionary<string, ISystem>()
        {
            { "Render", new RenderSystem() },
            { "LoadContent", new LoadContentSystem() }
        };

        // _____________________________________________________________________________________________________________________ //

        // This is the same as the CreateSystem method below,
        // except it uses oldschool syntax.
        public ISystem CreateSystem(string systemName)
        {
            if (!_systems.ContainsKey(systemName))
            {
                throw new Exception("No such system.");
            }
            else
            {
                return NewSystem(systemName);
            }
        }

        // Creates an instance of a new system that is specified
        // as a type parameter. The method check by using ContainsSystem
        // if the systemtype is a valid system and then returns an 
        // instance. https://msdn.microsoft.com/en-us/library/d5x73970.aspx  
        public T CreateSystem<T>() where T : new()
        {
            if (ContainsSystem<T>())
            {
                return new T();
            }
            else
            {
                throw new Exception("No such system exist.");
            }
        }

        private ISystem NewSystem(string name)
        {
            return (ISystem) Activator.CreateInstance(_systems[name].GetType());
        }

        // Checks the systems dictionary if it contains the 
        // system specified as type parameter. No big deal.
        private Boolean ContainsSystem<T>()
        {
            return _systems.Count(entry => entry.Value.GetType() == typeof(T)) == 1;
        }
    }
}