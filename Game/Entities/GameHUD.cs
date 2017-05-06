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
            //        CreateInGameHUD();
            //        break;
            //    default: break;
            //}

        }

        public void CreateInGameHUD()
        {
            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(10, 1150))
                .SetSprite("ak-47", scale: 0.3f)
                .Build();

            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(10, 1210))
                .SetSprite("ak-47", scale: 0.3f)
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
