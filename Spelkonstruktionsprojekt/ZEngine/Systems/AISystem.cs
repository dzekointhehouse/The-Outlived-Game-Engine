using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class AISystem : ISystem
    {
        private ComponentManager ComponentManager = ComponentManager.Instance;
        public void Process(GameTime gameTime)
        {
            var delta = gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var entity in ComponentManager.GetEntitiesWithComponent<AIComponent>())
            {
                var firstPlayer = ComponentManager.GetEntitiesWithComponent<MoveComponent>().First();
                var firstPlayerRenderComponent = ComponentManager.GetEntityComponentOrDefault<RenderComponent>(firstPlayer.Key);
                var aiMoveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
                var aiRenderComponent = ComponentManager.GetEntityComponentOrDefault<RenderComponent>(entity.Key);

                Vector2 playerPos = firstPlayerRenderComponent.PositionComponent.Position;
                Vector2 aiPos = aiRenderComponent.PositionComponent.Position;

                var dir = playerPos - aiPos;
                dir.Normalize();
                double newDirection = Math.Atan2(dir.Y, dir.X);

                aiMoveComponent.Direction = newDirection;
                if (aiMoveComponent.VelocitySpeed < 1)
                {
                    aiMoveComponent.VelocitySpeed = 1;
                }
            }
        }
    }
}
