﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static Game.Menu.States.CharacterMenu;
using static Game.Menu.States.GameModeMenu;
using static Game.Menu.States.MultiplayerMenu;

namespace Game.Services
{
    public class GameConfig
    {
        public GameModes GameMode { get; set; } = GameModes.Exit;

        public List<Player> Players { get; set; } = new List<Player>();

        public void Reset()
        {
            Players.Clear();
            Players = new List<Player>();
            GameMode = GameModes.Exit;
        }
    }

    public class Player
    {
        public TeamState Team { get; set; }
        public PlayerIndex Index { get; set; }
        public string SpriteName { get; set; }
        public CharacterType CharacterType { get; set; }
        public int CameraId { get; set; }
    }
}
