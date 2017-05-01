using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Managers;

namespace ZEngine.Systems
{
    public class ScoreSystem : ISystem
    {
        public void IncreaseEntityScore(int entityId, int points)
        {
            var scoreComponent = (ScoreComponent) ComponentManager.Instance.GetEntityComponentOrDefault(typeof(ScoreComponent), entityId);
            scoreComponent.score += points;
        }

        public int TotalScore()
        {
            var scoreEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(ScoreComponent));
            return scoreEntities
                .Select(entity => entity.Value)
                .OfType<ScoreComponent>()
                .Sum(scoreComponent => scoreComponent.score);
        }

        public int TotalScore(TeamComponent team)
        {
            return team.members
                .Where(member =>
                    ComponentManager.Instance.EntityHasComponent(typeof(ScoreComponent), member))
                .Sum(member => ComponentManager.Instance.GetEntityComponentOrDefault<ScoreComponent>(member).score);
        }

    }
}
