using System;
using System.Collections.Generic;
using System.Linq;
using ZEngine.Components;

namespace ZEngine.Managers
{
    public class ComponentManager
    {
        private Dictionary<Type, Dictionary<int, IComponent>> _components = new Dictionary<Type, Dictionary<int, IComponent>>();


        /*
         * Returns a dictionary with all the entities that has an instance of the given component type
         */
        public Dictionary<int, IComponent> GetEntitiesWithThisComponent(Type componentType)
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

        /*
         * Completely deletes the entity from all components.
         */
        public void DeleteEntity(int entityId)
        {
            foreach (var component in _components.Keys)
            {
                _components[component].Remove(entityId);
            }
        }
        /*
         * Deletes the entity from a given component's dictionary.
         * 
         */
        public void RemoveComponentFromEntity(Type component, int entityId)
        {
            var entityComponents = _components[component];
            entityComponents.Remove(entityId);

        }
    }
}
