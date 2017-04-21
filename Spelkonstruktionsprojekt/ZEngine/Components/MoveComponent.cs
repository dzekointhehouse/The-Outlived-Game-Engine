using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Wrappers;

namespace ZEngine.Components
{
    public class MoveComponent : IComponent
    {
        public Vector2 PreviousPosition { get; set; }
        public float Direction { get; set; } = 0;
        public float Speed = 0;
        public double AccelerationSpeed = 10;
        public double BackwardsPenaltyFactor = 0.5;
        public float MaxVelocitySpeed = 0;
        public double RotationMomentum = 0;
        public double RotationSpeed = 0;
    }
}