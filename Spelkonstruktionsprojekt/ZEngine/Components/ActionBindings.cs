using System.Collections.Generic;
using System.Security.AccessControl;
using Microsoft.Xna.Framework.Input;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class ActionBindings : IComponent
    {
        /**
         *  Actions is a dictionary where the Key of a Keyboard is connected to an event string and a KeyEvent enum.
         */
        public Dictionary<Keys, Dictionary<KeyEvent, string>> Actions = new Dictionary<Keys, Dictionary<KeyEvent, string>>();

        public enum KeyEvent
        {
            KeyUp,
            KeyDown,
            KeyPressed,
            KeyReleased,
            KeySustain
        }
    }

    public class ActionBindingsBuilder
    {

        private readonly ActionBindings _actionBindings = new ActionBindings();

        public ActionBindingsBuilder SetAction(Keys key, ActionBindings.KeyEvent keyEvent, string eventName)
        {
            AddKeyIfNotThere(key);
            _actionBindings.Actions[key][keyEvent] = eventName;
            return this;
        }

        public ActionBindings Build()
        {
            return _actionBindings;
        }

        private void AddKeyIfNotThere(Keys key)
        {
            if (!_actionBindings.Actions.ContainsKey(key))
            {
                _actionBindings.Actions[key] = new Dictionary<ActionBindings.KeyEvent, string>();
            }
        }
    }
}
