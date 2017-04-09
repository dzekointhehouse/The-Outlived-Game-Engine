using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEngine.EventBus
{
    public interface IEventBus
    {
        void Publish(string message);
        void Publish<T>(string message, T value);

        void Subscribe(string message, Action callback);
        void Subscribe<T>(string message, Action<T> callback);

        void Unsubscribe(string message, Action callback);
        void Unsubscribe<T>(string message, Action<T> callback);
    }
}
