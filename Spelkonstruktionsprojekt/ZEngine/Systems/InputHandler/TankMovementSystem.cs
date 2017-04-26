using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace ZEngine.Systems
{
    class TankMovementSystem : ISystem
    {
        private readonly EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public ISystem Start()
        {
            
            EventBus.Subscribe<InputEvent>(EventConstants.WalkForward, WalkForwards);
            EventBus.Subscribe<InputEvent>(EventConstants.WalkBackward, WalkBackwards);
            EventBus.Subscribe<InputEvent>(EventConstants.TurnLeft, TurnLeft);
            EventBus.Subscribe<InputEvent>(EventConstants.TurnRight, TurnRight);
            return this;
        }

        public ISystem Stop()
        {
            return this;
        }
        
        public void WalkForwards(InputEvent moveEvent)
        {
            UpdateMoveComponentIfApplicable(moveEvent.EntityId, moveComponent =>
            {
                if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
                {
                    moveComponent.CurrentAcceleration = moveComponent.AccelerationSpeed;
                }
                else if(moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyReleased)
                {
                    moveComponent.CurrentAcceleration = 0;
                }
            });
        }
        public void WalkBackwards(InputEvent moveEvent)
        {
            UpdateMoveComponentIfApplicable(moveEvent.EntityId, moveComponent =>
            {
                if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
                {
                    moveComponent.CurrentAcceleration = -moveComponent.AccelerationSpeed;
                }
                else if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyReleased)
                {
                    moveComponent.CurrentAcceleration = 0;
                }
            });
        }

        public void TurnRight(InputEvent moveEvent)
        {
            UpdateMoveComponentIfApplicable(moveEvent.EntityId, moveComponent =>
            {
                if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyDown)
                {
                    moveComponent.RotationMomentum = moveComponent.RotationSpeed;
                }
                else if(moveComponent.RotationMomentum > 0)
                {
                    moveComponent.RotationMomentum = 0;
                }
            });
        }

        public void TurnLeft(InputEvent moveEvent)
        {
            UpdateMoveComponentIfApplicable(moveEvent.EntityId, moveComponent =>
            {
                if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyDown)
                {
                    moveComponent.RotationMomentum = -moveComponent.RotationSpeed;
                }
                else if(moveComponent.RotationMomentum < 0)
                {
                    moveComponent.RotationMomentum = 0;
                }
            });
        }

        public void UpdateMoveComponentIfApplicable(int entityId, Action<MoveComponent> updateAction)
        {
            if (ComponentManager.EntityHasComponent<MoveComponent>(entityId))
            {
                var component = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entityId);
                updateAction(component);
            }
        }
    }

    public class MoveEvent
    {
        public double CurrentTimeMilliseconds = 0;
        public int EntityId { get; set; }
        public ActionBindings.KeyEvent KeyEvent { get; set; }

        public MoveEvent(int entityId, ActionBindings.KeyEvent keyEvent, double currentTimeMilliseconds)
        {
            CurrentTimeMilliseconds = currentTimeMilliseconds;
            EntityId = entityId;
            KeyEvent = keyEvent;
        }
    }
}