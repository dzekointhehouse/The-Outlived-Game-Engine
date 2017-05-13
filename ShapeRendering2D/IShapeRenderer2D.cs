using Microsoft.Xna.Framework;

namespace ShapeRendering2D
{
    public interface IShapeRenderer2D
    {
        void AddLine(Vector3 a, Vector3 b, Color color, float life);
        void AddBoundingRectangle(Rectangle rectangle, Color color, float life);
        void AddBoundingSphere(BoundingSphere sphere, Color color, float life);
    }
}
