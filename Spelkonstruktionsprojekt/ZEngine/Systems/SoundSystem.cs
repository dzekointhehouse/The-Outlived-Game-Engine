using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class SoundSystem
    {
        private EventBus EventBus = EventBus.Instance;
        public void Update()
        {
            EventBus.Subscribe<MoveEvent>("entityWalkForwards", WalkingSounds);


        }

        public void WalkingSounds(MoveEvent moveEvent)
        {
            if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var entities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(SoundComponent));

            }
        }

    }
}
