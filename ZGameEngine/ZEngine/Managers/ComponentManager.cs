using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Managers
{
    public class ComponentManager : IComponentManager
    {
        // _____________________________________________________________________________________________________________________ //
        
        // Singleton pattern is used here for the ComponentManager,
        // Instance is used heer to get that only instance.
        public static ComponentManager Instance => LazyInitializer.Value;
        private static readonly Lazy<ComponentManager> LazyInitializer = new Lazy<ComponentManager>(() => new ComponentManager());

        // Dictionary with all the components that should exist, and
        // an nested dictionary with all the entities associated with
        // that component as the key, and component instance as value.
        private Dictionary<Type, Dictionary<uint, IComponent>> _components = new Dictionary<Type, Dictionary<uint, IComponent>>();
        //second dictionary to get all components in one entity
        // _____________________________________________________________________________________________________________________ //

        //Used to create and recycle components
        //Most recycling is done automatically by the ComponentManager
        public ComponentFactory ComponentFactory { get; } = new ComponentFactory();

        // Returns a dictionary with all the entities that have an instance 
        // of the component type that is given as a parameter.        
        public Dictionary<uint, IComponent> GetEntitiesWithComponent(Type componentType)
        {
            if (!_components.ContainsKey(componentType))
            {
                return new Dictionary<uint, IComponent>();
            }
            return _components[componentType];
        }

        // This method returns true if the entity has an association
        // with the specified component. The component type is given as type parameter
        // and that type has to implement the IComponent interface.
        public bool EntityHasComponent<TComponentType>(uint entityId) where TComponentType : IComponent
        {
            var entityComponents = GetEntitiesWithComponent(typeof(TComponentType));
            return entityComponents.ContainsKey(entityId);
        }
  
        public bool EntityHasComponent(Type componentType, uint entityId)
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

        public TComponentType GetEntityComponentOrDefault<TComponentType>(uint entityId) where TComponentType : IComponent
        {
            var entityComponents = GetEntitiesWithComponent(typeof(TComponentType));
            if (!entityComponents.ContainsKey(entityId)) return default(TComponentType);
            return (TComponentType) entityComponents[entityId];
        }

        public IComponent GetEntityComponentOrDefault(Type componentType, uint entityId)
        {
            //Compacted for efficiency
            Dictionary<uint, IComponent> entityComponents;
            if (!_components.TryGetValue(componentType, out entityComponents)) return null; //Checks if component type exists
            IComponent component;
            if (!entityComponents.TryGetValue(entityId, out component)) return null; //Checks if entity has component
            return component; //returns component if it was found
        }

        public bool ContainsAllComponents(uint entityId, List<Type> componentTypes)
        {
            var containsAll = true;
            foreach (var componentType in componentTypes)
            {
                if (!_components.ContainsKey(componentType)
                    || !_components[componentType].ContainsKey(entityId))
                {
                    containsAll = false;
                }
            }
            return containsAll;
        }

        //public void AddComponentToEntity<TComponentType>(int entityId) where TComponentType : IComponent, new()
        //{
        //    AddComponentKeyIfNotPresent(typeof(TComponentType));

        //    var entityComponents = _components[typeof(TComponentType)];
        //    entityComponents.Add(entityId, (IComponent) new TComponentType());
        //}

        // This method is used to associate an instance of a component to a specified
        // entity. it takes the instance of the component and the key: entityId.
        public void AddComponentToEntity(IComponent componentInstance, uint entityId)
        {
            AddComponentKeyIfNotPresent(componentInstance.GetType());

            var entityComponents = _components[componentInstance.GetType()];
            entityComponents.Add(entityId, componentInstance);
        }

        // Completely deletes the entity key and all the usages of it
        // in other places (componentManager) where it is associated with
        // instances of other components. We loop through all component types,
        // in the component manager dictionary.
        public void DeleteEntity(uint entityId)
        {
            foreach (var component in _components.Keys)
            {
                if (_components[component].ContainsKey(entityId))
                {
                    ComponentFactory.Recycle(component, _components[component][entityId]);
                    _components[component].Remove(entityId);
                }
            }
        }

        public void Clear()
        {
            foreach (var type in _components)
            {
                type.Value.Clear();
            }
            _components.Clear();
            _components = new Dictionary<Type, Dictionary<uint, IComponent>>();
            ComponentFactory.Reset();
        }

        // Deletes the entity from a given component's dictionary.
        public void RemoveComponentFromEntity<T>(uint entityId)
        {
            if (!_components.ContainsKey(typeof(T))) return;
            var entityComponents = _components[typeof(T)];
            if (entityComponents == null) return;
            if (!entityComponents.ContainsKey(entityId)) return;
            ComponentFactory.Recycle(entityComponents[entityId]);
            entityComponents.Remove(entityId);
        }

        // This method adds the component type as the key to the dictionary that 
        // consists of those componenttype keys, and an inner dictionary that contains
        // the entityId as the key and the instance of the component as value.
        public void AddComponentKeyIfNotPresent(Type componentType)
        {
            if (!_components.ContainsKey(componentType))
            {
                _components[componentType] = new Dictionary<uint, IComponent>();
            }
        }
        
        public void Debug_ListComponentsForEntity(uint entityId)
        {
            Debug.WriteLine("Components for entity " + entityId);
            foreach (var component in _components)
            {
                if (component.Value.ContainsKey(entityId))
                {
                    Debug.WriteLine("\t" + component.Key.Name);
                }
            }
            Debug.WriteLine("");
        }
    }
}
