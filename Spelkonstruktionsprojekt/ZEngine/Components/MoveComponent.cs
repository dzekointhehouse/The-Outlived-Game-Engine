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
        public Vector2Component Velocity;
        public Vector2Component MinVelocity;
        public Vector2Component MaxVelocity;

        public Vector2Component Acceleration;
        public Vector2Component MinAcceleration;
        public Vector2Component MaxAcceleration;

        public double RotationSpeed = 0;
    }
}
