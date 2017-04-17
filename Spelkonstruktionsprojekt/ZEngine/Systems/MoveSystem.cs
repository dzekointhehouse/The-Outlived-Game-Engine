using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Components;
using ZEngine.Managers;

namespace ZEngine.Systems
{
    class MoveSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public void Move(GameTime gameTime)
        {
            var delta = gameTime.ElapsedGameTime.TotalSeconds;
            var moveEntities = ComponentManager.GetEntitiesWithComponent<MoveComponent>();
            var renderEntities = ComponentManager.GetEntitiesWithComponent<RenderComponent>();
            foreach (var entity in renderEntities)
            {
                if (moveEntities.ContainsKey(entity.Key))
                {
                    var moveComponent = moveEntities[entity.Key];

                    //Start direction based on angular momentum
                    moveComponent.Direction = (moveComponent.Direction + moveComponent.RotationMomentum * delta) %
                                              MathHelper.TwoPi;

                    //If the entity is moving, calculate new acceleration
                    //And if moving backwards, apply backwards motion penalty factor
                    var entityIsMoving = moveComponent.VelocitySpeed < -0.01 || moveComponent.VelocitySpeed > 0.01;
                    if (entityIsMoving)
                    {
                        //If an object is to accerlerate/deccelerate the VelocitySpeed is set to slightly above or below 0
                        // this is done by movement systems.
                        //If the velocity speed is positive than increase acceleration
                        //Else, decrease (i.e. deccelerate)
                        var multiplier = moveComponent.VelocitySpeed > 0
                            ? 1
                            : (-1 * moveComponent.BackwardsPenaltyFactor);

                        moveComponent.VelocitySpeed += multiplier * moveComponent.AccelerationSpeed * delta;
                    }

                    //Limit velocity if above max velocity
                    ApplyVelocityLimits(moveComponent);

                    //Start Velocity based on current direction and acceleration
                    moveComponent.Velocity = MoveDirectly(new Vector2(0, 0), moveComponent.Direction,
                        moveComponent.VelocitySpeed);

                    // Start position with current velocity.
                    moveComponent.PreviousPosition = entity.Value.PositionComponent.Position;
                    entity.Value.PositionComponent.Position = MoveVector(entity.Value.PositionComponent.Position, moveComponent.Velocity, delta);

                    //System.Diagnostics.Debug.WriteLine(
                    //    "moment " + moveComponent.RotationMomentum
                    //    + " direction " + moveComponent.Direction
                    //    + " velocity " + moveComponent.Velocity.ToString()
                    //    + " delta " + delta
                    //    + "  maxVelocity " + moveComponent.MaxVelocity.ToString()
                    //    + "  velocitySpeed " + moveComponent.VelocitySpeed
                    //);
                }
            }
        }

        public void ApplyVelocityLimits(MoveComponent moveComponent)
        {
            if (moveComponent.VelocitySpeed > moveComponent.MaxVelocitySpeed)
            {
                moveComponent.VelocitySpeed = moveComponent.MaxVelocitySpeed;
            }
            else if (moveComponent.VelocitySpeed < -moveComponent.MaxVelocitySpeed * moveComponent.BackwardsPenaltyFactor)
            {
                moveComponent.VelocitySpeed = -moveComponent.MaxVelocitySpeed * moveComponent.BackwardsPenaltyFactor;
            }
        }

        public Vector2D MoveVector(Vector2D oldVector, Vector2D deltaVector, double deltaTime)
        {
            var x = deltaVector.X * deltaTime;
            var y = deltaVector.Y * deltaTime;
            return Vector2D.Create(oldVector.X + x, oldVector.Y + y);
        }

        public Vector2D Move(Vector2D oldVector, double direction, Vector2D acceleration, double deltaTime)
        {
            var x1 = acceleration.X * Math.Cos(direction) * deltaTime;
            var y1 = acceleration.Y * Math.Sin(direction) * deltaTime;
            var x = (oldVector.X + x1);
            var y = (oldVector.Y + y1);
            return Vector2D.Create(x, y);
        }

        public Vector2D MoveDirectly(Vector2D oldVector, double direction, double acceleration)
        {
            var x1 = acceleration * Math.Cos(direction);
            var y1 = acceleration * Math.Sin(direction);
            var x = (oldVector.X + x1);
            var y = (oldVector.Y + y1);
            return Vector2D.Create(x, y);
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