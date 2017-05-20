using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;

namespace Spelkonstruktionsprojekt.ZEngine.Constants
{
    public static class GamePadStandardLayout
    {
        public static Dictionary<Buttons, string> Layout = new Dictionary<Buttons, string>
        {
            {Buttons.DPadUp, EventConstants.WalkForward },
            {Buttons.DPadDown, EventConstants.WalkBackward},
            {Buttons.DPadLeft, EventConstants.TurnLeft},
            {Buttons.DPadRight, EventConstants.TurnRight},
            {Buttons.RightTrigger, EventConstants.FirePistolWeapon},
            {Buttons.LeftTrigger, EventConstants.Running},
            {Buttons.A, EventConstants.TurnAround},
            {Buttons.Y, EventConstants.LightStatus}
        };

        //For easy single player demo of gamepad. When GamePadMovementSystem is implemented, this will no longer
        // be needed.
        public static Dictionary<Buttons, Keys> GamePadToKeyboardMap = new Dictionary<Buttons, Keys>
        {
            {Buttons.DPadUp, Keys.W},
            {Buttons.DPadDown, Keys.S},
            {Buttons.DPadLeft, Keys.A},
            {Buttons.DPadRight, Keys.D},
            {Buttons.RightTrigger, Keys.E},
            {Buttons.LeftTrigger, Keys.R},
            {Buttons.A, Keys.Q},
            {Buttons.Y, Keys.LeftShift}
        };
    }
}