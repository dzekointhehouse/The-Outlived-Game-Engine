using Microsoft.Xna.Framework;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public interface ISystem
    {
    }

    public interface IDrawables
    {
        bool Enabled { get; set; }
        int DrawOrder { get; set; }

        void Draw();
    }

    public interface IUpdateables
    {
        bool Enabled { get; set; }
        int UpdateOrder { get; set; }

        void Update(GameTime gt);
    }
}