using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Wrappers;

namespace ZEngine.Components.MoveComponent
{
    public class MoveComponent : IComponent
    {
        public double Direction { get; set; } = 0;
        public Vector2D Velocity { get; set; } = null;
        public Vector2D MinVelocity { get; set; } = null;
        public Vector2D MaxVelocity { get; set; } = null;

        public Vector2D Acceleration { get; set; } = null;
        public Vector2D MinAcceleration { get; set; } = null;
        public Vector2D MaxAcceleration { get; set; } = null;

        public double RotationSpeed = 0;
    }

    public static class MoveComponentHelper
    {
        /**
         *  The originVector is the current vector and the deltaVector is the change you want
         *  to impose on the originVector. The returned vector is the new translated vector.
         *  It is returned since a Vector2 cannot be changed (it is readonly).
         */
        public static Vector2D Translate(Vector2D originVector, Vector2D deltaVector)
        {
            return Vector2D.Create(
                originVector.X + deltaVector.X,
                originVector.Y + deltaVector.Y
            );
        }

        public static bool SomeAxisBelowMovingThreshold(Vector2D originVector)
        {
            return AxisBelowMovingThreshold(originVector.X) || AxisBelowMovingThreshold(originVector.Y);
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
