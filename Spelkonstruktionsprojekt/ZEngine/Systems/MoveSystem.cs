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
        public void Move()
        {
            var moveEntities = ComponentManager.GetEntitiesWithComponent<MoveComponent>();
            var renderEntities = ComponentManager.GetEntitiesWithComponent<RenderComponent>();
            foreach (var entity in renderEntities)
            {
                if (moveEntities.ContainsKey(entity.Key))
                {
                    var moveComponent = moveEntities[entity.Key];
                    moveComponent.Velocity = MoveVector(moveComponent.Velocity, moveComponent.Acceleration);
                    if (MoveComponentHelper.SomeAxisBelowMovingThreshold(moveComponent.Velocity))
                    {
                        MoveComponentHelper.SetVelocityToRest(moveComponent);
                    }
                    entity.Value.PositionComponent.Position = MoveVector(entity.Value.PositionComponent.Position, moveComponent.Velocity);
                }
            }
        }

        public Vector2D MoveVector(Vector2D oldVector, Vector2D deltaVector)
        {
            var x = deltaVector.X;
            var y = deltaVector.Y;
            return Vector2D.Create(oldVector.X + x, oldVector.Y + y);
        }
    }
}
