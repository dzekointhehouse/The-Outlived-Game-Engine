using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Wrappers.CollisionShapes
{
    public class BoundingCircle : CollisionShape
    {
        public BoundingCircle()
        {
            base.Type = VolumeType.Circle;
            base.BoundingCircle = new BoundingSphere();
        }

        public override Texture2D GetCollisionBorderTexture(GraphicsDevice graphics, int width, int height)
        {
            var colours = new List<Color>();
            var center = new Vector2((width - 1) / 2, (height - 1) / 2);
            var radius = width / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pos = new Vector2(x, y);
                    var dist = Vector2.Distance(pos, center);

                    if (dist == radius)
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
            return texture;
        }

        public override bool Intersects(CollisionShape shape)
        {
            switch (shape.Type)
            {
                case VolumeType.Rectangle:
                    return BoundingCircle.Intersects(shape.BoundingRectangle);
                case VolumeType.Circle:
                    return BoundingCircle.Intersects(shape.BoundingCircle);
                default:
                    return false;
            }
        }

        public override void UpdateVolume(uint id)
        {
            var position =
                 ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(id);

            var dimension = ComponentManager.Instance.GetEntityComponentOrDefault<DimensionsComponent>(id);

            int radius = (int)(dimension.Width * 0.5f);
            var x = position.Position.X + dimension.Width - radius;
            var y = position.Position.Y + dimension.Width - radius;

            BoundingCircle = new BoundingSphere(new Vector3(x, y, 0), radius);
        }
    }
}
