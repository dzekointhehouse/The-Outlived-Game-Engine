using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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
            System.Diagnostics.Debug.WriteLine("Entity Accelerate");
            System.Diagnostics.Debug.WriteLine(entityId);
            if (ComponentManager.EntityHasComponent<MoveComponent>(entityId))
            {
                System.Diagnostics.Debug.WriteLine("Entity has component");
                var component = ComponentManager.GetEntityComponent<MoveComponent>(entityId);
                if (component.Velocity != null)
                {
                    var angle = GetAngleFromMoveComponent(component);
                    component.Velocity = Move(component.Velocity.Value, angle, 1);
                }
            }
        }
        public void EntityDeccelerate(int entityId)
        {
            System.Diagnostics.Debug.WriteLine("Entity Deccelerate");
            if (ComponentManager.EntityHasComponent<MoveComponent>(entityId))
            {
                var component = ComponentManager.GetEntityComponent<MoveComponent>(entityId);
                if (component.Velocity != null)
                {
                    var angle = GetAngleFromMoveComponent(component);
                    component.Velocity = Move(component.Velocity.Value, angle, -1);
                }
            }
        }

        public double GetAngleFromMoveComponent(MoveComponent moveComponent)
        {
            if (moveComponent.Velocity != null && NotMoving(moveComponent.Velocity.Value))
            {
                return moveComponent.RestingDirection;
            }
            return GetAngleFromVector(moveComponent.Velocity.Value);
        }
        public bool NotMoving(Vector2 velocityVector)
        {
            var TOLERANCE = 0.001;
            return Math.Abs(velocityVector.X) < TOLERANCE && Math.Abs(velocityVector.Y) < TOLERANCE;
        }

        public double GetAngleFromVector(Vector2 velocityVector)
        {
            return Math.Atan2(velocityVector.Y, velocityVector.X);
        }

        public Vector2 Move(Vector2 oldVector, double direction, int acceleration)
        {
            var x = (float)(oldVector.X + acceleration * Math.Cos(direction));
            var y = (float)(oldVector.Y + acceleration * Math.Sin(direction));
            return new Vector2(oldVector.X += x, oldVector.Y += y);
        }

        /*
         *  x = cx + r * cos(a)
         *  y = cy + r * sin(a)
         * 
         */
    }

}
