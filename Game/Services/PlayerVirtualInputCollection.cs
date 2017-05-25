using System;

namespace Game.Services
{
    public class PlayerVirtualInputCollection
    {
        public const int MAX_PLAYERS = 4;
        public VirtualGamePad[] VirtualGamePads;

        public PlayerVirtualInputCollection(VirtualGamePad[] virtualGamePads)
        {
            if (virtualGamePads.Length > MAX_PLAYERS)
            {
                throw new Exception("So much players are not supported at this time.");
            }
            if (virtualGamePads.Length < 1)
            {
                throw new Exception("At least one player is needed [PlayerVirtualInputCollection]");
            }

            VirtualGamePads = virtualGamePads;
        }

        public VirtualGamePad PlayerOne()
        {
            return VirtualGamePads[0];
        }
    }
}