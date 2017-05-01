using System;
using System.Collections.Generic;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Managers
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
        //second dictionary to get all components in one entity
        // _____________________________________________________________________________________________________________________ //

        
        // Returns a dictionary with all the entities that have an instance 
        // of the component type that is given as a parameter.        
        public Dictionary<int, IComponent> GetEntitiesWithComponent(Type componentType)
        {
            if (!_components.ContainsKey(componentType))
            {
                return new Dictionary<int, IComponent>();
            }
            else
            {
                return _components[componentType];
            }
        }

        // This method returns true if the entity has an association
        // with the specified component. The component type is given as type parameter
        // and that type has to implement the IComponent interface.
        public bool EntityHasComponent<TComponentType>(int entityId) where TComponentType : IComponent
        {
            var entityComponents = GetEntitiesWithComponent(typeof(TComponentType));
            return entityComponents.ContainsKey(entityId);
        }
  
        public bool EntityHasComponent(Type componentType, int entityId)
        {
            var entityComponents = GetEntitiesWithComponent(componentType);
            return entityComponents.ContainsKey(entityId);
        }

        // returns the component for the entity
        //public TComponentType GetEntityComponent<TComponentType>(int entityId) where TComponentType : IComponent
        //{
        //    var entityComponents = GetEntitiesWithComponent<TComponentType>();
        //    if (!entityComponents.ContainsKey(entityId)) throw new Exception("No such Entity has component.");
        //    return entityComponents[entityId];
        //}

        public TComponentType GetEntityComponentOrDefault<TComponentType>(int entityId) where TComponentType : IComponent
        {
            var entityComponents = GetEntitiesWithComponent(typeof(TComponentType));
            if (!entityComponents.ContainsKey(entityId)) return default(TComponentType);
            return (TComponentType) entityComponents[entityId];
        }

        public IComponent GetEntityComponentOrDefault(Type componentType, int entityId)
        {
            var entityComponents = GetEntitiesWithComponent(componentType);
            if (!entityComponents.ContainsKey(entityId)) return null;
            return entityComponents[entityId];
        }

        //public void AddComponentToEntity<TComponentType>(int entityId) where TComponentType : IComponent, new()
        //{
        //    AddComponentKeyIfNotPresent(typeof(TComponentType));

        //    var entityComponents = _components[typeof(TComponentType)];
        //    entityComponents.Add(entityId, (IComponent) new TComponentType());
        //}

        // This method is used to associate an instance of a component to a specified
        // entity. it takes the instance of the component and the key: entityId.
        public void AddComponentToEntity(IComponent componentInstance, int entityId)
        {
            AddComponentKeyIfNotPresent(componentInstance.GetType());

            var entityComponents = _components[componentInstance.GetType()];
            entityComponents.Add(entityId, componentInstance);

        }

        // Completely deletes the entity key and all the usages of it
        // in other places (componentManager) where it is associated with
        // instances of other components. We loop through all component types,
        // in the component manager dictionary.
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

        // This method adds the component type as the key to the dictionary that 
        // consists of those componenttype keys, and an inner dictionary that contains
        // the entityId as the key and the instance of the component as value.
        public void AddComponentKeyIfNotPresent(Type componentType)
        {
            if (!_components.ContainsKey(componentType))
            {
                _components[componentType] = new Dictionary<int, IComponent>();
            }
        }
    }
}
