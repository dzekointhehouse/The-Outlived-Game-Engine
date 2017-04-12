using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZEngine.Components.CollisionComponent
{
    class CollisionComponent : IComponent
    {
        private Vector2 spritePosition;
        private Rectangle spriteBoundingRectangle;
        private BoundingSphere spriteBoundingSphere;
        private const int spriteMoveSpeed = 5;
        private Vector2 spriteOrigin;
        private Vector3 spriteCenter;
        private float spriteRadius;
        private Boolean isCollidable;
    }
}
