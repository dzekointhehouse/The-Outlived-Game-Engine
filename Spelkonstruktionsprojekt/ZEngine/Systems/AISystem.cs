using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class AISystem : ISystem
    {
        private ComponentManager componentManager = ComponentManager.Instance;

        public void Update(GameTime gameTime)
        {
            var delta = gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var entity in componentManager.GetEntitiesWithComponent<AIComponent>())
            {
                var firstPlayer = componentManager.GetEntitiesWithComponent<MoveComponent>().First();
//                var firstPlayerRenderComponent = componentManager.GetEntityComponentOrDefault<RenderComponent>(firstPlayer.Key);
                var aiMoveComponent = componentManager.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
//                var aiRenderComponent = componentManager.GetEntityComponentOrDefault<RenderComponent>(entity.Key);

                var aiPositionComponent = componentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                var firstPlayerPositionComponent = componentManager.GetEntityComponentOrDefault<PositionComponent>(firstPlayer.Key);


                Vector2 playerPos = firstPlayerPositionComponent.Position;
                Vector2 aiPos = aiPositionComponent.Position;

                var dir = playerPos - aiPos;
                dir.Normalize();
                var newDirection = Math.Atan2(dir.Y, dir.X);

                aiMoveComponent.Direction = (float) newDirection;
                if (aiMoveComponent.Speed < 1)
                {
                    aiMoveComponent.Speed = 1;
                }
            }
        }
    }
}
