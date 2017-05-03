using System.Collections.Generic;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class StateComponent : IComponent
    {
        public HashSet<State> State { get; set; } = new HashSet<State>();
    }

    public enum State
    {
        WalkingForward,
        WalkingBackwards,
        Shooting,
        Dead
    }

    public static class StateManager
    {
        private static readonly EventBus EventBus = EventBus.Instance;

        public static void TryAddState(int entityId, State state, double currentTime)
        {
            var stateComponent = ComponentManager.Instance.GetEntityComponentOrDefault<StateComponent>(entityId);
            stateComponent?.State.Add(state);
            PublishStateChangeEvent(entityId, currentTime);
        }

        public static void TryRemoveState(int entityId, State state, double currentTime)
        {
            var stateComponent = ComponentManager.Instance.GetEntityComponentOrDefault<StateComponent>(entityId);
            stateComponent?.State.Remove(state);
            PublishStateChangeEvent(entityId, currentTime);
        }

        public static void PublishStateChangeEvent(int entityId, double currentTime)
        {
            EventBus.Publish(
                "StateChanged",
                new StateChangeEventBuilder()
                    .Entity(entityId)
                    .Time(currentTime).Build()
            );
        }
    }
}