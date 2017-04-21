using System;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler
{
    public class GeneralAnimation
    {
        public bool IsDone = false;

        //Animation takes one parameter <CurrentTimeInMilliseconds>
        public Action<double> Animation { get; set; }

        public bool Loop { get; set; } = false;

        //When animation is unique, no second animation of same AnimationId may run on entity
        public bool Unique { get; set; } = false;

        public string AnimationType { get; set; } = "";

        public double Length { get; set; } = 0;

        public double StartOfAnimation { get; set; } = 0;
    }
}