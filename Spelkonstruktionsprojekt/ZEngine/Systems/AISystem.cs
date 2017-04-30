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
            foreach (var entity in componentManager.GetEntitiesWithComponent(typeof(AIComponent)))
            {


                var aiMoveComponent = componentManager.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
                var aiPositionComponent = componentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);


                // We should be getting all the players and check so the ai (monster) should
                // follow the closest player
                var playerComponent = componentManager.GetEntitiesWithComponent(typeof(PlayerComponent));
                if (playerComponent == null) return;

                float distance;

                foreach (var player in playerComponent)
                {
                    // We cannot follow a player without a position
                    var playerPositionComponent = componentManager.GetEntityComponentOrDefault<PositionComponent>(player.Key);
                    if (playerPositionComponent == null) return;



                    // distance = playerPositionComponent.Position - aiPositionComponent.Position;

                    AIComponent aiComponent = entity.Value as AIComponent;


                    // If The player is within the distance that the AI will follow then we start moving 
                    // the ai towards that player. We also want to check so that the AI follows the closest
                    // player, and not just the first player within the distance...but maybe, we'll check how it looks first.

                    var position = playerPositionComponent.Position;
                    var vector2 = aiPositionComponent.Position;

                    // Gives us a float distance that we can use for the Ai
                    // to see if it should follow the player. 
                    Vector2.Distance(ref position, ref vector2,
                        out distance);

                    // Follow the player if distance is within accepted range
                    if (distance < aiComponent.FollowDistance)
                    {
                        // We need the players lightComponent because we only want 
                        // to follow players that have flashligt on.
                        var playerLightComponent = componentManager.GetEntityComponentOrDefault<LightComponent>(player.Key);
                        if (playerLightComponent == null) return;

                        if (playerLightComponent.Light.Enabled)
                        {
                            Vector2 playerPos = playerPositionComponent.Position;
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
        }
    }
}
