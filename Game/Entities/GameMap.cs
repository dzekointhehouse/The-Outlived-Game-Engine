using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Helpers.DefaultMaps;

namespace Game.Entities
{
    public class GameMap
    {



        public void SetupGameMap(MapType map)
        {
            var tileTypes = new Dictionary<int, string>();
            //tileTypes.Add(0, "blue64");
            tileTypes.Add(0, "grass");
            tileTypes.Add(1, "grass");
            tileTypes.Add(2, "grass");
            tileTypes.Add(4, "grass");
            //tileTypes.Add(2, "red64");
            //tileTypes.Add(4, "yellowwall64");
            MapHelper mapcreator = new MapHelper(tileTypes);
            mapcreator.CreateMapTiles(MapPack.Blockworld, 100);

            switch (map)
            {
                    case MapType.BlockWorld:

                        break;
            }




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
