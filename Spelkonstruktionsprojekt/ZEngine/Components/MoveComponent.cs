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

namespace ZEngine.Components.MoveComponent
{
    public class MoveComponent : IComponent
    {
        public Vector2 PreviousPosition { get; set; }
        public double Direction { get; set; } = 0;

        public Vector2D Velocity { get; set; } = null;
        public Vector2D MaxVelocity { get; set; } = Vector2D.Create(0,0);

        public Vector2D Acceleration { get; set; } = null;
        public Vector2D MaxAcceleration { get; set; } = null;
        public double BackwardsPenaltyFactor = 0.5;
        public double VelocitySpeed = 0;
        public double MaxVelocitySpeed = 10;
        public double AccelerationSpeed = 10;
        public double RotationMomentum = 0;
        public double RotationSpeed = 0;
    }

    public class VectorHelper
    {
        public static void ApplyVelocitySpeedToLimit(MoveComponent moveComponent, double speedLimit)
        {
            if (moveComponent.VelocitySpeed > speedLimit)
            {
                moveComponent.VelocitySpeed = speedLimit;
            }
            else if (moveComponent.VelocitySpeed < -speedLimit)
            {
                moveComponent.VelocitySpeed = -speedLimit;
            }
        }

        public static void StopAxesAtSpeedLimit(Vector2D originVector, Vector2D maxLimimt)
        {
            if (originVector.X > maxLimimt.X) originVector.X = maxLimimt.X;
            if (originVector.Y > maxLimimt.Y) originVector.Y = maxLimimt.Y;
            if (originVector.X < -maxLimimt.X) originVector.X = -maxLimimt.X;
            if (originVector.Y < -maxLimimt.Y) originVector.Y = -maxLimimt.Y;
        }

        public static bool SomeAxisBelowMovingThreshold(Vector2D originVector)
        {
            return AxisBelowMovingThreshold(originVector.X) || AxisBelowMovingThreshold(originVector.Y);
        }

        public static Vector2D Translate(Vector2D originVector, Vector2D deltaVector)
        {
            return Vector2D.Create(
                originVector.X + deltaVector.X,
                originVector.Y + deltaVector.Y
            );
        }

        public static bool AxisBelowMovingThreshold(double axis)
        {
            return axis < 0.0001 && axis > -0.0001;
        }

        public static void SetVelocityToRest(MoveComponent moveComponent)
        {
            moveComponent.Velocity = StopMotionOnAxesBelowMovingThreshold(moveComponent.Velocity);
        }

        public static void SetAccelerationToRest(MoveComponent moveComponent)
        {
            moveComponent.Acceleration = StopMotionOnAxesBelowMovingThreshold(moveComponent.Acceleration);
        }

        public static double GetAngleFromVector(Vector2D vector)
        {
            return Math.Atan2(vector.Y, vector.X);
        }

        public static Vector2D StopMotionOnAxesBelowMovingThreshold(Vector2D originVector)
        {
            var x = originVector.X;
            var y = originVector.Y;
            if (AxisBelowMovingThreshold(originVector.X))
            {
                x = 0;
            }
            if (AxisBelowMovingThreshold(originVector.Y))
            {
                y = 0;
            }

            return Vector2D.Create(x, y);
        }

        public static bool VectorResting(Vector2D vector)
        {
            double TOLERANCE = 0.000001;
            double NEG_TOLERANCE = 0.000001;
            return Math.Abs(vector.X) < TOLERANCE || Math.Abs(vector.X) > NEG_TOLERANCE 
                   && Math.Abs(vector.Y) < TOLERANCE || Math.Abs(vector.Y) > NEG_TOLERANCE;
        }
    }
}
