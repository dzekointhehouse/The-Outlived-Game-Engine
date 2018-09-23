﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
 using Microsoft.Xna.Framework.Graphics;
 using Penumbra;
 using Spelkonstruktionsprojekt.ZEngine.Components;
 using Spelkonstruktionsprojekt.ZEngine.Managers;
 using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    // Optimus prime
    public class MapHelper
    {
        public Dictionary<int, string> TileTypes { get; set; }
        public List<int> Collisions;
        public  List<int> HullList;

        private static ComponentFactory ComponentFactory = ComponentManager.Instance.ComponentFactory;

        // the user defines which tiles to be used in the MapPack 
        // here, the key will be used as the position in the matrix
        // that is given in the CreateMap method.
        public MapHelper(Dictionary<int, string> tileTypes)
        {
            this.TileTypes = tileTypes;
            Collisions = new List<int>(10);
            HullList = new List<int>(10);
        }


        public void AddNumberToCollisionList(int number)
        {
            Collisions.Add(number);
        }

        public void AddNumbersToCollisionList(params int[] numbers)
        {
            foreach (var number in numbers)
            {
            Collisions.Add(number);

            }
        }

        public void AddNumbersToHullList(params int[] numbers)
        {
            foreach (var number in numbers)
            {
                HullList.Add(number);

            }
        }

        // This method takes a matrix that represents a MapPack and
        // the size the tiles should be, then it creates all the entities
        // for the MapPack, by using the strings in the dictionary specified earlier
        // to know which texture to use where.
        public void CreateMap(int[,] map, int size, Texture2D mapImage = null)
        {
            var height = map.GetLength(0) * size;
            var width = map.GetLength(1) * size;

            Color[,] mapdata = null;

            if (mapImage != null)
            {
                mapdata = MapData(mapImage);
            }

            var worldEntity = EntityManager.GetEntityManager().NewEntity();
            var worldComponent = new WorldComponent() {WorldHeight = height, WorldWidth = width, World = map, WorldData = mapdata};
            ComponentManager.Instance.AddComponentToEntity(worldComponent, worldEntity);

            // Gets the number of values in the specified dimension.
            for (var x = 0; x < map.GetLength(1); x++)
            {
                // Gets the number of values in the specified dimension.
                for (var y = 0; y < map.GetLength(0); y++)
                {
                    //
                    int positionNumber = map[y, x];

                    // where to place it.
                    PositionComponent posComponent = ComponentFactory.NewComponent<PositionComponent>();
                    posComponent.Position = new Vector2(x * size, y * size);
                    posComponent.ZIndex = 1;

                    DimensionsComponent dimensionsComponent = ComponentFactory.NewComponent<DimensionsComponent>();
                    dimensionsComponent.Width = size;
                    dimensionsComponent.Height = size;

                    RenderComponent renderComponent = ComponentFactory.NewComponent<RenderComponent>();
                    renderComponent.IsVisible = true;
                    renderComponent.Fixed = true;

                    // We use the positionNumber from the MapPack in the dictionary so
                    // we can find which tile to use there that the user specifies.
                    SpriteComponent spriteComponent = ComponentFactory.NewComponent<SpriteComponent>();
                    spriteComponent.SpriteName = TileTypes[positionNumber];

                    var id = EntityManager.GetEntityManager().NewEntity();

                    ComponentManager.Instance.AddComponentToEntity(posComponent, id);
                    ComponentManager.Instance.AddComponentToEntity(dimensionsComponent, id);
                    ComponentManager.Instance.AddComponentToEntity(renderComponent, id);
                    ComponentManager.Instance.AddComponentToEntity(spriteComponent, id);

                    // If the position number is contained in our
                    // defined list of which tiles should have the
                    // collision component (for walls or other obstacles)
                    if (Collisions.Contains(positionNumber))
                    {
                        var collision = ComponentFactory.NewComponent<CollisionComponent>();
                        ComponentManager.Instance.AddComponentToEntity(collision, id);

                        ComponentManager.Instance.AddComponentToEntity(new WallComponent(), id);
                    }

                    if (HullList.Contains(positionNumber))
                    {
                        var hull = new Hull(
                            new Vector2(1.0f),
                            new Vector2(-1.0f, 1.0f),
                            new Vector2(-1.0f),
                            new Vector2(1.0f, -1.0f)
                        );
                        hull.Scale = new Vector2(size * 0.5f);
                        hull.Position = new Vector2(x, y);
                        var hullComponent = new HullComponent()
                        {
                            Hull = hull
                        };
                        ComponentManager.Instance.AddComponentToEntity(hullComponent, id);
                    }
                }
            }

        }

        private Color[,] MapData(Texture2D map)
        {
            Color[] rawData = new Color[map.Width * map.Height];
            map.GetData<Color>(rawData);
            

            // Note that this stores the pixel's row in the first index, and the pixel's column in the second,
            // with this setup.
            Color[,] rawDataAsGrid = new Color[map.Height, map.Width];
            for (int row = 0; row < map.Height; row++)
            {
                for (int column = 0; column < map.Width; column++)
                {
                    // Assumes row major ordering of the array.
                    rawDataAsGrid[row, column] = rawData[row * map.Width + column];
                }
            }
            // How to extract:
            // Color color = rawDataAsGrid[7, 10]; // 10th pixel on the 7th row
            return rawDataAsGrid;
        }
    }
}
