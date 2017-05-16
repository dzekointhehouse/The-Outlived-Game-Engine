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
                var bounds = Texture.Bounds;

                //Calculate the distance to move our image, based on speed
                Vector2 distance = direction * Speed * elapsed;

                if (viewport.X >= Offset.X)
                {
                    GameManager.MoveRight = true; // viewport to move right because its X is smaller than image
                }
                if (bounds.Width <= Offset.X + viewport.Width)
                {
                    GameManager.MoveRight = false;
                }

                if (GameManager.MoveRight)
                {

                    //Move our offset to the right of the image
                    Offset += distance;
                    
                }
                else
                {
                    //Move our offset to the left of the image
                    Offset -= distance;
                }


            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, new Vector2(Viewport.X, Viewport.Y), Rectangle, Color.White, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);
            }
        }
    }
}