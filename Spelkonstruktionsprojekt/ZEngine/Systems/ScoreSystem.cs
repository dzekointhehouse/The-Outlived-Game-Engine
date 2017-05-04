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
            var scoreComponent = (ScoreComponent)ComponentManager.Instance.GetEntityComponentOrDefault(typeof(ScoreComponent), entityId);
            scoreComponent.score += points;
        }

        public int TotalScore()
        {
            int scoresum = 0;
            var scoreEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(ScoreComponent));
            foreach (var scores in scoreEntities)
            {
                var ScoreComponent =
                          ComponentManager.Instance.GetEntityComponentOrDefault<ScoreComponent>(scores.Key);
                if (ComponentManager.Instance.EntityHasComponent<ScoreComponent>(scores.Key))
                {
                    scoresum = ScoreComponent.score;

                }

            }
            return scoresum;
            //return scoreEntities
            //    .Select(entity => entity.Value)
            //    .OfType<ScoreComponent>()
            //    .Sum(scoreComponent => scoreComponent.score);
        }

        public int TotalScore(TeamComponent team)
        {
            int teamScore = 0;
            var scoreEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(ScoreComponent));
            foreach (var scores in scoreEntities)
            {
                var ScoreComponent =
                          ComponentManager.Instance.GetEntityComponentOrDefault<ScoreComponent>(scores.Key);
                if (ComponentManager.Instance.EntityHasComponent<ScoreComponent>(scores.Key))
                {
                    teamScore += ScoreComponent.score;

                }

            }
            return teamScore;
            //return team.members
            //    .Where(member =>
            //        ComponentManager.Instance.EntityHasComponent(typeof(ScoreComponent), member))
            //    .Sum(member => ComponentManager.Instance.GetEntityComponentOrDefault<ScoreComponent>(member).score);
        }

    }
}
