using System.Collections.Generic;
using System.Diagnostics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.EventBus;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class StateComponent : IComponent
    {
        public HashSet<State> State { get; set; } = new HashSet<State>();

        public IComponent Reset()
        {
            State.Clear();
            return this;
        }
    }

    public enum State
    {
        WalkingForward,
        WalkingBackwards,
        Shooting,
        Dead,
        Collided
    }

    public static class StateManager
    {
        private static readonly EventBus EventBus = EventBus.Instance;

        public static void TryAddState(uint entityId, State state, double currentTime)
        {
//            System.Diagnostics.Debug.WriteLine("Adding state: " + state);
            var stateComponent = ComponentManager.Instance.GetEntityComponentOrDefault<StateComponent>(entityId);
            if (!stateComponent.State.Contains(state)) //TODO effectives so that StateChange dont get called so often. This might be from the GamePad systems!
            {
                stateComponent?.State.Add(state);
                PublishStateChangeEvent(entityId, currentTime);
            }
        }

        public static void TryRemoveState(uint entityId, State state, double currentTime)
        {
//            System.Diagnostics.Debug.WriteLine("Removing state: " + state);
            var stateComponent = ComponentManager.Instance.GetEntityComponentOrDefault<StateComponent>(entityId);
            stateComponent?.State.Remove(state);
            PublishStateChangeEvent(entityId, currentTime);
        }

        public static void PublishStateChangeEvent(uint entityId, double currentTime)
        {
            EventBus.Publish(
                "StateChanged",
                new StateChangeEventBuilder()
                    .Entity(entityId)
                    .Time(currentTime)
                    .Build()
            );
        }
    }
}