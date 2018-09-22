using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelkonstruktionsprojekt.ZEngine.Wrappers
{
    public abstract class CollisionShape
    {
        public enum VolumeType
        {
            Circle,
            Rectangle,
        }

        public BoundingBox BoundingRectangle { get; set; }
        public BoundingSphere BoundingCircle { get; set; }
        public VolumeType Type { get; protected set; }
        public Texture2D CollisionTexture { get; internal set; }

        // public abstract void UpdateVolume(Rectangle rectangle);
        public abstract void UpdateVolume(uint id); 
        public abstract bool Intersects(CollisionShape shape);
        /// <summary>
        /// for debugging
        /// </summary>
        /// <returns></returns>
        public abstract void GetCollisionBorderTexture(GraphicsDevice graphics, int width, int height);
    }
}
