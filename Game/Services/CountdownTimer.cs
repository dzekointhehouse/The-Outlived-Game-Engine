using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Game.Services
{
    public class CountdownTimer
    {
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }
        public bool IsDone { get; private set; } = false;

        // Private variables for reset and ticks
        // and stopping timer
        private int resetMinutes;
        private int resetSeconds;
        private double TimeSinceLastTick;
        private bool isStopped = false;


        public CountdownTimer(int minutes)
        {
            this.Minutes = minutes;

            resetMinutes = minutes;
            resetSeconds = 0;
        }

        public CountdownTimer(int minutes, int seconds)
        {
            this.Minutes = minutes;
            this.Seconds = seconds;

            resetMinutes = minutes;
            resetSeconds = seconds;
        }

        public void UpdateTimer(GameTime gameTime)
        {
            if (Minutes <= 0 && Seconds <= 0)
            {
                isStopped = true;
                IsDone = true;
            }

            if (isStopped) return;

            TimeSinceLastTick += gameTime.ElapsedGameTime.TotalSeconds;

            if (TimeSinceLastTick > 1.0f)
            {
                TimeSinceLastTick = 0;

                if (Seconds == 0)
                {
                    Minutes--;
                    Seconds = 59;
                }
                else
                {
                    Seconds--;
                }
            }
        }

        public string GetFormatedTime()
        {
            if(Seconds >= 10) return String.Format("{0}:{1}", Minutes, Seconds);
            else return String.Format("{0}:0{1}", Minutes, Seconds);
        }
        public void Count()
        {
            isStopped = false;
        }
        public void Reset()
        {
            IsDone = false;
            Minutes = resetMinutes;
            Seconds = resetSeconds;
            TimeSinceLastTick = 0;
        }
    }
}


