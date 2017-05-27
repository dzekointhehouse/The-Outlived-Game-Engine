using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Components;

namespace Game
{
    class Timer
    {
        public bool IsCounting { get; set; } = true;

        private SoundEffectInstance currentSound;
        private string message;
        private int whenToStop;
        private int counter = 3;
        private double timeSincelastCount;
        private SpriteFont font;
        private Viewport viewport;
        private float alpha = 1f;
        private Dictionary<int, SoundEffectInstance> counterSounds;
        public Timer(int whenToStop, SpriteFont font, Viewport viewport)
        {
            this.whenToStop = whenToStop;
            this.font = font;
            this.viewport = viewport;
            counterSounds = new Dictionary<int, SoundEffectInstance>(4);

            counterSounds.Add(1, OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/321Play/One")
                .CreateInstance());
            counterSounds.Add(2, OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/321Play/Two")
                .CreateInstance());
            counterSounds.Add(3, OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/321Play/Three")
                .CreateInstance());
            counterSounds.Add(0, OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/321Play/Play")
                .CreateInstance());
        }

        public void Update(GameTime gameTime)
        {
            if (IsCounting)
            {
                timeSincelastCount += gameTime.ElapsedGameTime.TotalSeconds;
                counterSounds.TryGetValue(counter, out currentSound);
                if (timeSincelastCount > 0.8f)
                {
                    if (currentSound.State != SoundState.Playing)
                    {
                        currentSound.Play();
                    }
                }
                if (timeSincelastCount > 1.5f)
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
                sb.DrawString(font, message, new Vector2(((viewport.Width - font.MeasureString(message).X) * 0.5f), viewport.Y * 0.5f), new Color(255, 255, 255, alpha));
                sb.End();
            }

        }
    }
}
