using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        //second dictionary to get all components in one entity
        private Dictionary<int, Dictionary<Type, IComponent>> _entity = new Dictionary<int, Dictionary<Type, IComponent>>();
        // _____________________________________________________________________________________________________________________ //

         //returns a dictionary with components instance in a entity
        public Dictionary<Type, IComponent> GetComponentsWithEntity(int entity) {
            if (!_entity.ContainsKey(entity))
            {
                return new Dictionary<Type, IComponent>();
            }
            else
            {
                return _entity[entity];
            }

        }

        public void addEntity(int id)
        {
            _entity.Add(id, new Dictionary<Type, IComponent>());
        }
        
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

        // Overloaded method that is a generic method. It takes a type parameter
        // and returns a dictionary with all the entities that are associated 
        // with an instance of that component that was given as type parameter.
        public Dictionary<int, T> GetEntitiesWithComponent<T>() where T : IComponent    
        {
            if (!_components.ContainsKey(typeof(T)))
            {
                return new Dictionary<int, T>();
            }
            else
            {
                return _components[typeof(T)].ToDictionary(
                    entry => entry.Key,
                    entry => (T) entry.Value
                );
            }
        }

        // This method returns true if the entity has an association
        // with the specified component. The component type is given as type parameter
        // and that type has to implement the IComponent interface.
        public bool EntityHasComponent<T>(int entityId) where T : IComponent
        {
            var entityComponents = this.GetEntitiesWithComponent<T>();
            return entityComponents.ContainsKey(entityId);
        }

        public ComponentType GetEntityComponent<ComponentType>(int entityId) where ComponentType : IComponent
        {
            var entityComponents = this.GetEntitiesWithComponent<ComponentType>();
            if (!entityComponents.ContainsKey(entityId)) throw new Exception("No such Entity has component.");
            return entityComponents[entityId];
        }

        // Creates an returns an instance of an empty component
        // that is specified in the type parameter. This metod also checks
        // if the dictionary contains that metod, otherwise we cannot create it.
        // The component type you want to create also needs to implement IComponent.
        public T CreateComponent<T>() where T : IComponent, new()
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

        // I don't know what this does, this shit wasn't me.
        private ISystem NewComponent(Type name)
        {
            return (ISystem)Activator.CreateInstance(_components[name].GetType());
        }

        // The only thing done here is that this method checks
        // if the dictionary contains the componenttype specified
        // in the type parameter.
        private Boolean ContainsComponent<T>()
        {
            return _components.Count(entry => entry.Value.GetType() == typeof(T)) == 1;
        }

        public void AddComponentToEntity<T>(int entityId) where T : new()
        {
            AddComponentKeyIfNotPresent(typeof(T));

            var entityComponents = _components[typeof(T)];
            entityComponents.Add(entityId, (IComponent) new T());

            var entityComponents2 = _entity[entityId];
            entityComponents2.Add(typeof(T), (IComponent)new T());
            
        }

        // This method is used to associate an instance of a component to a specified
        // entity. it takes the instance of the component and the key: entityId.
        public void AddComponentToEntity(IComponent componentInstance, int entityId)
        {
            AddComponentKeyIfNotPresent(componentInstance.GetType());

            var entityComponents = _components[componentInstance.GetType()];
            entityComponents.Add(entityId, componentInstance);

            var entityComponents2 = _entity[entityId];
            if(entityComponents2 == null)
            {
                entityComponents2 = new Dictionary<Type, IComponent>();
            }
            entityComponents2.Add(componentInstance.GetType(), componentInstance);

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
