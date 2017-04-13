using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Components;
using ZEngine.Components.MoveComponent;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
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

                    moveComponent.Direction = (moveComponent.Direction + moveComponent.RotationMomentum * delta) % MathHelper.TwoPi;

                    var entityIsMoving = moveComponent.VelocitySpeed < -0.01 || moveComponent.VelocitySpeed > 0.01;
                    if (entityIsMoving)
                    {
                        var multiplier = moveComponent.VelocitySpeed > 0 ? 1 : (-1 * moveComponent.BackwardsPenaltyFactor);
                        moveComponent.VelocitySpeed += multiplier * moveComponent.AccelerationSpeed * delta; //Accelerate
                    }
                    ApplyVelocityLimits(moveComponent);
                    moveComponent.Velocity = MoveDirectly(new Vector2(0,0), moveComponent.Direction, moveComponent.VelocitySpeed);
                    
                    entity.Value.PositionComponent.Position = MoveVector(entity.Value.PositionComponent.Position, moveComponent.Velocity, delta);

                    System.Diagnostics.Debug.WriteLine(
                        "moment " + moveComponent.RotationMomentum
                        + " direction " + moveComponent.Direction
                        + " velocity " + moveComponent.Velocity.ToString()
                        + " delta " + delta
                        + "  maxVelocity " + moveComponent.MaxVelocity.ToString()
                        + "  velocitySpeed " + moveComponent.VelocitySpeed
                    );
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

    }
}
