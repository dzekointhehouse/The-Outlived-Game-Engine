using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler
{
    class AbilitySystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void Start()
        {
            EventBus.Subscribe<MoveEvent>("entityTurnAround", Handle);
        }

        private void Handle(MoveEvent moveEvent)
        {
            if (moveEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) return;

            Debug.WriteLine("ABILITY HANDLE");
            var entityId = moveEvent.EntityId;
            if (ComponentManager.EntityHasComponent<MoveComponent>(entityId))
            {
                var component = ComponentManager.GetEntityComponent<MoveComponent>(entityId);
                //component.Direction = (component.Direction + Math.PI) % MathHelper.TwoPi;
                component.RotationMomentum = Math.PI * 0.1;
            }
        }
    }
}
