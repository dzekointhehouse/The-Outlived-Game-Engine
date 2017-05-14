using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Services
{
    public class MenuHelper
    {
        public static void DrawBackground(SpriteBatch sb, GameContent gameContent)
        {
            if (GameManager.Scale <= GameManager.MaxScale && GameManager.Scale >= GameManager.MinScale)
            {
                if (GameManager.Scale <= GameManager.MinScale + 0.1)
                {
                    GameManager.MoveHigher = true;
                }
                if (GameManager.Scale >= GameManager.MaxScale - 0.1)
                {
                    GameManager.MoveHigher = false;
                }

                if (GameManager.MoveHigher)
                {
                    GameManager.Scale = GameManager.Scale + 0.0001f;
                }
                else
                {
                    GameManager.Scale = GameManager.Scale - 0.0001f;
                }
            }

            sb.Draw(
                texture: gameContent.Background,
                position: Vector2.Zero,
                color: Color.White,
                scale: new Vector2(GameManager.Scale)
            );
        }
    }
}