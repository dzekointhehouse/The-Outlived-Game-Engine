using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class InertiaDampenerSystem : ISystem
    {
        public void Apply(GameTime gameTime)
        {
            var delta = gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(MoveComponent)))
            {
                var moveComponent = entity.Value as MoveComponent;
                var dampeningComponent = ComponentManager.Instance
                    .GetEntityComponentOrDefault<InertiaDampeningComponent>(entity.Key);
                if (dampeningComponent == null) return;
                if (moveComponent == null) return;

                if (moveComponent.CurrentAcceleration > -0.01 && moveComponent.Speed < 0)
                {
                    moveComponent.Speed += (float)(dampeningComponent.StabilisingSpeed * delta);
                    if (moveComponent.Speed > 0)
                    {
                        moveComponent.Speed = 0;
                    }
                }
                else if (moveComponent.CurrentAcceleration < 0.01 && moveComponent.Speed > 0)
                {
                    moveComponent.Speed -= (float) (dampeningComponent.StabilisingSpeed * delta);
                    if (moveComponent.Speed < 0)
                    {
                        moveComponent.Speed = 0;
                    }
                }
            }
        }
    }
}