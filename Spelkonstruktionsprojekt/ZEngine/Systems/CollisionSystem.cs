using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Components.CollisionComponent;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class CollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;        

        //public void BindRectangleSprite()
        //{

        //}

        //public void getCollisions()
        //{
        //    var entitylist = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();
            
        //}


        // stops the sprite from going off the screen
        public void Boundering(SpriteComponent sprite, GraphicsDeviceManager graphics)
        {                          
            int x = MathHelper.Clamp(sprite.Position.X, graphics.GraphicsDevice.Viewport.X, graphics.GraphicsDevice.Viewport.Width - sprite.Width);
            int y= MathHelper.Clamp(sprite.Position.Y, graphics.GraphicsDevice.Viewport.Y, graphics.GraphicsDevice.Viewport.Height - sprite.Height);

            sprite.Position = new Point(x, y);
            
        }

        // calculate bounding rectangle of the sprite
        //public void CalculateBoundingRectangle()
        //{

        //}
    }
}
