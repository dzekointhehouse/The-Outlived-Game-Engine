using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Managers;
using ZEngine.EventBus;
using System.Diagnostics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;
using Microsoft.Xna.Framework;

namespace ZEngine.Systems
{
    public class ScoreSystem : ISystem
    {
        private readonly EventBus.EventBus E = EventBus.EventBus.Instance;


        public void Start()
        {
            E.Subscribe<SpecificCollisionEvent>(EventConstants.BulletCollision, HandleBulletCollisionScore);
            E.Subscribe<SpecificCollisionEvent>(EventConstants.EnemyCollision, HandleEnemyCollisionScore);
        }

        private void HandleBulletCollisionScore(SpecificCollisionEvent CollisionEvent)
        {
            int shooter = ComponentManager.Instance.GetEntityComponentOrDefault<BulletComponent>(CollisionEvent.Entity).ShooterEntityId;
            if (shooter == CollisionEvent.Target) return;

            var GameScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent));
            if (GameScoreList.Count <= 0) return;
            var GameScore = (GameScoreComponent)GameScoreList.First().Value;

            var shooterScore = ComponentManager.Instance.GetEntityComponentOrDefault<EntityScoreComponent>(shooter);
            if (shooterScore == null) return;

            if (DifferentTeams(CollisionEvent.Target, shooter))
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

            if (DifferentTeams(CollisionEvent.Target, CollisionEvent.Entity))
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
                var health = (HealthComponent)ComponentManager.Instance.GetEntityComponentOrDefault(typeof(HealthComponent), entity.Key);
                if (health == null) continue;
                if (health.Alive)
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
        private bool DifferentTeams(int target, int entity)
        {
            var targetTeamComponent = (TeamComponent)ComponentManager.Instance.GetEntityComponentOrDefault(typeof(TeamComponent), target);
            var entityTeamComponent = (TeamComponent)ComponentManager.Instance.GetEntityComponentOrDefault(typeof(TeamComponent), entity);

            if (targetTeamComponent == null || entityTeamComponent == null)
                return true;

            if (targetTeamComponent.teamId != entityTeamComponent.teamId)
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
