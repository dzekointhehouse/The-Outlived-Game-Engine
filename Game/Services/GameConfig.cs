using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static Game.Menu.States.CharacterMenu;
using static Game.Menu.States.MultiplayerMenu;

namespace Game.Services
{
    public class GameConfig
    {
        public enum GameModes
        {
            Survival,
            Extinction,
            Blockworld
        }
        public GameModes GameMode { get; set; }

        //public List<Player> GamePlayers { get; set; } = new List<Player>(4);
        //public List<Player> TeamTwo { get; set; } = new List<Player>(4);
        public List<Player> Players { get; set; } = new List<Player>(4);
    }

    public class Player
    {
        public TeamState Team { get; set; }
        public PlayerIndex Index { get; set; }
        public CharacterState Character { get; set; }
    }
}
