using System;
using System.Collections.Generic;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Managers
{
    public interface IComponentManager
    {
        ComponentFactory ComponentFactory { get; }
        Dictionary<uint, IComponent> GetEntitiesWithComponent(Type componentType);
        bool EntityHasComponent<TComponentType>(uint entityId) where TComponentType : IComponent;
        bool EntityHasComponent(Type componentType, uint entityId);
        TComponentType GetEntityComponentOrDefault<TComponentType>(uint entityId) where TComponentType : IComponent;
        IComponent GetEntityComponentOrDefault(Type componentType, uint entityId);
        bool ContainsAllComponents(uint entityId, List<Type> componentTypes);
        void AddComponentToEntity(IComponent componentInstance, uint entityId);
        void DeleteEntity(uint entityId);
        void Clear();
        void RemoveComponentFromEntity<T>(uint entityId);
        void AddComponentKeyIfNotPresent(Type componentType);
        void Debug_ListComponentsForEntity(uint entityId);
    }
}