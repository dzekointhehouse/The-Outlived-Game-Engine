using System.Collections.Generic;
using ZombieGame;

namespace Spelkonstruktionsprojekt
{
    class EntityManager
    {
        private static EntityManager _entityManager;
        private int _nextEntityId;
        private Dictionary<int, Velocity> ExistingEntities;
        public Dictionary<int, Velocity> Type { get; set; }


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
        public int GetNewEntity()
        {
            return _nextEntityId++;
        }


    }


}
