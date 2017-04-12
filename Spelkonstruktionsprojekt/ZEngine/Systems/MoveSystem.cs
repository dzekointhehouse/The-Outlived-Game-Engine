using System;
using System.Collections.Generic;
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
                    moveComponent.Velocity = Move(moveComponent.Velocity, moveComponent.Direction, moveComponent.Acceleration, delta);

                    if (MoveComponent.SomeAxisBelowMovingThreshold(moveComponent.Velocity))
                    {
                        MoveComponent.SetVelocityToRest(moveComponent);
                    }
                    MoveComponent.StopAxesAtSpeedLimit(moveComponent.Velocity, moveComponent.MaxVelocity);
                    entity.Value.PositionComponent.Position = MoveVector(entity.Value.PositionComponent.Position, moveComponent.Velocity, delta);
                    //entity.Value.PositionComponent.Position = Move(entity.Value.PositionComponent.Position, moveComponent.Direction, moveComponent.Velocity, delta);
                    moveComponent.Direction += moveComponent.RotationMomentum * delta;
                    System.Diagnostics.Debug.WriteLine("moment " + moveComponent.RotationMomentum);
                    System.Diagnostics.Debug.WriteLine("direction" + moveComponent.Direction);
                }
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

    }
}
