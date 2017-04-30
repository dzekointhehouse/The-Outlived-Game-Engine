using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.Zenu.templates;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.Zenu.factories
{
    public class MenuBuilder
    {
        private readonly List<int> _buttons = new List<int>();

        public MenuBuilder(int menuManagerId, Vector2 position)
        {
            MenuManager = menuManagerId;
            Menu = EntityManager.GetEntityManager().NewEntity();
            Position = position;
        }

        public int MenuManager { get; }
        public int Menu { get; }

        public Vector2 Position { get; set; }
        public int ButtonHeight { get; set; } = 80;

        public void AddButton()
        {
            const int margin = 10;
            const int buttonHeight = 80;
            var zenuButtonTemplate = ZenuButton.Template();
            var button = new ButtonBuilder()
                .FromTemplate(zenuButtonTemplate)
                .Offset(0, _buttons.Count * (buttonHeight + margin))
                .Position(Position.X, Position.Y)
                .Build();

            _buttons.Add(button);
        }
    }
}