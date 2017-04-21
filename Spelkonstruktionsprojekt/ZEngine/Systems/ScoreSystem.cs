using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class ScoreSystem : ISystem
    {
        public void increaseEntityScore(int entityId, int points)
        {
            var scoreComponent = (ScoreComponent) ComponentManager.Instance.GetEntityComponentOrDefault(typeof(ScoreComponent), entityId);
            scoreComponent.score += points;
        }

        public int totalScore()
        {
            int total = 0;
            var scoreEntities = ComponentManager.Instance.GetEntitiesWithComponent<ScoreComponent>();
            foreach (int key in scoreEntities.Keys)
            {
                total += scoreEntities[key].score;
            }
            return total;
        }

        public int totalScore(TeamComponent team)
        {
            int total = 0;
            foreach(int member in team.members)
            {
                if (ComponentManager.Instance.EntityHasComponent(typeof(ScoreComponent), member)){
                    total += ComponentManager.Instance.GetEntityComponentOrDefault<ScoreComponent>(member).score;
                }
            }
            return total;
        }

    }
}
