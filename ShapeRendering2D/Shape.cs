using Microsoft.Xna.Framework.Graphics;

namespace ShapeRendering2D
{
    public class Shape
    {
        /// <summary>
        /// The array of vertices the shape can use.
        /// </summary>
        public VertexPositionColor[] Vertices;

        /// <summary>
        /// The number of lines to draw for this shape.
        /// </summary>
        public int LineCount;

        /// <summary>
        /// The length of time to keep this shape visible.
        /// </summary>
        public float Lifetime;

        public Shape(int LineCount, float LifeTime)
        {
            this.Vertices = new VertexPositionColor[LineCount * 2];
            this.LineCount = LineCount;
            this.Lifetime = LifeTime;
        }
    }
}
