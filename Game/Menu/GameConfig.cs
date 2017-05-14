using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game.Menu
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

        public List<PlayerIndex> TeamOne { get; set; } = new List<PlayerIndex>(4);
        public List<PlayerIndex> TeamTwo { get; set; } = new List<PlayerIndex>(4);


    }


}
