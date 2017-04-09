using System;
using System.Collections.Generic;
using System.Linq;
using ZEngine.Components;

namespace ZEngine.Managers
{
    public class ComponentManager
    {
        // _____________________________________________________________________________________________________________________ //
        
        // Singleton pattern is used here for the ComponentManager,
        // Instance is used heer to get that only instance.
        public static ComponentManager Instance => LazyInitializer.Value;
        private static readonly Lazy<ComponentManager> LazyInitializer = new Lazy<ComponentManager>(() => new ComponentManager());

        // Dictionary with all the components that should exist, and
        // an nested dictionary with all the entities associated with
        // that component as the key, and component instance as value.
        private Dictionary<Type, Dictionary<int, IComponent>> _components = new Dictionary<Type, Dictionary<int, IComponent>>();

        // _____________________________________________________________________________________________________________________ //


        // Returns a dictionary with all the entities that have an instance 
        // of the component type that is given as a parameter.        
        public Dictionary<int, IComponent> GetEntitiesWithComponent(Type componentType)
        {
            if (!_components.ContainsKey(componentType))
            {
                throw new Exception("No such component.");
            }
            else
            {
                return _components[componentType];
            }
        }

        // Overloaded method that is a generic method. It takes a type parameter
        // and returns a dictionary with all the entities that are associated 
        // with an instance of that component that was given as type parameter.
        public Dictionary<int, T> GetEntitiesWithComponent<T>()
        {
            if (!_components.ContainsKey(typeof(T)))
            {
                return new Dictionary<int, T>();
            }
            else
            {
                return _components[typeof(T)] as Dictionary<int, T>;
            }
        }


        public T CreateComponent<T>() where T : new()
        {
            if (ContainsComponent<T>())
            {
                return new T();
            }
            else
            {
                throw new Exception("No such component exist.");
            }
        }

        private ISystem NewComponent(Type name)
        {
            return (ISystem)Activator.CreateInstance(_components[name].GetType());
        }


        private Boolean ContainsComponent<T>()
        {
            return _components.Count(entry => entry.Value.GetType() == typeof(T)) == 1;
        }

        /* This method is used to associate an instance of a component to a specified
         * entity. it takes the instance of the component and the key: entityId.
         */
        public void AddComponentToEntity(IComponent component, int entityId)
        {
            var entityComponents = _components[component.GetType()];
            entityComponents.Add(entityId, component);
        }

        // Completely deletes the entity from all components.
        public void DeleteEntity(int entityId)
        {
            foreach (var component in _components.Keys)
            {
                _components[component].Remove(entityId);
            }
        }

        // Deletes the entity from a given component's dictionary.
        public void RemoveComponentFromEntity(Type component, int entityId)
        {
            var entityComponents = _components[component];
            entityComponents.Remove(entityId);

        }
    }
}
