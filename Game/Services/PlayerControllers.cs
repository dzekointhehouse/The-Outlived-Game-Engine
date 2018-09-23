using System;
using System.Collections.Generic;

namespace Game.Services
{
    public class PlayerControllers
    {
        public const int MAX_PLAYERS = 4;
        public List<VirtualGamePad> Controllers;

        public PlayerControllers(VirtualGamePad[] controllers)
        {
            if (controllers.Length > MAX_PLAYERS)
            {
                throw new Exception("So much players are not supported at this time.");
            }
            if (controllers.Length < 1)
            {
                throw new Exception("At least one player is needed [PlayerVirtualInputCollection]");
            }

            Controllers = new List<VirtualGamePad>(controllers);
        }

        public VirtualGamePad PlayerOne()
        {
            return Controllers[0];
        }
    }
}