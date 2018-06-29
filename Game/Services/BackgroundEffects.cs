using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Game.Services
{
    public class BackgroundEffects
    {
        private Viewport v;

        public float MaxScale { get; set; } = 2f;
        public float MinScale { get; set; } = 1f;
        public float Scale { get; set; } = 1f;
        public bool ExpandImage { get; set; }

        public BackgroundEffects(Viewport v)
        {
            this.v = v;
        }

        public void DrawExpandingEffect(SpriteBatch sb, Texture2D image, float speed = 0.01f)
        {
            if (Scale <= MaxScale && Scale >= MinScale)
            {
                if (Scale <= MinScale + 0.1)
                {
                    ExpandImage = true;
                }
                if (Scale >= MaxScale - 0.1)
                {
                    ExpandImage = false;
                }

                if (ExpandImage)
                {
                    Scale = Scale + speed;
                }
                else
                {
                    Scale = Scale - speed;
                }
            }
            Debug.WriteLine(Scale);
            sb.Draw(image, ResizeImage(Scale), Color.White);
        }

        private Rectangle ResizeImage(float scale)
        {
            var size = v.TitleSafeArea;

            return new Rectangle(
                (int)(size.X / scale),
                (int)(size.Y / scale),
                (int)(size.Width * scale),
                (int)(size.Height * scale));
        }
    }
    public class SidewaysBackground
    {
        private Texture2D Texture;      //The image to use
        private Vector2 Offset;         //Offset to start drawing our image
        private bool moveRight = true;
        public Vector2 Speed;           //Speed of movement of our parallax effect
        public float Zoom;              //Zoom level of our image

        private Viewport Viewport;      //Our game viewport

        //Calculate Rectangle dimensions, based on offset/viewport/zoom values
        private Rectangle Rectangle
        {
            get { return new Rectangle((int)(Offset.X), (int)(Offset.Y), (int)(Viewport.Width / Zoom), (int)(Viewport.Height / Zoom)); }
        }

        public SidewaysBackground(Texture2D texture, Vector2 speed, float zoom)
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
                moveRight = true; // viewport to move right because its X is smaller than image
            }
            if (bounds.Width <= Offset.X + viewport.Width)
            {
                moveRight = false;
            }

            if (moveRight)
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