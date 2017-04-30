namespace ZEngine.Components
{
    public class BackwardsPenaltyComponent : IComponent
    {
        public double AccelerationBeforeBackwardsPenaltyApplied { get; set; } = 0;
        public double BackwardsPenaltyFactor { get; set; } = 0.5;
    }
}