using Microsoft.Xna.Framework;

namespace ZEngine.Components
{
    public class MoveComponent : IComponent
    {
        public Vector2 PreviousPosition { get; set; }
        public float PreviousDirection { get; set; }
        public float Direction { get; set; }
        public Vector2 GrandPreviousPosition { get; internal set; }

        public float Speed;
        public double CurrentAcceleration;
        public double AccelerationSpeed = 10;
        public float MaxVelocitySpeed;
        public double RotationMomentum;
        public double RotationSpeed;

        public MoveComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            PreviousPosition = Vector2.Zero;
            PreviousDirection = 0;
            Direction = 0;
            Speed = 0;
            CurrentAcceleration = 0;
            AccelerationSpeed = 10;
            MaxVelocitySpeed = 0;
            RotationMomentum = 0;
            RotationSpeed = 0;
            return this;
        }
    }
}