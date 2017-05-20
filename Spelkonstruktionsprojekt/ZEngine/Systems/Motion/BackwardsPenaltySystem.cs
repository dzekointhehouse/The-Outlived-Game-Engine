using System.Diagnostics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class BackwardsPenaltySystem : ISystem
    {
        public void Apply()
        {
            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(BackwardsPenaltyComponent)))
            {
                var backwardsPenaltyComponent = entity.Value as BackwardsPenaltyComponent;
                var moveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
                if (moveComponent == null) return;

                var isMovingBackwards = moveComponent.CurrentAcceleration < -0.01;

                // Enters first block on the first run through this system
                // So we dont't apply penalty factor more than once on the initial acceleration.
                if (isMovingBackwards && backwardsPenaltyComponent.AccelerationBeforeBackwardsPenaltyApplied == 0)
                {
                    backwardsPenaltyComponent.AccelerationBeforeBackwardsPenaltyApplied = moveComponent.CurrentAcceleration;
                    moveComponent.CurrentAcceleration *= backwardsPenaltyComponent.BackwardsPenaltyFactor;
                }
                else if (isMovingBackwards)
                {
                    moveComponent.CurrentAcceleration = backwardsPenaltyComponent.AccelerationBeforeBackwardsPenaltyApplied *
                                                        backwardsPenaltyComponent.BackwardsPenaltyFactor;
                }
                else
                {
                    backwardsPenaltyComponent.AccelerationBeforeBackwardsPenaltyApplied = 0;
                }
            }
        }
    }
}