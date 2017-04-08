using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ZombieGame;

namespace Spelkonstruktionsprojekt
{
    class EntityManager
    {
        private static EntityManager _entityManager;
        private int _nextEntityId;

        // The idea here is to have the entity 
        // entityId as a key here and add 
        public Dictionary<int, Dictionary<string, IComponent>> ExistingEntities { get; set; }


        private EntityManager()
        {
            _nextEntityId = 0;
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
        public int GetNewEntity()
        {
            return _nextEntityId++;
        }

        public void AddComponent(int entityId, IComponent component)
        {

            var innerDictionary = ExistingEntities[entityId];
            innerDictionary.Add(component.GetComponentName, component);

        }

        public void RemoveComponent(int entityId, IComponent component)
        {
            var innerDictionary = ExistingEntities[entityId];
            innerDictionary.Remove(component.GetComponentName);
        }
        public void RemoveComponent(int entityId, string component)
        {
            var innerDictionary = ExistingEntities[entityId];
            innerDictionary.Remove(component);
        }

        public IComponent GetEntityComponent(int entityId, string componentName)
        {
            var innerDictionary = ExistingEntities[entityId];
            return innerDictionary[componentName];
        }



    }


}
