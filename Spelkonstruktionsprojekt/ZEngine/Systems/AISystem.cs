using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MoreLinq;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
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
                var aiComponent = entity.Value as AIComponent;
                var aiPosition = aiPositionComponent.Position;



                var playerEntities = componentManager.GetEntitiesWithComponent(typeof(PlayerComponent));
                if (playerEntities.Count == 0)
                {
                    aiMoveComponent.CurrentAcceleration = 0;
                    continue;
                }

                //Get closest players that
                //    - Has position component
                //    - Has flashlight component.
                //    - Has a flashlight turned on
                // We want to get all the players that achieve this criteria above.
                // We do this by using linq. The first where clause gets the players that have
                // that criteria, select clause selects the player properties that we need, and
                // minby compares the distances and gives us the player with the smallest distance 
                // to the player.
                var closestPlayer = playerEntities
                    .Where(e =>
                    {
                        var hasPositionComponent =
                            ComponentManager.Instance.EntityHasComponent<PositionComponent>(e.Key);
                        if (!hasPositionComponent) return false;

                        var lightComponent =
                            ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(e.Key);
                        if (lightComponent == null) return false;

                        var hasFlashlightEnabled = lightComponent.Light.Enabled;
                        if (hasFlashlightEnabled) return true;
                        else return false;
                    })
                    .Select(e =>
                    {
                        var positionComponent =
                            ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(e.Key);
                        var distance = Vector2.Distance(positionComponent.Position, aiPosition);

                        return new Tuple<float, PositionComponent>(
                            distance,
                            positionComponent
                            );
                    })
                    .MinBy(e =>
                    {
                        // item1 is the distance
                        var distance = e.Item1;
                        return distance;
                    });


                var closestPlayerDistance = closestPlayer.Item1;
                var closestPlayerPosition = closestPlayer.Item2.Position;

                // If The player is within the distance that the AI will follow then we start moving
                // the ai towards that player.
                if (closestPlayerDistance < aiComponent.FollowDistance)
                {
                    var dir = closestPlayerPosition - aiPosition;
                    dir.Normalize();
                    var newDirection = Math.Atan2(dir.Y, dir.X);

                    aiMoveComponent.Direction = (float)newDirection;

                    aiMoveComponent.CurrentAcceleration = aiMoveComponent.AccelerationSpeed; //Make AI move.
                }
                else
                {
                    aiMoveComponent.CurrentAcceleration = 0;
                }
            }
        }
    }
}