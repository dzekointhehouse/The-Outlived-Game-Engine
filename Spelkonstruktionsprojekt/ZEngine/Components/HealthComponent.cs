using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class HealthComponent : IComponent
    {
        /*
        *  MaxHealth is the maximum health for a character. for now we set it to 100
        *  CurrentHealth is the health for a character that could have been damaged.
        *  Damage is a list of all the damage that has been taken but not yet applied in the HealthSystem
        *  our motivation for a list was in case we would like to keep track of all damage sources later
        *  it would be easier to refactor this way.
        */
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }

        public List<int> Damage { get; set; } = new List<int>(30);

        public bool Alive { get; set; }
        public bool isHuman { get; set; }

        public HealthComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            MaxHealth = 100;
            CurrentHealth = 100;
            Damage.Clear();
            Alive = true;
            isHuman = false;
            return this;
        }
    }
}

