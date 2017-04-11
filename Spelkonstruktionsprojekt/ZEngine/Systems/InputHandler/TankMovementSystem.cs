using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Components.MoveComponent;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class TankMovementSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;
        private Action<int> _entityAccelerate;
        private Action<int> _entityDeccelerate;

        public TankMovementSystem()
        {
            _entityAccelerate = new Action<int>(EntityAccelerate);
            _entityDeccelerate = new Action<int>(EntityDeccelerate);
        }
        public ISystem Start()
        {
            EventBus.Subscribe<int>("entityAccelerate", _entityAccelerate);
            EventBus.Subscribe<int>("entityDeccelerate", _entityDeccelerate);
            return this;
        }

        public ISystem Stop()
        {
            return this;
        }

        public void EntityAccelerate(int entityId)
        {
            UpdateMoveComponentIfApplicable(entityId, moveComponent =>
            {
                System.Diagnostics.Debug.WriteLine("Entity Accelerate");
                if (moveComponent.Velocity != null)
                {
                    var angle = moveComponent.Direction;
                    moveComponent.Acceleration = Move(moveComponent.Acceleration, angle, 0.001);
                    MoveComponentHelper.StopMotionOnAxesBelowMovingThreshold(moveComponent.Acceleration);
                }
            });
        }
        public void EntityDeccelerate(int entityId)
        {
            UpdateMoveComponentIfApplicable(entityId, moveComponent =>
            {
                System.Diagnostics.Debug.WriteLine("Entity Deccelerate");
                if (moveComponent.Velocity != null)
                {
                    var angle = moveComponent.Direction;
                    moveComponent.Acceleration = Move(moveComponent.Acceleration, angle, -0.001);
                    MoveComponentHelper.StopMotionOnAxesBelowMovingThreshold(moveComponent.Acceleration);
                }
            });
        }

        public void UpdateMoveComponentIfApplicable(int entityId, Action<MoveComponent> updateAction)
        {
            if (ComponentManager.EntityHasComponent<MoveComponent>(entityId))
            {
                var component = ComponentManager.GetEntityComponent<MoveComponent>(entityId);
                updateAction(component);
            }
        }

        public Vector2D Move(Vector2D oldVector, double direction, double acceleration)
        {
            var x1 = acceleration * Math.Cos(direction);
            var y1 = acceleration * Math.Sin(direction);
            var x = (oldVector.X + x1);
            var y = (oldVector.Y + y1);
            System.Diagnostics.Debug.WriteLine("x:" + x + " xc:" + x1 + ", y:" + y + " ys:" + y1 + " acc:" + acceleration + ", angle:" + direction);
            return Vector2D.Create(x, y);
        }

        /*
         *  x = cx + r * cos(a)
         *  y = cy + r * sin(a)
         * 
         */
    }

}
