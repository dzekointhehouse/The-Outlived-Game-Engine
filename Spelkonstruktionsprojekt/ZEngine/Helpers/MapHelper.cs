using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    public class MapHelper
    {
        private readonly Dictionary<int, string> tileTypes;

        // the user defines which tiles to be used in the map 
        // here, the key will be used as the position in the matrix
        // that is given in the CreateMap method.
        public MapHelper(Dictionary<int, string> tileTypes)
        {
            this.tileTypes = tileTypes;
        }

        // This method takes a matrix that represents a map and
        // the size the tiles should be, then it creates all the entities
        // for the map, by using the strings in the dictionary specified earlier
        // to know which texture to use where.
        public void CreateMapTiles(int[,] map, int size)
        {
            // Gets the number of values in the specified dimension.
            for (var x = 0; x < map.GetLength(1); x++)
            {
                // Gets the number of values in the specified dimension.
                for (var y = 0; y < map.GetLength(0); y++)
                {
                    //
                    int positionNumber = map[y, x];

                    // where to place it.
                    var position = new PositionComponent() { Position = new Vector2(x * size, y * size), ZIndex = 1 };


                    var renderComponent = new RenderComponent()
                    {
                        DimensionsComponent = new DimensionsComponent() { Height = size, Width = size }
                    };

                    // We use the positionNumber from the map in the dictionary so
                    // we can find which tile to use there that the user specifies.
                    var spriteComponent = new SpriteComponent() { SpriteName = tileTypes[positionNumber] };

                    var id = EntityManager.GetEntityManager().NewEntity();
                    ComponentManager.Instance.AddComponentToEntity(position, id);
                    ComponentManager.Instance.AddComponentToEntity(renderComponent, id);
                    ComponentManager.Instance.AddComponentToEntity(spriteComponent, id);
                }
            }

        }
    }
}