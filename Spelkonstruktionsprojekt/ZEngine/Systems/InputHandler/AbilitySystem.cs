using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

        private string TurnAroundEventName = "entityTurnAround";
        private string RunEventName = "entityRun";

        //Temp thing for demo
        private float VelocityBonus = 400;
        private float PreviousMaxVelocity;

        public void Start()
        {
            EventBus.Subscribe<InputEvent>(TurnAroundEventName, HandleTurnAroundEvent);
            EventBus.Subscribe<InputEvent>(RunEventName, HandleRunEvent);
        }

        private void HandleTurnAroundEvent(InputEvent moveEvent)
        {
            if (moveEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) return;

            var entityId = moveEvent.EntityId;
            if (ComponentManager.EntityHasComponent<MoveComponent>(entityId))
            {
                var animationComponent = ComponentManager.GetEntityComponentOrDefault<AnimationComponent>(entityId);
                if (animationComponent == null)
                {
                    animationComponent = new AnimationComponent();
                    ComponentManager.AddComponentToEntity(animationComponent, entityId);
                }

                var animation = new GeneralAnimation()
                {
                    AnimationType = RunEventName,
                    StartOfAnimation = moveEvent.EventTime,
                    Unique = true,
                    Length = 220
                };
                var animationAction = NewTurningAnimation(entityId, animation);
                animation.Animation = animationAction;

                animationComponent.Animations.Add(animation);
            }
        }

        public Action<double> NewTurningAnimation(int entityId, GeneralAnimation generalAnimation)
        {
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entityId);
            if (moveComponent == null) return null;

            double start = moveComponent.Direction;
            double target = (start + Math.PI);
            return delegate(double currentTime)
            {
                var elapsedTime = currentTime - generalAnimation.StartOfAnimation;
                if (elapsedTime > generalAnimation.Length) generalAnimation.IsDone = true;

                //Algorithm for turning stepwise on each iteration
                //Modulus is for when the direction makes a whole turn
                moveComponent.Direction = (float) ((start + (target - start) / generalAnimation.Length * elapsedTime) %
                                                   MathHelper.TwoPi);
            };
        }

        public void HandleRunEvent(InputEvent moveEvent)
        {
            if (moveEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) return;

            var entityId = moveEvent.EntityId;
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entityId);
            if (moveComponent == null) return;
            var animationComponent = ComponentManager.GetEntityComponentOrDefault<AnimationComponent>(entityId);
            if (animationComponent == null)
            {
                animationComponent = new AnimationComponent();
                ComponentManager.AddComponentToEntity(animationComponent, entityId);
            }

            var animation = new GeneralAnimation()
            {
                AnimationType = TurnAroundEventName,
                StartOfAnimation = moveEvent.EventTime,
                Unique = true,
                Length = 8000
            };
            var animationAction = NewRunAnimation(animation, moveComponent, 2000);
            animation.Animation = animationAction;

            animationComponent.Animations.Add(animation);
            PreviousMaxVelocity = moveComponent.MaxVelocitySpeed;
            moveComponent.MaxVelocitySpeed = VelocityBonus;
            moveComponent.Speed = moveComponent.MaxVelocitySpeed;
        }

        public Action<double> NewRunAnimation(GeneralAnimation generalAnimation, MoveComponent moveComponent, int sprintTime)
        {
            Debug.WriteLine("RUN FORREST RUN");

            return delegate(double currentTime)
            {
                var elapsedTime = currentTime - generalAnimation.StartOfAnimation;
                if (elapsedTime > generalAnimation.Length)
                {
                    Debug.WriteLine("Animation done.");
                    generalAnimation.IsDone = true;
                    return;
                }
                if (elapsedTime > sprintTime && moveComponent.MaxVelocitySpeed == VelocityBonus)
                {
                    Debug.WriteLine("Sprint done.");
                    moveComponent.MaxVelocitySpeed = PreviousMaxVelocity;
                    moveComponent.Speed = (float) (moveComponent.MaxVelocitySpeed * 0.2);
                }
            };
        }
    }
}