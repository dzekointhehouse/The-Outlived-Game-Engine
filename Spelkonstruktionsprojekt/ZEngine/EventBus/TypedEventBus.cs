using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEngine.EventBus
{
    public class TypedEventBus
    {
        private readonly Dictionary<Type, Dictionary<string, ICollection<Action<object>>>> _actions = new Dictionary<Type, Dictionary<string, ICollection<Action<object>>>>();

        /**
         *  When publishing a message and a value of certain type
         *  will initiate an Action stored in "_actions" that corresponds
         *  to the value type and the message.
         */
        public void Publish<T>(string message, T value)
        {
            if (NoCallbacksForMessage<T>(message)) return;

            foreach (var action in _actions[typeof(T)][message])
            {
                (action as Action<T>)?.Invoke(value);
            }
        }

        /**
         * When subscribing to a certain message with a certain type
         * will execute callback when some value of that same
         * type and with that same message is pusblished to the EventBus.
         */

        public void Subscribe<T>(string message, Action<T> callback)
        {
            if (NoCallbacksForMessage<T>(message))
            {
                if (NoMessageArraysForType<T>())
                {
                    _actions[typeof(T)] = new Dictionary<string, ICollection<Action<object>>>();
                }

                _actions[typeof(T)][message] = new Collection<Action<object>>() {callback as Action<object>};
            }
            else
            {
                _actions[typeof(T)][message].Add(callback as Action<object>);
            }
        }

        public void Unsubscribe<T>(string message, Action<T> callback)
        {
            if (NoCallbacksForMessage<T>(message)) throw new Exception("Nothing to unsubscribe.");

            _actions[typeof(T)][message].Remove(callback as Action<object>);
        }

        private bool NoCallbacksForMessage<T>(string message)
        {
            return !_actions.ContainsKey(typeof(T)) || !_actions[typeof(T)].ContainsKey(message);
        }

        private bool NoMessageArraysForType<T>()
        {
            return !_actions.ContainsKey(typeof(T));
        }
    }
}
