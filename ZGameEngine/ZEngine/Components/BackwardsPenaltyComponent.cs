namespace ZEngine.Components
{
    public class BackwardsPenaltyComponent : IComponent
    {
        public double AccelerationBeforeBackwardsPenaltyApplied { get; set; }
        public double BackwardsPenaltyFactor { get; set; }

        public BackwardsPenaltyComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            AccelerationBeforeBackwardsPenaltyApplied = 0;
            BackwardsPenaltyFactor = 0.5;
            return this;
        }
    }
}