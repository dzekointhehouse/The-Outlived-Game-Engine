using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class BackwardsPenaltySystem : ISystem
    {
        public void Apply()
        {
            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(MoveComponent)))
            {
                var moveComponent = entity.Value as MoveComponent;
                moveComponent.AccelerationSpeed *= moveComponent.BackwardsPenaltyFactor;
            }
        }
    }
}