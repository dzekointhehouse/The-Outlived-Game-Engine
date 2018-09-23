using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Wrappers.CollisionShapes
{
    public class BoundingRectangle : CollisionShape
    {
        public BoundingRectangle()
        {
            base.Type = VolumeType.Rectangle;
            base.BoundingRectangle = new BoundingBox();
            base.CollisionTexture = null;
        }


        public override void GetCollisionBorderTexture(GraphicsDevice graphics, int width, int height)
        {
            if(CollisionTexture == null)
            {
                var colours = new List<Color>();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        {
                            colours.Add(new Color(255, 255, 255, 255));
                        }
                        else
                        {
                            colours.Add(new Color(0, 0, 0, 0));
                        }
                    }
                }

                var texture = new Texture2D(graphics, width, height);
                texture.SetData<Color>(colours.ToArray());
                CollisionTexture = texture;
            }

        }

        public override bool Intersects(CollisionShape shape)
        {
            switch (shape.Type)
            {
                case VolumeType.Rectangle:
                    return BoundingRectangle.Intersects(shape.BoundingRectangle);
                case VolumeType.Circle:
                    return BoundingRectangle.Intersects(shape.BoundingCircle);
                default:
                    return false;
            }
        }

        public override void UpdateVolume(uint id)
        {
            var position =
                ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(id);

            var dimension = ComponentManager.Instance.GetEntityComponentOrDefault<DimensionsComponent>(id);

            BoundingRectangle = new BoundingBox(
                new Vector3(position.Position.X, position.Position.Y, 0),
                new Vector3(position.Position.X + dimension.Width, position.Position.Y + dimension.Height, 0));
        }
    }
}
