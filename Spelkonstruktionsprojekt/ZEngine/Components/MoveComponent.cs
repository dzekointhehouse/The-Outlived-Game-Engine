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
        public Vector2 Velocity;
        public Vector2 MinVelocity;
        public Vector2 MaxVelocity;

        public Vector2 Acceleration;
        public Vector2 MinAcceleration;
        public Vector2 MaxAcceleration;

        public double RotationSpeed = 0;
    }
}
