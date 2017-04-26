using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class InertiaDampenerSystem : ISystem
    {
        public void Apply(GameTime gameTime)
        {
            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(MoveComponent)))
            {
                var moveComponent = entity.Value as MoveComponent;
                var dampeningComponent = ComponentManager.Instance
                    .GetEntityComponentOrDefault<InertiaDampeningComponent>(entity.Key);
                Debug.WriteLine("inside dampening 1");
                if (dampeningComponent == null) return;
                Debug.WriteLine("inside dampening 2");

                if (moveComponent == null) return;
                if (moveComponent.Speed < -0.01)
                {
                    Debug.WriteLine("damp up");
                    moveComponent.Speed += dampeningComponent.StabilisingSpeed;
                }
                else if (moveComponent.Speed > 0.01)
                {
                    Debug.WriteLine("damp down");
                    moveComponent.Speed -= dampeningComponent.StabilisingSpeed;
                }
                else
                {
                    moveComponent.Speed = 0;
                }
            }
        }
    }
}