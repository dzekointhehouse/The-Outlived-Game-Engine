using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    class Timer
    {
        public bool IsCounting { get; set; }= true;

        private string message;
        private int whenToStop;
        private int counter = 3;
        private double timeSincelastCount;
        private SpriteFont font;
        private Viewport viewport;
        private float alpha = 1f;
        public Timer(int whenToStop, SpriteFont font, Viewport viewport)
        {
            this.whenToStop = whenToStop;
            this.font = font;
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime)
        {
            if (IsCounting)
            {
                timeSincelastCount += gameTime.ElapsedGameTime.TotalSeconds;

                if (timeSincelastCount > 2)
                {
                    timeSincelastCount = 0;
                    counter--;
                    alpha = 1f;

                }
            }
            if (counter == whenToStop - 1)
            {
                IsCounting = false;
            }


        }

        public void Draw(SpriteBatch sb)
        {
            if (IsCounting)
            {
                alpha = alpha - 0.01f;
                if (counter > whenToStop)
                    message = counter.ToString();
                else
                {
                    message = "PLAY!";
                }
                sb.Begin();
                sb.DrawString(font, message, new Vector2(((viewport.Width - font.MeasureString(message).X) * 0.5f), viewport.Y * 0.5f), new Color(255,255,255, alpha));
                sb.End();
            }

        }
    }
}
