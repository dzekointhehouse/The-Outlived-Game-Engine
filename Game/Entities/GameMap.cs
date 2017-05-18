using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Helpers.DefaultMaps;

namespace Game.Entities
{
    public class GameMap
    {
        private Vector2 spawnPositionOne = new Vector2(50, 50);
        private Vector2 spawnPositionTwo = new Vector2(70, 70);
        private Vector2 spawnPositionThree = new Vector2(80, 80);
        private Vector2 spawnPositionFour = new Vector2(30, 30);



        public void SetupMap(GameConfig config)
        {
            var tileTypes = new Dictionary<int, string>();

            //tileTypes.Add(0, "blue64");
            tileTypes.Add(2, "yellowwall64");
            tileTypes.Add(28, "grass");
            //tileTypes.Add(4, "yellowwall64");


            MapHelper mapcreator = new MapHelper(tileTypes);

            mapcreator.AddNumberToCollisionList(2);

            mapcreator.CreateMapTiles(MapPack.TheWallMap, 100);
        }
    }

    public enum MapType
    {
        BlockWorld,
        Downtown,
        LastSurvivors,
        OnlyGrass
    }

}
