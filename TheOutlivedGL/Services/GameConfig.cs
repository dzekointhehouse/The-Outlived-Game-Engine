using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Game.Menu.OutlivedStates;
using static Game.Menu.States.CharacterMenu;
using static Game.Menu.States.MultiplayerMenu;

namespace Game.Services
{
    public class GameConfig
    {
        public GameState GameMode { get; set; } = GameState.Quit;

        public List<Player> Players { get; set; } = new List<Player>();

        public void Reset()
        {
            Players.Clear();
            Players = new List<Player>();
            GameMode = GameState.Quit;
        }
    }

    public class Player
    {
        public TeamStates Team { get; set; }
        public PlayerIndex Index { get; set; }
        public string SpriteName { get; set; }
        public CharacterType CharacterType { get; set; }
        public int CameraId { get; set; }
    }
}
