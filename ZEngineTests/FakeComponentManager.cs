using System;
using System.Collections.Generic;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;

namespace ZEngineTests
{
    public class FakeComponentManager : IComponentManager
    {
        private IComponentManager _componentManagerImplementation;
        
        public ComponentFactory ComponentFactory => _componentManagerImplementation.ComponentFactory;

        public Dictionary<uint, IComponent> GetEntitiesWithComponent(Type componentType)
        {
            return _componentManagerImplementation.GetEntitiesWithComponent(componentType);
        }

        public bool EntityHasComponent<TComponentType>(uint entityId) where TComponentType : IComponent
        {
            return _componentManagerImplementation.EntityHasComponent<TComponentType>(entityId);
        }

        public bool EntityHasComponent(Type componentType, uint entityId)
        {
            return _componentManagerImplementation.EntityHasComponent(componentType, entityId);
        }

        public TComponentType GetEntityComponentOrDefault<TComponentType>(uint entityId) where TComponentType : IComponent
        {
            return _componentManagerImplementation.GetEntityComponentOrDefault<TComponentType>(entityId);
        }

        public IComponent GetEntityComponentOrDefault(Type componentType, uint entityId)
        {
            return _componentManagerImplementation.GetEntityComponentOrDefault(componentType, entityId);
        }

        public bool ContainsAllComponents(uint entityId, List<Type> componentTypes)
        {
            return _componentManagerImplementation.ContainsAllComponents(entityId, componentTypes);
        }

        public void AddComponentToEntity(IComponent componentInstance, uint entityId)
        {
            _componentManagerImplementation.AddComponentToEntity(componentInstance, entityId);
        }

        public void DeleteEntity(uint entityId)
        {
            _componentManagerImplementation.DeleteEntity(entityId);
        }

        public void Clear()
        {
            _componentManagerImplementation.Clear();
        }

        public void RemoveComponentFromEntity<T>(uint entityId)
        {
            _componentManagerImplementation.RemoveComponentFromEntity<T>(entityId);
        }

        public void AddComponentKeyIfNotPresent(Type componentType)
        {
            _componentManagerImplementation.AddComponentKeyIfNotPresent(componentType);
        }

        public void Debug_ListComponentsForEntity(uint entityId)
        {
            _componentManagerImplementation.Debug_ListComponentsForEntity(entityId);
        }
    }
}