namespace ZEngine.Components
{
    public class BackwardsPenaltyComponent : IComponent
    {
        public double PreProcessingAcceleration { get; set; } = 0;
        public double BackwardsPenaltyFactor { get; set; } = 0.5;
    }
}