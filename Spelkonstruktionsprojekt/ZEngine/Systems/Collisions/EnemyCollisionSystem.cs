using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace ZEngine.Systems.Collisions
{
    class EnemyCollisionSystem : ISystem
    {
        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;
        public GameTime GameTime { get; set; }

        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.EnemyCollision, Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
//            Debug.WriteLine("Added damage");
            var healthComponent = ComponentManager.GetEntityComponentOrDefault<HealthComponent>(collisionEvent.Entity);
            if (healthComponent != null)
            {
                healthComponent.Damage.Add(1000);
            }
//            Debug.WriteLine("Handle enemy collision");
//            GameEnder.Score = GameTime.TotalGameTime.TotalSeconds;
        }
    }
}

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    public class TempGameEnder
    {
        public double Score = 0;
    }
}