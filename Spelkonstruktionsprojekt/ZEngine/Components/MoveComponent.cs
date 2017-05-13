using Microsoft.Xna.Framework;

namespace ZEngine.Components
{
    public class MoveComponent : IComponent
    {
        public Vector2 PreviousPosition { get; set; }
        public float PreviousDirection { get; set; }
        public float Direction { get; set; } = 0;
        public float Speed = 0;
        public double CurrentAcceleration = 0;
        public double AccelerationSpeed = 10;
        public double BackwardsPenaltyFactor = 0.5;
        public float MaxVelocitySpeed = 0;
        public double RotationMomentum = 0;
        public double RotationSpeed = 0;
    }
}