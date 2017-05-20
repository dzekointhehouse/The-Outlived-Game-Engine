using System;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler
{
    public class SprintAbilitySystem : ISystem
    {

        private readonly EventBus EventBus = EventBus.Instance;
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        private const string SprintEventName = EventConstants.Running;
        private const int VelocityBonus = 400;

        public void Start()
        {
            EventBus.Subscribe<InputEvent>(SprintEventName, HandleRunEvent);
        }

        public void HandleRunEvent(InputEvent moveEvent)
        {
            if (moveEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) return;
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(moveEvent.EntityId);
            if (moveComponent == null) return;

            var entityId = moveEvent.EntityId;
            var animationComponent = GetOrCreateDefault(entityId);

            var animation = new GeneralAnimation
            {
                AnimationType = SprintEventName,
                StartOfAnimation = moveEvent.EventTime,
                Unique = true,
                Length = 8000
            };
            AddNewSprintAnimation(animation, moveComponent, 2000);
            SetNewSprintMaxVelocity(moveComponent, VelocityBonus);
            animationComponent.Animations.Add(animation);
        }

        private void AddNewSprintAnimation(GeneralAnimation generalAnimation, MoveComponent moveComponent, int sprintTime)
        {
            var previousMaxVelocity = moveComponent.MaxVelocitySpeed;
            var sprintIsDone = false;

            generalAnimation.Animation = delegate(double currentTime)
            {
                var elapsedTime = currentTime - generalAnimation.StartOfAnimation;
                if (elapsedTime > generalAnimation.Length)
                {
                    generalAnimation.IsDone = true;
                    return;
                }
                if (elapsedTime > sprintTime && !sprintIsDone)
                {
                    sprintIsDone = true;
                    moveComponent.MaxVelocitySpeed = previousMaxVelocity;
                    moveComponent.Speed = (float) (moveComponent.MaxVelocitySpeed * 0.2);
                }
            };
        }

        private void SetNewSprintMaxVelocity(MoveComponent moveComponent, int velocityBonus)
        {
            moveComponent.MaxVelocitySpeed = velocityBonus;
//            moveComponent.Speed = moveComponent.MaxVelocitySpeed;
        }

        private AnimationComponent GetOrCreateDefault(uint entityId)
        {
            var animationComponent = ComponentManager.GetEntityComponentOrDefault<AnimationComponent>(entityId);
            if (animationComponent == null)
            {
                animationComponent = ComponentManager.ComponentFactory.NewComponent<AnimationComponent>();
                ComponentManager.AddComponentToEntity(animationComponent, entityId);
            }
            return animationComponent;
        }

    }
}