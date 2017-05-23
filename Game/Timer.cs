using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    class Timer
    {
        private bool isCounting = true;
        private int whenToStop;
        private double timeSincelastCount;
        private SpriteFont font;
        public Timer(int whenToStop, SpriteFont font)
        {
            this.whenToStop = whenToStop;
            this.font = font;
        }

        public void Update(GameTime gameTime)
        {
            if (isCounting)
            {
                timeSincelastCount += gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (timeSincelastCount >= whenToStop)
            {
                isCounting = false;
            }


        }

        public void Draw(SpriteBatch sb)
        {
            if (isCounting)
            {
                sb.Begin();
                sb.DrawString(font, timeSincelastCount.ToString(), new Vector2(50, 50), Color.White);
                sb.End();
            }

        }
    }
}
