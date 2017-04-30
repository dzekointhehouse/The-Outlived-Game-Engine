using System.Collections.Generic;
using ZEngine.Components;

namespace Zenu.components
{
    public class MenuManagerComponent : IComponent
    {
        public static int NO_MENU_SELECTED { get; } = -1;
        public List<int> Menus { get; set; }
        public int SelectedMenu { get; set; }
    }
}