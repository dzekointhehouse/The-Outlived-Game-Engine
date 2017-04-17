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
                var lengthInSeconds = 1;
                var animation = new AnimationComponent()
                {
                    Animation = NewTurningAnimation(moveEvent.CurrentTimeMilliseconds ,lengthInSeconds, entityId)
                };
                if (!ComponentManager.EntityHasComponent<AnimationComponent>(entityId))
                {
                    ComponentManager.AddComponentToEntity(animation, entityId);
                }
            }
        }

        public Action<double> NewTurningAnimation(int startOfAnimation, double length,  int entityId)
        {
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entityId);
            if (moveComponent == null) return null;

            double start = moveComponent.Direction;
            double target = (start + Math.PI);
            return delegate(double currentTime)
            {
                var elapsedTime = currentTime - startOfAnimation;
                moveComponent.Direction = (start + (target - start) / length * elapsedTime) % MathHelper.TwoPi;
                //Debug.WriteLine("Start " + start + ", Target " + target + ", TotalFrame " + totalFrames +
                //                ", CurrentFrame " + currentFrame);
            };
        }
    }
}
