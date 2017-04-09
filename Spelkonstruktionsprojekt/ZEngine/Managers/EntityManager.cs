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
        private List<int> _existingEntities;
   
        private EntityManager()
        {
            _nextEntityId = 0;
            _existingEntities = new List<int>();

        }

        // Singelton pattern, we will only have one instance
        // of our entity manager.
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
        // Add new entity id to the list of entites and return it
        // while generating a new id for the next call.
        public int NewEntity()
        {
            _existingEntities.Add(_nextEntityId);
            return _nextEntityId++;
        }

        // Completely deletes the entity and all components
        // that are associated with it, thats why it needs to
        // use the component manager instance.
        public void DeleteEntity(int entityId)
        {
            _existingEntities.Remove(entityId);
            ComponentManager.Instance.DeleteEntity(entityId);
        }

        // Returns the complete list with all of the existing 
        // Entities, that have been created.
        public List<int> GetListWithEntities()
        {
            return _existingEntities;
        }


    }


}
