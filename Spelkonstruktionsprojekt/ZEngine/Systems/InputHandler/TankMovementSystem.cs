using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class TankMovementSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        
        public ISystem Start()
        {
            return this;
        }


        public void StartSystem(RenderDependencies gd)
        {

        }

        public ISystem Stop()
        {
            return this;
        }

        
    }
}
