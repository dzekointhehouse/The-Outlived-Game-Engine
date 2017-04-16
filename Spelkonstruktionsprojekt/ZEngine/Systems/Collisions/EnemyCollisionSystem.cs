using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using ZEngine.Components;
using ZEngine.Managers;

namespace ZEngine.Systems.Collisions
{
    class EnemyCollisionSystem : ISystem
    {
        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        private TempGameEnder GameEnder;
        public GameTime GameTime { get; set; }

        public void Start(TempGameEnder gameEnder)
        {
            GameEnder = gameEnder;
            EventBus.Subscribe<SpecificCollisionEvent>("EnemyCollision", Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            Debug.WriteLine("Handle enemy collision");
            GameEnder.Score = GameTime.TotalGameTime.TotalSeconds;
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
