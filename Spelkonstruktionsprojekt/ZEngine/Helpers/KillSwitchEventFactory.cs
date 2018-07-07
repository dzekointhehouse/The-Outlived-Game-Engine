using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    public class KillSwitchEventFactory : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        public void Update(GameTime gt)
        {
        }

        private static EventBus EventBus = EventBus.Instance;
        private static ComponentManager ComponentManager = ComponentManager.Instance;

        public void Start()
        {
            EventBus.Subscribe<InputEvent>(EventConstants.KillAllLights, KillAllLights);
        }

        public void Stop()
        {
        }

        public static void KillAllLights(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var killEvent = new KillSwitchEvent()
                {
                    EventTime = inputEvent.EventTime
                };
                EventBus.Publish(EventConstants.KillLights, killEvent);
            }
        }
    }
}