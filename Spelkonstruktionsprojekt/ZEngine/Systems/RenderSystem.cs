using System;
using Spelkonstruktionsprojekt;
using ZEngine.Managers;

namespace ZEngine.Systems
{
    public class RenderSystem : ISystem
    {
        public static string SystemName = "Render";
        private EntityManager EntityManager { get; set; }
        public RenderSystem(EntityManager entityManager)
        {
            EntityManager = entityManager;
        }

        public void Start()
        {
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }

}
