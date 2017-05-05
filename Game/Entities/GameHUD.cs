using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Game.Entities
{
    class GameHUD
    {
        public GameHUD()
        {
            //switch (type)
            //{
            //    case TypeHUD.Xbox:
            //        CreateXboxHUD();
            //        break;
            //    default: break;
            //}
            
        }

        public void CreateXboxHUD()
        {
            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(10, 1100))
                .SetSprite("XboxController")
                .Build();

            // Health Icons
            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(1680, 1150))
                .SetSprite("health3_small")
                .Build();
            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(1680, 1210))
                .SetSprite("health3_small")
                .Build();
        }
    }

    enum TypeHUD
    {
        Keyboard,
        Xbox,

    }
}
