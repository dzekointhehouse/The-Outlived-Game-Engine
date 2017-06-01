using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class GameScoreComponent : IComponent
    {
        //public int ElapsedSecondsLimit { get; set; } = 10;

        //public int PointsPerSecondsLimit { get; set; }

        public int TotalGameScore { get; set; }
        public int KillsTeamOne { get; set; } = 0;
        public int KillsTeamTwo { get; set; } = 0;


        public double multiplier;
        public double survivalScoreFactor;
        public int damageScore;
        public int damagePenalty;

        public GameScoreComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            TotalGameScore = 0;
            multiplier = 1;
            survivalScoreFactor = 5;
            damageScore = 50;
            damagePenalty = -500;
            return this;
        }
    }
}
