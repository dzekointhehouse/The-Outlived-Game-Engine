﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
 using Spelkonstruktionsprojekt.ZEngine.Managers;
 using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    // Optimus prime
    public class MapHelper
    {
        public Dictionary<int, string> TileTypes { get; set; }
        public List<int> Collisions { get; set; } = new List<int>(10);

        // the user defines which tiles to be used in the MapPack 
        // here, the key will be used as the position in the matrix
        // that is given in the CreateMap method.
        public MapHelper(Dictionary<int, string> tileTypes)
        {
            this.TileTypes = tileTypes;
        }


        public void AddNumberToCollisionList(int number)
        {
            Collisions.Add(number);
        }


        // This method takes a matrix that represents a MapPack and
        // the size the tiles should be, then it creates all the entities
        // for the MapPack, by using the strings in the dictionary specified earlier
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
                    PositionComponent position = new PositionComponent{ Position = new Vector2(x * size, y * size), ZIndex = 1 };
                    DimensionsComponent dimensionsComponent = new DimensionsComponent() { Height = size, Width = size };
                    RenderComponent renderComponent = new RenderComponent() { IsVisible = true, Fixed = true};
                

                    // We use the positionNumber from the MapPack in the dictionary so
                    // we can find which tile to use there that the user specifies.
                    SpriteComponent spriteComponent = new SpriteComponent{ SpriteName = TileTypes[positionNumber] };

                    var id = EntityManager.GetEntityManager().NewEntity();

                    ComponentManager.Instance.AddComponentToEntity(position, id);
                    ComponentManager.Instance.AddComponentToEntity(dimensionsComponent, id);
                    ComponentManager.Instance.AddComponentToEntity(renderComponent, id);
                    ComponentManager.Instance.AddComponentToEntity(spriteComponent, id);

                    // If the position number is contained in our
                    // defined list of which tiles should have the
                    // collision component (for walls or other obstacles)
                    if (Collisions.Contains(positionNumber))
                    {
                        var collision = new CollisionComponent();
                        ComponentManager.Instance.AddComponentToEntity(collision, id);
                    }
                }
            }

        }
    }
}