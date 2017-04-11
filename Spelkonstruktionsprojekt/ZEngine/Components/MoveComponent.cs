using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ZEngine.Wrappers;

namespace ZEngine.Components.MoveComponent
{
    public class MoveComponent : IComponent
    {
        public double RestingDirection { get; set; } = 0;
        public Nullable<Vector2> Velocity { get; set; } = null;
        public Nullable<Vector2> MinVelocity { get; set; } = null;
        public Nullable<Vector2> MaxVelocity { get; set; } = null;

        public Nullable<Vector2> Acceleration { get; set; } = null;
        public Nullable<Vector2> MinAcceleration { get; set; } = null;
        public Nullable<Vector2> MaxAcceleration { get; set; } = null;

        public double RotationSpeed = 0;
    }
}
