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
        public Vector2 spawnPositionOne;
        public Vector2 spawnPositionTwo;
        public Vector2 spawnPositionThree;
        public Vector2 spawnPositionFour;

        // TODO: Spawn points for players

        public void SetupMap(GameConfig config)
        {
            MapHelper mapHelper;
            var tileTypes = new Dictionary<int, string>();
            switch (config.GameMode)
            {
                case GameModes.Survival:
                    tileTypes.Clear();
                    int[,] map = new int[40, 40];
                    tileTypes.Add(0, "grass");
                    mapHelper = new MapHelper(tileTypes);
                    mapHelper.CreateMapTiles(map, 128);
                    spawnPositionOne = new Vector2(2500, 2400);
                    spawnPositionTwo = new Vector2(2450, 2520);
                    spawnPositionThree = new Vector2(2300, 2300);
                    spawnPositionFour = new Vector2(2300, 2450);
                    break;
                case GameModes.Extinction:
                    tileTypes.Clear();
                    tileTypes.Add(2, "yellowwall64");
                    tileTypes.Add(28, "grass");
                    mapHelper = new MapHelper(tileTypes);
                    mapHelper.AddNumberToCollisionList(2);
                    mapHelper.CreateMapTiles(MapPack.TheWallMap, 100);
                    spawnPositionOne = new Vector2(120, 120);
                    spawnPositionTwo = new Vector2(110, 150);
                    spawnPositionThree = new Vector2(1840, 1840);
                    spawnPositionFour = new Vector2(1760, 1840);
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
                    spawnPositionOne = new Vector2(150, 150);
                    spawnPositionTwo = new Vector2(5800, 3600);
                    spawnPositionThree = new Vector2(350, 150);
                    spawnPositionFour = new Vector2(5600, 3600);
                    break;
            }
        }
    }
}
