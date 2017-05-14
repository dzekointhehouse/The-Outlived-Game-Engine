using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ShapeRendering2D
{
    public class ShapeRenderer2D : DrawableGameComponent, IShapeRenderer2D
    {
        private GraphicsDevice graphics;
        private BasicEffect effect;
        private ContentManager content;

        // The shapes that we'll be drawing
        // We use a cache system to reuse our DebugShape instances to avoid creating garbage
        private static readonly List<Shape> activeShapes = new List<Shape>();
        
        // Allocate an array to hold our vertices; this will grow as needed by our renderer
        private static VertexPositionColor[] verts = new VertexPositionColor[64];
        
        // An array we use to get corners from bounding boxes
        private static Vector3[] corners = new Vector3[4];
        
        // This holds the vertices for our unit sphere that we will use when drawing bounding spheres
        private const int circleResolution = 30;
        private const int circleLineCount = (circleResolution + 1);
        private static Vector3[] unitSphere;

        public ShapeRenderer2D(Game game) : base(game)
        {
            content = game.Content;
            graphics = game.GraphicsDevice;
        }

        public override void Initialize()
        {
            Viewport v = graphics.Viewport;
            effect = new BasicEffect(graphics);
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = false;

            // Fit an orthographic projectin to the viewport
            effect.World = Matrix.CreateTranslation(-v.Width / 2, -v.Height / 2, 0) * Matrix.CreateScale(1, -1, 1);
            effect.Projection = Matrix.CreateOrthographic(v.Width, v.Height, -0.1f, 0.1f);

            // Create our unit sphere vertices
            InitializeCircle();

            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            // Go through our active shapes and remove dead shapes
            for (int i = activeShapes.Count - 1; i >= 0; i--)
            {
                Shape s = activeShapes[i];
                s.Lifetime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (s.Lifetime <= 0)
                {
                    activeShapes.RemoveAt(i);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Enabled || !Visible)
                return;

            // Calculate the total number of vertices we're going to be rendering.
            int vertexCount = 0;
            foreach (var shape in activeShapes)
                vertexCount += shape.LineCount * 2;

            // If we have some vertices to draw
            if (vertexCount > 0)
            {
                // Make sure our array is large enough
                if (verts.Length < vertexCount)
                {
                    // If we have to resize, we make our array twice as large as necessary so
                    // we hopefully won't have to resize it for a while.
                    verts = new VertexPositionColor[vertexCount * 2];
                }

                // Now go through the shapes again to move the vertices to our array and
                // add up the number of lines to draw.
                int lineCount = 0;
                int vertIndex = 0;
                foreach (Shape shape in activeShapes)
                {
                    lineCount += shape.LineCount;
                    int shapeVerts = shape.LineCount * 2;
                    for (int i = 0; i < shapeVerts; i++)
                        verts[vertIndex++] = shape.Vertices[i];
                }

                // Apply the settings for our effect and render the primitives
                effect.CurrentTechnique.Passes[0].Apply();
                graphics.DrawUserPrimitives(PrimitiveType.LineList, verts, 0, lineCount);
            }

            base.Draw(gameTime);
        }

        private void InitializeCircle()
        {
            // We need two vertices per line, so we can allocate our vertices
            unitSphere = new Vector3[circleLineCount * 2];

            // Compute our step around each circle
            float step = MathHelper.TwoPi / circleResolution;

            // Used to track the index into our vertex array
            int index = 0;

            // Create the loop on the XY plane first
            for (float a = 0f; a < MathHelper.TwoPi; a += step)
            {
                unitSphere[index++] = new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f);
                unitSphere[index++] = new Vector3((float)Math.Cos(a + step), (float)Math.Sin(a + step), 0f);
            }
        }

        public void AddLine(Vector3 a, Vector3 b, Color color, float life)
        {
            // Get a DebugShape we can use to draw the line
            Shape shape = new Shape(1, life);

            // Add the two vertices to the shape
            shape.Vertices[0] = new VertexPositionColor(a, color);
            shape.Vertices[1] = new VertexPositionColor(b, color);
            activeShapes.Add(shape);
        }

        public void AddBoundingRectangle(Rectangle rectangle, Color color, float life)
        {
            // Get a DebugShape we can use to draw the box
            Shape shape = new Shape(4, life);

            // Get the corners of the rectangle            
            corners[0] = new Vector3(rectangle.Left, rectangle.Bottom, 0);
            corners[1] = new Vector3(rectangle.Left, rectangle.Top, 0);
            corners[2] = new Vector3(rectangle.Right, rectangle.Top, 0);
            corners[3] = new Vector3(rectangle.Right, rectangle.Bottom, 0);

            // Fill in the vertices for the bottom of the box
            shape.Vertices[0] = new VertexPositionColor(corners[0], color);
            shape.Vertices[1] = new VertexPositionColor(corners[1], color);
            shape.Vertices[2] = new VertexPositionColor(corners[1], color);
            shape.Vertices[3] = new VertexPositionColor(corners[2], color);
            shape.Vertices[4] = new VertexPositionColor(corners[2], color);
            shape.Vertices[5] = new VertexPositionColor(corners[3], color);
            shape.Vertices[6] = new VertexPositionColor(corners[3], color);
            shape.Vertices[7] = new VertexPositionColor(corners[0], color);

            activeShapes.Add(shape);
        }

        public void AddBoundingSphere(BoundingSphere sphere, Color color, float life)
        {
            // Get a DebugShape we can use to draw the sphere
            Shape shape = new Shape(circleLineCount, life);

            // Iterate our unit sphere vertices
            for (int i = 0; i < unitSphere.Length; i++)
            {
                // Compute the vertex position by transforming the point by the radius and center of the sphere
                Vector3 vertPos = unitSphere[i] * sphere.Radius + sphere.Center;

                // Add the vertex to the shape
                shape.Vertices[i] = new VertexPositionColor(vertPos, color);
            }
            activeShapes.Add(shape);
        }
    }
}
