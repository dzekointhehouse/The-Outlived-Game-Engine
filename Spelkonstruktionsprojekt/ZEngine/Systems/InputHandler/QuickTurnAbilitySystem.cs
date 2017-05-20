using System;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.Managers;

namespace ZEngine.Systems.InputHandler
{
    public class QuickTurnAbilitySystem : ISystem
    {
        private readonly EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        private const string TurnAroundEventName = EventConstants.TurnAround;

        public void Start()
        {
            EventBus.Subscribe<InputEvent>(TurnAroundEventName, HandleTurnAroundEvent);
        }

        private void HandleTurnAroundEvent(InputEvent moveEvent)
        {
            if (moveEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) return;
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(moveEvent.EntityId);
            if (moveComponent == null) return;

            var animationComponent = GetOrCreateDefault(moveEvent.EntityId);
            var animation = new GeneralAnimation
            {
                AnimationType = TurnAroundEventName,
                StartOfAnimation = moveEvent.EventTime,
                Unique = true,
                Length = 220
            };
            NewTurningAnimation(moveEvent.EntityId, animation, moveComponent);
            animationComponent.Animations.Add(animation);
        }

        public void NewTurningAnimation(uint entityId, GeneralAnimation generalAnimation, MoveComponent moveComponent)
        {
            double start = moveComponent.Direction;
            double target = start + Math.PI;
            generalAnimation.Animation = delegate(double currentTime)
            {
                var elapsedTime = currentTime - generalAnimation.StartOfAnimation;
                if (elapsedTime > generalAnimation.Length) generalAnimation.IsDone = true;

                //Algorithm for turning stepwise on each iteration
                //Modulus is for when the direction makes a whole turn
                moveComponent.Direction = (float)
                    ((start + (target - start) / generalAnimation.Length * elapsedTime) % MathHelper.TwoPi);
            };
        }

        private AnimationComponent GetOrCreateDefault(uint entityId)
        {
            var animationComponent = ComponentManager.GetEntityComponentOrDefault<AnimationComponent>(entityId);
            if (animationComponent != null) return animationComponent;

            animationComponent = ComponentManager.ComponentFactory.NewComponent<AnimationComponent>();
            ComponentManager.AddComponentToEntity(animationComponent, entityId);
            return animationComponent;
        }
    }
}