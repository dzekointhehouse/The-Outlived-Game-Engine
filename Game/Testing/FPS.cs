using Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spelkonstruktionsprojekt.ZEngine.GameTest
{

    public class FPS : DrawableGameComponent
    {
            // Reference to the Game object
            private OutlivedGame game;

            // Frame rate related stuff
            private float frameCount = 0;           // frame counter
            private float timeSinceLastUpdate = 0;  // time since last FPS update
            private float updateInterval = 1;       // updateInterval = 1 -> "frames per second"
            private float fps = 0;                  // frames per second

            // This holds the font we use to draw the FPS string
            private SpriteFont font;

            public FPS(Microsoft.Xna.Framework.Game game) : base(game)
            {
                // Store a reference to the Game object
                this.game = (OutlivedGame)game;

                // Add this component to the game's GameComponentCollection
                game.Components.Add(this);
            }

            public override void Initialize()
            {
                base.Initialize();
            }

            protected override void LoadContent()
            {
                // Load the sprite font
                font = Game.Content.Load<SpriteFont>("ZEone");
                base.LoadContent();
            }

            protected override void UnloadContent()
            {
                base.UnloadContent();
            }

            public override void Update(GameTime gameTime)
            {
                // Increment the frame counter
                frameCount++;

                // Time elapsed since the last frame
                float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Time elapsed since the last FPS update interval
                timeSinceLastUpdate += elapsedGameTime;

                // If the time elapsed since the last FPS update interval
                // exceeds the chosen update interval ...
                if (timeSinceLastUpdate > updateInterval)
                {
                    // Calculate the number of frames that have
                    // occurred since the last FPS update interval
                    // (this is "frames per second" if updateInterval = 1)
                    fps = frameCount / timeSinceLastUpdate;

                    // Reset the frame counter
                    frameCount = 0;

                    // Adjust the time elapsed since the last FPS update
                    timeSinceLastUpdate -= updateInterval;
                }

                base.Update(gameTime);
            }

            public override void Draw(GameTime gameTime)
            {
                // Draw a text containing the FPS
                game.spriteBatch.Begin();
                game.spriteBatch.DrawString(
                    font, string.Format("FPS: {0}", fps.ToString()),
                    new Vector2(0f, 0f), Color.White);
                game.spriteBatch.End();

                base.Draw(gameTime);
            }
        }
}
