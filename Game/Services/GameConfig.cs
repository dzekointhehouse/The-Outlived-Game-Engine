using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static Game.Menu.States.CharacterMenu;
using static Game.Menu.States.GameModeMenu;
using static Game.Menu.States.MultiplayerMenu;

namespace Game.Services
{
    public class GameConfig
    {
        public GameModes GameMode { get; set; }

        public List<Player> Players { get; set; } = new List<Player>(4);
    }

    public class Player
    {
        public TeamState Team { get; set; }
        public PlayerIndex Index { get; set; }
        public string Character { get; set; }
        public int CameraId { get; set; }
    }
}
