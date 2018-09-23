using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Menu;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Helpers.DefaultMaps;
using TheOutlivedGL;
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
                case OutlivedStates.GameState.SurvivalGame:
                    tileTypes.Clear();
                    int[,] map = new int[40, 40];
                    tileTypes.Add(0, "grass");
                    mapHelper = new MapHelper(tileTypes);
                    mapHelper.CreateMap(map, 128);
                    spawnPositionOne = new Vector2(2500, 2400);
                    spawnPositionTwo = new Vector2(2450, 2520);
                    spawnPositionThree = new Vector2(2300, 2300);
                    spawnPositionFour = new Vector2(2300, 2450);
                    break;
                case OutlivedStates.GameState.PlayExtinctionGame:
                    //tileTypes.Clear();
                    //tileTypes.Add(2, "yellowwall64");
                    //tileTypes.Add(28, "grass");
                    //mapHelper = new MapHelper(tileTypes);
                    //mapHelper.AddNumberToCollisionList(2);
                    //mapHelper.CreateMap(MapPack.TheWallMap, 100);
                    //spawnPositionOne = new Vector2(120, 120);
                    //spawnPositionTwo = new Vector2(110, 150);
                    //spawnPositionThree = new Vector2(1840, 1840);
                    //spawnPositionFour = new Vector2(1760, 1840);

                    tileTypes.Clear();

                    tileTypes.Add(35, "Images/InGame/Road/blankroad");
                    tileTypes.Add(36, "Images/InGame/Road/leftsideroad");
                    tileTypes.Add(37, "Images/InGame/Road/pavementleft");
                    tileTypes.Add(38, "Images/InGame/Road/pavementright");
                    tileTypes.Add(39, "Images/InGame/Road/rightsideroad");
                    tileTypes.Add(40, "Images/InGame/Objects/barrel");
                    tileTypes.Add(42, "Images/InGame/Objects/market");
                    tileTypes.Add(43, "Images/InGame/Objects/market2");
                    tileTypes.Add(16, "Images/InGame/Tiles/tile2");


                    tileTypes.Add(1, "Images/InGame/Tiles/House1/cornerlefttop");
                    tileTypes.Add(2, "Images/InGame/Tiles/House1/housecorner");
                    tileTypes.Add(3, "Images/InGame/Tiles/House1/housecornerrighttop");
                    tileTypes.Add(4, "Images/InGame/Tiles/House1/housesidebottom");
                    tileTypes.Add(5, "Images/InGame/Tiles/House1/housesideleft");
                    tileTypes.Add(6, "Images/InGame/Tiles/House1/housesideright");
                    tileTypes.Add(7, "Images/InGame/Tiles/House1/sidetop");
                    tileTypes.Add(8, "Images/InGame/Tiles/House1/center");
                    tileTypes.Add(9, "Images/InGame/Tiles/House1/leftbottomcorner");

                    var mapImage = OutlivedGame.Instance().Get<Texture2D>("Images/Blockworld");
                    mapHelper = new MapHelper(tileTypes);
                    mapHelper.AddNumbersToHullList(1,2,3,4,5,6,7,8,9);
                    mapHelper.AddNumbersToCollisionList(40, 42, 43, 1, 2, 3, 4, 5, 6, 7, 9);
                    mapHelper.CreateMap(MapPack.TheCity, 64, mapImage);
                    spawnPositionOne = new Vector2(120, 120);
                    spawnPositionTwo = new Vector2(110, 150);
                    spawnPositionThree = new Vector2(1840, 1840);
                    spawnPositionFour = new Vector2(1760, 1840);



                    break;
                case OutlivedStates.GameState.PlayDeathMatchGame:
                    tileTypes.Clear();
                    tileTypes.Add(4, "yellowwall64");
                    tileTypes.Add(2, "blue64");
                    tileTypes.Add(0, "red64");
                    tileTypes.Add(1, "green64");
                    var mapImage2 = OutlivedGame.Instance().Get<Texture2D>("Images/Blockworld");
                    mapHelper = new MapHelper(tileTypes);
                    mapHelper.AddNumberToCollisionList(4);
                    mapHelper.CreateMap(MapPack.Blockworld2, 64, mapImage2);
                    spawnPositionOne = new Vector2(150, 150);
                    spawnPositionTwo = new Vector2(5800, 3600);
                    spawnPositionThree = new Vector2(350, 150);
                    spawnPositionFour = new Vector2(5600, 3600);
                    break;
            }
        }
    }
}
