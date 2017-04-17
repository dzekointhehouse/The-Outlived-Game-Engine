using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace ZEngine.Systems
{
    class TankMovementSystem : ISystem
    {
        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;
        private Action<MoveEvent> _entityAccelerate;
        private Action<MoveEvent> _entityDeccelerate;

        public TankMovementSystem()
        {
            _entityAccelerate = new Action<MoveEvent>(WalkForwards);
            _entityDeccelerate = new Action<MoveEvent>(WalkBackwards);
        }

        public ISystem Start()
        {
            EventBus.Subscribe<MoveEvent>("entityWalkForwards", _entityAccelerate);
            EventBus.Subscribe<MoveEvent>("entityWalkBackwards", _entityDeccelerate);
            EventBus.Subscribe<MoveEvent>("entityTurnLeft", TurnLeft);
            EventBus.Subscribe<MoveEvent>("entityTurnRight", TurnRight);
            return this;
        }

        public ISystem Stop()
        {
            return this;
        }
        
        public void WalkForwards(MoveEvent moveEvent)
        {
            UpdateMoveComponentIfApplicable(moveEvent.EntityId, moveComponent =>
            {
                if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
                {
                    moveComponent.VelocitySpeed = 1;
                }
                else if(moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyReleased)
                {
                    moveComponent.VelocitySpeed = 0;
                }
            });
        }
        public void WalkBackwards(MoveEvent moveEvent)
        {
            UpdateMoveComponentIfApplicable(moveEvent.EntityId, moveComponent =>
            {
                if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
                {
                    moveComponent.VelocitySpeed = -1;
                }
                else if (moveEvent.KeyEvent == ActionBindings.KeyEvent.KeyReleased)
                {
                    moveComponent.VelocitySpeed = 0;
                }
            });
        }

        public void TurnRight(MoveEvent moveEvent)
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

        public void TurnLeft(MoveEvent moveEvent)
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
                if (component.Velocity != null && component.Acceleration != null)
                {
                    updateAction(component);
                }
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