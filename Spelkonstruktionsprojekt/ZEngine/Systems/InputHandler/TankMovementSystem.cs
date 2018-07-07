using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace ZEngine.Systems
{
    class TankMovementSystem : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        public void Update(GameTime gt)
        {
        }

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
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(moveEvent.EntityId);
            if (moveComponent == null) return;
            if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
//                    Debug.WriteLine("WALKING FORWARDS");
                moveComponent.CurrentAcceleration = moveComponent.AccelerationSpeed;
                StateManager.TryAddState(moveEvent.EntityId, State.WalkingForward, moveEvent.EventTime);
            }
            else if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyReleased)
            {
                moveComponent.CurrentAcceleration = 0;
                StateManager.TryRemoveState(moveEvent.EntityId, State.WalkingForward, moveEvent.EventTime);
            }
        }

        public void WalkBackwards(InputEvent moveEvent)
        {
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(moveEvent.EntityId);
            if (moveComponent == null) return;
            if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                moveComponent.CurrentAcceleration = -moveComponent.AccelerationSpeed;
                StateManager.TryAddState(moveEvent.EntityId, State.WalkingBackwards, moveEvent.EventTime);
            }
            else if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyReleased)
            {
                moveComponent.CurrentAcceleration = 0;
                StateManager.TryRemoveState(moveEvent.EntityId, State.WalkingBackwards, moveEvent.EventTime);
            }
        }

        public void TurnRight(InputEvent moveEvent)
        {
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(moveEvent.EntityId);
            if (moveComponent == null) return;
            if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyDown)
            {
                moveComponent.RotationMomentum = moveComponent.RotationSpeed;
            }
            else if (moveComponent.RotationMomentum > 0)
            {
                moveComponent.RotationMomentum = 0;
            }
        }

        public void TurnLeft(InputEvent moveEvent)
        {
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(moveEvent.EntityId);
            if (moveComponent == null) return;
            if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyDown)
            {
                moveComponent.RotationMomentum = -moveComponent.RotationSpeed;
            }
            else if (moveComponent.RotationMomentum < 0)
            {
                moveComponent.RotationMomentum = 0;
            }
        }
    }
}