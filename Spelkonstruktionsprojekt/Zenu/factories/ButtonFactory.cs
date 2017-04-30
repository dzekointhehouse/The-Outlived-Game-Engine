using System;
using Zenu.components;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.Zenu.factories
{
    public class ButtonFactory
    {
        public ButtonFactory(int menuManagerId)
        {
            MenuManager = menuManagerId;
        }

        public int MenuManager { get; }

        public int StartGameButton(int menuId, Action startGameCallback)
        {
            var entityId = EntityManager.GetEntityManager().NewEntity();
            var buttonComponent = new ButtonComponent
            {
                Link = () =>
                {
                    var menuManagerComponent = ComponentManager.Instance
                        .GetEntityComponentOrDefault<MenuManagerComponent>(MenuManager);
                    if (menuManagerComponent == null) return;
                    menuManagerComponent.SelectedMenu = MenuManagerComponent.NO_MENU_SELECTED;
                    startGameCallback();
                }
            };
            ComponentManager.Instance.AddComponentToEntity(buttonComponent, entityId);


            return entityId;
        }
    }
}