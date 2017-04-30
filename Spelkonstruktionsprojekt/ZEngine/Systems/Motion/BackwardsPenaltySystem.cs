using System.Diagnostics;
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
                if (isMovingBackwards && backwardsPenaltyComponent.PreProcessingAcceleration == 0)
                {
                    Debug.WriteLine("1");
                    backwardsPenaltyComponent.PreProcessingAcceleration = moveComponent.CurrentAcceleration;
                    moveComponent.CurrentAcceleration *= moveComponent.BackwardsPenaltyFactor;
                }
                else if (isMovingBackwards)
                {
                    Debug.WriteLine("2");
                    moveComponent.CurrentAcceleration = backwardsPenaltyComponent.PreProcessingAcceleration *
                                                        backwardsPenaltyComponent.BackwardsPenaltyFactor;
                }
                else
                {
                    Debug.WriteLine("3");
                    backwardsPenaltyComponent.PreProcessingAcceleration = 0;
                }
            }
        }
    }
}