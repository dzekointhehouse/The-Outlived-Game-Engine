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
            {Buttons.A, EventConstants.TurnAround},
            {Buttons.X, EventConstants.Running},
            {Buttons.Y, EventConstants.LightStatus},
            {Buttons.B, EventConstants.ReloadWeapon}
        };
    }
}