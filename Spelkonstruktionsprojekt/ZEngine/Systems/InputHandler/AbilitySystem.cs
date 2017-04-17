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

        public void Start()
        {
            EventBus.Subscribe<MoveEvent>(TurnAroundEventName, HandleTurnAroundEvent);
        }

        private void HandleTurnAroundEvent(MoveEvent moveEvent)
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
                    AnimationType = TurnAroundEventName,
                    StartOfAnimation = moveEvent.CurrentTimeMilliseconds,
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
                moveComponent.Direction = (start + (target - start) / generalAnimation.Length * elapsedTime) % MathHelper.TwoPi;
            };
        }
    }

    public class GeneralAnimation
    {
        public bool IsDone = false;

        //Animation takes one parameter <CurrentTimeInMilliseconds>
        public Action<double> Animation { get; set; }

        public bool Loop { get; set; }

        //When animation is unique, no second animation of same AnimationId may run on entity
        public bool Unique { get; set; }

        public string AnimationType { get; set; }

        public double Length { get; set; }

        public double StartOfAnimation { get; set; }
    }

}
