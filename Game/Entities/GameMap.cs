using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Helpers.DefaultMaps;
using static Game.Menu.States.GameModeMenu;
using static Game.Services.GameConfig;

namespace Game.Entities
{
    public class GameMap
    {
        private Vector2 spawnPositionOne = new Vector2(50, 50);
        private Vector2 spawnPositionTwo = new Vector2(70, 70);
        private Vector2 spawnPositionThree = new Vector2(80, 80);
        private Vector2 spawnPositionFour = new Vector2(30, 30);

        // TODO: Spawn points for players

        public void SetupMap(GameConfig config)
        {
            MapHelper mapHelper;
            var tileTypes = new Dictionary<int, string>();
            switch (config.GameMode)
            {
                case GameModes.Survival:
                    tileTypes.Clear();
                    int[,] map = new int[256, 256];
                    tileTypes.Add(0, "grass");
                    mapHelper = new MapHelper(tileTypes);
                    mapHelper.CreateMapTiles(map, 32);
                    break;
                case GameModes.Extinction:
                    tileTypes.Clear();
                    tileTypes.Add(2, "yellowwall64");
                    tileTypes.Add(28, "grass");
                    mapHelper = new MapHelper(tileTypes);
                    mapHelper.AddNumberToCollisionList(2);
                    mapHelper.CreateMapTiles(MapPack.TheWallMap, 100);
                    break;
                case GameModes.Blockworld:
                    tileTypes.Clear();
                    tileTypes.Add(4, "yellowwall64");
                    tileTypes.Add(2, "blue64");
                    tileTypes.Add(0, "red64");
                    tileTypes.Add(1, "green64");

                    mapHelper = new MapHelper(tileTypes);
                    mapHelper.AddNumberToCollisionList(4);
                    mapHelper.CreateMapTiles(MapPack.Blockworld2, 64);
                    break;
            }
        }
    }
}
