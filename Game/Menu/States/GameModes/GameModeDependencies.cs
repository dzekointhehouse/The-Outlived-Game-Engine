using Game.Services;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Game.Menu.States.GameModes
{
    public class GameModeDependencies
    {
        public GameConfig GameConfig { get; set; }
        public Viewport Viewport { get; set; }
        public FullSystemBundle SystemsBundle { get; set; }
        public MenuNavigator MenuNavigator { get; set; }
        public VirtualGamePad VirtualGamePad { get; set; }
    }
}