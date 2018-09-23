using System.Linq;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class ScoreSystem : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        private readonly global::ZEngine.EventBus.EventBus EventBus = global::ZEngine.EventBus.EventBus.Instance;


        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.BulletCollision, HandleBulletCollisionScore);
            EventBus.Subscribe<SpecificCollisionEvent>(EventConstants.EnemyCollision, HandleEnemyCollisionScore);
        }

        private void HandleBulletCollisionScore(SpecificCollisionEvent CollisionEvent)
        {
            uint shooter = ComponentManager.Instance.GetEntityComponentOrDefault<BulletComponent>(CollisionEvent.Entity).ShooterEntityId;
            if (shooter == CollisionEvent.Target) return;

            var GameScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent));
            if (GameScoreList.Count <= 0) return;
            var GameScore = (GameScoreComponent)GameScoreList.First().Value;

            var shooterScore = ComponentManager.Instance.GetEntityComponentOrDefault<EntityScoreComponent>(shooter);
            if (shooterScore == null) return;

            if (DifferentTeams((uint) CollisionEvent.Target, shooter))
            { 
                shooterScore.score += GameScore.damageScore;
            }
        }

        private void HandleEnemyCollisionScore(SpecificCollisionEvent CollisionEvent)
        {
            var EntityScore = ComponentManager.Instance.GetEntityComponentOrDefault<EntityScoreComponent>(CollisionEvent.Entity);
            if (EntityScore == null) return;


            var GameScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent));
            if (GameScoreList.Count <= 0) return;
            var GameScore = (GameScoreComponent)GameScoreList.First().Value;

            if (DifferentTeams((uint) CollisionEvent.Target, CollisionEvent.Entity))
            {
                EntityScore.score += GameScore.damagePenalty;
            }
        }


        public void Update(GameTime gameTime)
        {

            var GameScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent));


            if (GameScoreList.Count <= 0) return;
            var GameScore = (GameScoreComponent)GameScoreList.First().Value;

            GameScore.TotalGameScore = 0;

            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(EntityScoreComponent)))
            {
                var scoreComponent = entity.Value as EntityScoreComponent;
                var health = ComponentManager.Instance.GetEntityComponentOrDefault<HealthComponent>(entity.Key);
                if (health == null) continue;
                if (health.IsAlive)
                {
                    scoreComponent.score += GameScore.survivalScoreFactor * gameTime.ElapsedGameTime.TotalSeconds;

                }
                GameScore.TotalGameScore += (int)scoreComponent.score;
                //Debug.WriteLine(entity.Key + ": " + scoreComponent.score);
            }
            //Debug.WriteLine("Total: " + GameScore.TotalGameScore);
        }



        /*
         * This function checks if the involved parties were on the same team or not
         * 
         */
        private bool DifferentTeams(uint target, uint entity)
        {
            var targetTeamComponent = ComponentManager.Instance.GetEntityComponentOrDefault<TeamComponent>(target);
            var entityTeamComponent = ComponentManager.Instance.GetEntityComponentOrDefault<TeamComponent>(entity);

            if (targetTeamComponent == null || entityTeamComponent == null)
                return true;

            if (targetTeamComponent.TeamId != entityTeamComponent.TeamId)
                return true;
            else return false;
        }

        public int TotalScore()
        {
            int scoresum = 0;
            var scoreEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(EntityScoreComponent));
            foreach (var scores in scoreEntities)
            {
                var ScoreComponent =
                          ComponentManager.Instance.GetEntityComponentOrDefault<EntityScoreComponent>(scores.Key);
                if (ScoreComponent != null)
                {
                    scoresum += (int) ScoreComponent.score;
                }
            }
            return scoresum;
        }

        public int TotalScore(TeamComponent team)
        {
            int teamScore = 0;
            var scoreEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(EntityScoreComponent));
            foreach (var scores in scoreEntities)
            {
                var ScoreComponent =
                          ComponentManager.Instance.GetEntityComponentOrDefault<EntityScoreComponent>(scores.Key);
                if (ComponentManager.Instance.EntityHasComponent<EntityScoreComponent>(scores.Key))
                {
                    teamScore += (int) ScoreComponent.score;
                }

            }
            return teamScore;
        }
    }
}
