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
                Debug.WriteLine("INITIATING ANIMATION");
                var lengthInSeconds = 1;
                var animation = ComponentManager.GetEntityComponentOrDefault<AnimationComponent>(entityId);
                if (animation == null)
                {
                    animation = new AnimationComponent();
                    ComponentManager.AddComponentToEntity(animation, entityId);
                }
                animation.Animations.Add(NewTurningAnimation(moveEvent.CurrentTimeMilliseconds, lengthInSeconds,
                    entityId));
            }
        }

        public Func<double, bool> NewTurningAnimation(double startOfAnimation, double length,  int entityId)
        {
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entityId);
            if (moveComponent == null) return null;

            double start = moveComponent.Direction;
            double target = (start + Math.PI);
            return delegate(double currentTime)
            {
                var elapsedTime = currentTime - startOfAnimation;
                moveComponent.Direction = (start + (target - start) / length * elapsedTime) % MathHelper.TwoPi;
                Debug.WriteLine("Start " + start + ", Target " + target + ", elapsedTime " + elapsedTime +
                                ", currentTime " + currentTime);

                var targetToleranceSpan = 0.01;
                return moveComponent.Direction >= target - targetToleranceSpan;
            };
        }
    }
}
