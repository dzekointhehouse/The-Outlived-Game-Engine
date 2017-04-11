using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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
                    entity.Value.PositionComponent.Position = MoveVector(entity.Value.PositionComponent.Position, moveComponent);
                }
            }
        }

        public Vector2 MoveVector(Vector2 oldVector, MoveComponent moveComponent)
        {
            if (moveComponent.Velocity == null) return oldVector;

            var x = moveComponent.Velocity.Value.X;
            var y = moveComponent.Velocity.Value.Y;
            return new Vector2(oldVector.X += x, oldVector.Y += y);
        }
    }
}
