using Game.Services;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Game.Menu.States.GameModes
{
    public class GameModeDependencies
    {
        public GameConfig GameConfig { get; set; }
        public Viewport Viewport { get; set; }
        public GameEngine SystemsBundle { get; set; }
        public MenuNavigator MenuNavigator { get; set; }
        public PlayerVirtualInputCollection VirtualInputs { get; set; }
    }
}