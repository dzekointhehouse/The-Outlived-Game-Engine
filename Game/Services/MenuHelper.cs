using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Services
{
    public class MenuHelper
    {
       // private GameManager gm;
        //private bool moveRight = false;
       //public MenuHelper(GameManager gm)
       //{
       //    this.gm = gm;
       //}
        public static void DrawBackgroundWithScaling(SpriteBatch sb, MenuContent menuContent, float speed)
        {
            if (GameManager.Scale <= GameManager.MaxScale && GameManager.Scale >= GameManager.MinScale)
            {
                if (GameManager.Scale <= GameManager.MinScale + 0.1)
                {
                    GameManager.MoveHigher = true;
                }
                if (GameManager.Scale >= GameManager.MaxScale - 0.1)
                {
                    GameManager.MoveHigher = false;
                }

                if (GameManager.MoveHigher)
                {
                    GameManager.Scale = GameManager.Scale + speed;
                }
                else
                {
                    GameManager.Scale = GameManager.Scale - speed;
                }
            }

            sb.Draw(
                texture: menuContent.Background,
                position: Vector2.Zero,
                color: Color.White,
                scale: new Vector2(GameManager.Scale)
            );
        }

        //public static void DrawBackgroundMovingSideways(SpriteBatch sb, GameContent gameContent, Viewport viewport, float speed)
        //{
        //    var bounds = gameContent.BackgroundFog.Bounds;
        //   // var viewport = gm.Engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
        //    Vector2 position = Vector2.Zero;
        //    Vector2 Offset = Vector2.Zero;

        //    if (GameManager.Scale <= GameManager.MaxScale && GameManager.Scale >= GameManager.MinScale)
        //    {
        //        if (viewport.X <= bounds.X)
        //        {
        //            GameManager.MoveRight = true;
        //        }
        //        if (viewport.X >= viewport.Width - bounds.Width)
        //        {
        //            GameManager.MoveRight = false;
        //        }

        //        if (GameManager.MoveRight)
        //        {
        //            //Calculate the distance to move our image, based on speed
        //            Vector2 distance = direction * Speed * elapsed;

        //            //Update our offset
        //            Offset += distance;
        //            position = new Vector2(bounds.X - speed);
        //        }
        //        else
        //        {
        //            position = new Vector2(bounds.X + speed);
        //        }
        //    }

        //    sb.Draw(gameContent.BackgroundFog, new Vector2(viewport.X, viewport.Y), Rectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        //}

        public class Background
        {
            private Texture2D Texture;      //The image to use
            private Vector2 Offset;         //Offset to start drawing our image
            public Vector2 Speed;           //Speed of movement of our parallax effect
            public float Zoom;              //Zoom level of our image

            private Viewport Viewport;      //Our game viewport

            //Calculate Rectangle dimensions, based on offset/viewport/zoom values
            private Rectangle Rectangle
            {
                get { return new Rectangle((int)(Offset.X), (int)(Offset.Y), (int)(Viewport.Width / Zoom), (int)(Viewport.Height / Zoom)); }
            }

            public Background(Texture2D texture, Vector2 speed, float zoom)
            {
                Texture = texture;
                Offset = Vector2.Zero;
                Speed = speed;
                Zoom = zoom;
            }

            public void Update(GameTime gametime, Vector2 direction, Viewport viewport)
            {
                float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

                //Store the viewport
                Viewport = viewport;

                //Calculate the distance to move our image, based on speed
                Vector2 distance = direction * Speed * elapsed;

                //Update our offset
                Offset += distance;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, new Vector2(Viewport.X, Viewport.Y), Rectangle, Color.White, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);
            }
        }
    }
}