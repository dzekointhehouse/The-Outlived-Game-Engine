using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ZEngine.Components;
using ZombieGame;

namespace ZEngine.Managers
{
    public class EntityManager
    {
        private static EntityManager _entityManager;
        private int _nextEntityId;

        // The idea here is to have the entity 
        // entityId as a key here and add 
        private Dictionary<int, Dictionary<string, IComponent>> ExistingEntities;

        private EntityManager()
        {
            _nextEntityId = 0;
            ExistingEntities = new Dictionary<int, Dictionary<string, IComponent>>();
        }

        public static EntityManager GetEntityManager()
        {
            if (_entityManager != null)
            {
                return _entityManager;
            }
            else
            {
                _entityManager = new EntityManager();
                return _entityManager;
            }
        }

        // This method generates a unique entity that can be used
        // to build a gameobject with components.
        public int NewEntity()
        {
            return _nextEntityId++;
        }
        /*
         * This method takes an id and a component, and adds the component
         * to the entity of that id.
         */
        public void AddComponent(int entityId, IComponent component)
        {

            var entityComponents = ExistingEntities[entityId];
            entityComponents.Add(component.GetComponentName, component);

        }
        /*
         * This method takes a component and deletes it from 
         * the entity with the corresponding id.
         */
        public void RemoveComponent(int entityId, IComponent component)
        {
            RemoveComponent(entityId, component.GetComponentName);
        }


        /* Overloaded RemoveComponent where you only need to know the component
         * name you want to remove, to remove it.
         */
        public void RemoveComponent(int entityId, string component)
        {
            var entityComponents = ExistingEntities[entityId];
            entityComponents.Remove(component);
        }


        /* This method Returns one specific component
         * that is specified by component name.
         */
        public IComponent GetEntityComponent(int entityId, string componentName)
        {
            var entityComponents = ExistingEntities[entityId];
            return entityComponents[componentName];
        }

        /* Returns a dictionary with all the components for the entity.
         */
        public Dictionary<string,IComponent> GetEntityComponents(int entityId)
        {
            return ExistingEntities[entityId];
        }

        /* To my dear friend fucking August.
         */
        public Dictionary<int, Dictionary<string, IComponent>> GetEntities()
        {
            return ExistingEntities;
        }

    
    }


}
