using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;

namespace ZEngine.Systems
{
    class MoveSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public void Move(GameTime gameTime)
        {
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            var moveEntities = ComponentManager.GetEntitiesWithComponent(typeof(MoveComponent));
            var positionComponents =
                ComponentManager.GetEntitiesWithComponent(typeof(PositionComponent))
                    .Where(entry => moveEntities.ContainsKey(entry.Key));
            foreach (var entity in positionComponents)
            {
                var moveComponent = moveEntities[entity.Key] as MoveComponent;

                //Play direction based on angular momentum
                moveComponent.PreviousDirection = moveComponent.Direction;
                moveComponent.Direction =
                    (float) ((moveComponent.Direction + moveComponent.RotationMomentum * delta) %
                             MathHelper.TwoPi);
                //If the entity is moving, calculate new acceleration
                //And if moving backwards, apply backwards motion penalty factor
                moveComponent.Speed += (float) (moveComponent.CurrentAcceleration * delta);

                //Limit velocity if above max velocity
                ApplyVelocityLimits(moveComponent);

                //Play Velocity based on current direction and acceleration
                var oldVector = new Vector2(0, 0);
                var velocity = MoveDirectly(ref oldVector, moveComponent.Direction,
                    moveComponent.Speed);

                // Play position with current velocity.
                var positionComponent = entity.Value as PositionComponent;
                var valuePosition = positionComponent.Position;

                moveComponent.PreviousPosition = positionComponent.Position;
                positionComponent.Position = MoveVector(ref valuePosition, ref velocity, delta);
//
//                System.Diagnostics.Debug.WriteLine(
//                    "moment " + moveComponent.RotationMomentum
//                    + " direction " + moveComponent.Direction
//                    + " delta " + delta
//                    + "  maxVelocity " + moveComponent.MaxVelocitySpeed.ToString()
//                    + "  velocitySpeed " + moveComponent.Speed
//                    + "  position " + positionComponent.Position
//                );
            }
        }

        public void ApplyVelocityLimits(MoveComponent moveComponent)
        {
            if (moveComponent.Speed > moveComponent.MaxVelocitySpeed)
            {
                moveComponent.Speed = moveComponent.MaxVelocitySpeed;
            }
            else if (moveComponent.Speed < -moveComponent.MaxVelocitySpeed * moveComponent.BackwardsPenaltyFactor)
            {
                moveComponent.Speed = (float) (-moveComponent.MaxVelocitySpeed * moveComponent.BackwardsPenaltyFactor);
            }
        }

        public Vector2 MoveVector(ref Vector2 oldVector, ref Vector2 deltaVector, float deltaTime)
        {
            var x = deltaVector.X * deltaTime;
            var y = deltaVector.Y * deltaTime;
            return new Vector2(oldVector.X + x, oldVector.Y + y);
        }

        public Vector2 Move(ref Vector2 oldVector, float direction, ref Vector2 acceleration, float deltaTime)
        {
            var x1 = acceleration.X * Math.Cos(direction) * deltaTime;
            var y1 = acceleration.Y * Math.Sin(direction) * deltaTime;
            var x = (oldVector.X + x1);
            var y = (oldVector.Y + y1);
            return new Vector2((float) x, (float) y);
        }

        public Vector2 MoveDirectly(ref Vector2 oldVector, float direction, float acceleration)
        {
            var x1 = acceleration * Math.Cos(direction);
            var y1 = acceleration * Math.Sin(direction);
            var x = (oldVector.X + x1);
            var y = (oldVector.Y + y1);
            return new Vector2((float) x, (float) y);
        }

        public bool HasCollided(int entityId)
        {
            if (ComponentManager.EntityHasComponent<CollisionComponent>(entityId))
            {
                var collisionComponent = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(entityId);
                return collisionComponent.collisions.Count > 0;
            }
            return false;
        }
    }
}