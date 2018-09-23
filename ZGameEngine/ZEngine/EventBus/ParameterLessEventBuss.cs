using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ZEngine.EventBus
{
    public class ParameterlessEventBus
    {
        private readonly Dictionary<string, ICollection<Action>> _actions;

        public ParameterlessEventBus()
        {
            _actions = new Dictionary<string, ICollection<Action>>();
        }

        /**
         *  When publishing a message and a value of certain type it
         *  will initiate an Action stored in "_actions" that corresponds
         *  to the value type and the message.
         */
        public void Publish(string message)
        {
            if (NoCallbacksForMessage(message)) return;

            foreach (var action in _actions[message])
            {
                action.Invoke();
            }
        }

        /**
         * When subscribing to a certain message with a certain type
         * will execute callback when some value of that same
         * type and with that same message is pusblished to the EventBus.
         */

        public void Subscribe(string message, Action callback)
        {
            if (NoCallbacksForMessage(message))
            {
                _actions[message] = new Collection<Action> { callback };
            }
            else
            {
                _actions[message].Add(callback);
            }
        }

        public void Unsubscribe(string message, Action callback)
        {
            if (NoCallbacksForMessage(message)) throw new Exception("Nothing to unsubscribe.");

            _actions[message].Remove(callback);
        }

        private bool NoCallbacksForMessage(string message)
        {
            return !_actions.ContainsKey(message);
        }
    }
}
