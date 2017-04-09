using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Managers;
using ZEngine.EventBus;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class LoadContent : ISystem
    {
        private EventBus EventBus = EventBus.Instance;

        public void Start()
        {
            EventBus.Subscribe("LoadContent", Load);
        }

        public void Stop()
        {
            EventBus.Unsubscribe("LoadContent", Load);
        }

        public void Load()
        {
        }
    }
}
