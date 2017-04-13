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
        public Dictionary<Keys, string> Actions = new Dictionary<Keys, string>();

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

        public ActionBindingsBuilder SetAction(Keys key, string eventName)
        {
            _actionBindings.Actions[key] = eventName;
            return this;
        }

        public ActionBindings Build()
        {
            return _actionBindings;
        }
    }
}
