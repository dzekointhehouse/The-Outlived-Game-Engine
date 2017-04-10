using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class HealthComponent
    {
        /*
        *  MaxHealth is the maximum health for a character. for now we set it to 100
        *  CurrentHealth is the health for a character that could have been damaged.
        */
        public int MaxHealth { get; set; } = 100;
        public int CurrentHealth { get; set; }

    }
}
