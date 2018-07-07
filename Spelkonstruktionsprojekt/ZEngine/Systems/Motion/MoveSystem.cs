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
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Components;
using ZEngine.Managers;

namespace ZEngine.Systems
{
    class MoveSystem : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public void Update(GameTime gameTime)
        {
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(MoveComponent)))
            {
                var moveComponent = entity.Value as MoveComponent;

                var positionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                if (positionComponent == null) continue;
                var backwardsPenaltyComponent =
                    ComponentManager.GetEntityComponentOrDefault<BackwardsPenaltyComponent>(entity.Key);


                //Play direction based on angular momentum
                moveComponent.PreviousDirection = moveComponent.Direction;
                moveComponent.Direction =
                    (float) ((moveComponent.Direction + moveComponent.RotationMomentum * delta) %
                             MathHelper.TwoPi);
                //If the entity is moving, calculate new acceleration
                //And if moving backwards, apply backwards motion penalty factor
                moveComponent.Speed += (float) (moveComponent.CurrentAcceleration * delta);

                //Limit velocity if above max velocity
                ApplyVelocityLimits(moveComponent, backwardsPenaltyComponent);

                //Play Velocity based on current direction and acceleration
                var oldVector = new Vector2(0, 0);
                var velocity = MoveDirectly(ref oldVector, moveComponent.Direction,
                    moveComponent.Speed);

                // Play position with current velocity.
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
            // Future implementation

            //var playerComponents = ComponentManager.GetEntitiesWithComponent(typeof(PlayerComponent));
            //foreach (var player in playerComponents)
            //{
            //    var cameraComponent = ComponentManager.GetEntityComponentOrDefault(typeof(CameraViewComponent), player.Key);
            //    CameraViewComponent camera = cameraComponent as CameraViewComponent;
            //    var positionComponent = ComponentManager.GetEntityComponentOrDefault(typeof(PositionComponent), player.Key);
            //    var position = positionComponent as PositionComponent;

            //    var x = MathHelper.Clamp(position.Position.X, camera.View.X + 50, camera.View.Width - 50);
            //    var y = MathHelper.Clamp(position.Position.Y, camera.View.Y + 50, camera.View.Height - 50);

            //    position.Position = new Vector2(x, y);
            //}
        }

        public void ApplyVelocityLimits(MoveComponent moveComponent, BackwardsPenaltyComponent backwardsPenaltyComponent = null)
        {
            double backwardsPenaltyFactor = 1;
            if (backwardsPenaltyComponent != null)
            {
                backwardsPenaltyFactor = backwardsPenaltyComponent.BackwardsPenaltyFactor;
            }

            if (moveComponent.Speed > moveComponent.MaxVelocitySpeed)
            {
                moveComponent.Speed = moveComponent.MaxVelocitySpeed;
            }
            else if (moveComponent.Speed < -moveComponent.MaxVelocitySpeed * backwardsPenaltyFactor)
            {
                moveComponent.Speed = (float) (-moveComponent.MaxVelocitySpeed * backwardsPenaltyFactor);
            }
        }

        public Vector2 MoveVector(ref Vector2 oldVector, ref Vector2 deltaVector, float deltaTime)
        {
            var x = deltaVector.X * deltaTime;
            var y = deltaVector.Y * deltaTime;
            return new Vector2(oldVector.X + x, oldVector.Y + y);
        }

        public Vector2 Update(ref Vector2 oldVector, float direction, ref Vector2 acceleration, float deltaTime)
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

        public bool HasCollided(uint entityId)
        {
            var collisionComponent = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(entityId);
            if (collisionComponent == null) return false;
            return collisionComponent.Collisions.Count > 0;
        }
    }
}