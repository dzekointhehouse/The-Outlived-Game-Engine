using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Menu.States;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using static Game.Menu.States.CharacterMenu;

namespace Game.Entities
{
   public class GamePlayers
   {
       private GameConfig config;
       private Dictionary<PlayerIndex, Viewport> viewports;
        public GamePlayers(GameConfig config, Dictionary<PlayerIndex, Viewport> viewports)
        {
            this.config = config;
            this.viewports = viewports;
        }


        public void CreatePlayers()
        {
            foreach (var player in config.Players)
            {
                if (player.Team == MultiplayerMenu.TeamState.TeamOne)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            this.InitPlayerOne(player.CameraId, player.Character);
                            break;
                        case PlayerIndex.Two:
                            this.InitPlayerTwo(player.CameraId, player.Character);
                            break;
                        case PlayerIndex.Three:
                            this.InitPlayerThree(player.CameraId, player.Character);
                            break;
                        case PlayerIndex.Four:
                            this.InitPlayerFour(player.CameraId, player.Character);
                            break;
                    };
                }
                if (player.Team == MultiplayerMenu.TeamState.TeamTwo)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            this.InitPlayerOne(player.CameraId, player.Character);
                            break;
                        case PlayerIndex.Two:
                            this.InitPlayerTwo(player.CameraId, player.Character);
                            break;
                        case PlayerIndex.Three:
                            this.InitPlayerThree(player.CameraId, player.Character);
                            break;
                        case PlayerIndex.Four:
                            this.InitPlayerFour(player.CameraId, player.Character);
                            break;
                    };
                }

            }


        }


        // TODO should get spawn positions depending on map
        private void InitPlayerOne(int cameraId, string character)
        {
            var actionBindings1 = new ActionBindingsBuilder()
                .SetAction(Keys.W, EventConstants.WalkForward) //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FireWeapon)
                .SetAction(Keys.LeftShift, EventConstants.LightStatus)
                .SetAction(Keys.R, EventConstants.ReloadWeapon)
                .Build();

            CreatePlayer(
                sprite: character,
                actionBindings: actionBindings1,
                position: new Vector2(200, 200), // spawn point
                viewport: viewports[PlayerIndex.One],
                cageId: cameraId
            );
        }

        private void InitPlayerTwo(int cageId, string character)
        {
            var player2 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings2 = new ActionBindingsBuilder()
                .SetAction(Keys.I, EventConstants.WalkForward)
                .SetAction(Keys.K, EventConstants.WalkBackward)
                .SetAction(Keys.J, EventConstants.TurnLeft)
                .SetAction(Keys.L, EventConstants.TurnRight)
                .SetAction(Keys.O, EventConstants.FireWeapon)
                .SetAction(Keys.U, EventConstants.TurnAround)
                .SetAction(Keys.H, EventConstants.LightStatus)
                .SetAction(Keys.P, EventConstants.ReloadWeapon)
                .Build();

            CreatePlayer(
                character,
                actionBindings2,
                position: new Vector2(400, 400), // spawn point,
                viewport: viewports[PlayerIndex.Two],
                cageId: cageId,
                disabled: false
            );
        }

        private void InitPlayerThree(int cageId, string character)
        {
            var player3 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FireWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();

            CreatePlayer(
                character,
                actionBindings,
                position: new Vector2(300, 400), // spawn point,
                viewport: viewports[PlayerIndex.Three],
                cageId: cageId,
                disabled: false
                );

        }

        private void InitPlayerFour(int cageId, string character)
        {
            var player3 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FireWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();

            CreatePlayer(
                character,
                actionBindings,
                position: new Vector2(250, 250), // spawn point,
                viewport: viewports[PlayerIndex.Four],
                cageId: cageId,
                disabled: false
                );

        }

        //The multitude of options here is for easy debug purposes
        private void CreatePlayer(string sprite, ActionBindings actionBindings,
            Vector2 position,
            Viewport viewport,
            bool disabled = false, int cageId = 0)
        {
            if (disabled) return;

            var light = new Spotlight()
            {
                Position = position,
                Scale = new Vector2(850f),
                Radius = (float)0.0001,
                Intensity = (float)0.6,
                ShadowType = ShadowType.Solid // Will not lit hulls themselves
            };
            IEntityBuilder playerEntity = new EntityBuilder()
                .SetPosition(position, 10)
                .SetRendering(100, 100)
                .SetInertiaDampening()
                .SetBackwardsPenalty()
                .SetSprite(sprite, new Point(1252, 206), 313, 206)
                .SetLight(light)
                .SetSound("walking")
                .SetMovement(200, 380, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10) // Random direction
                .SetRectangleCollision()
                .SetCameraFollow(cageId)
                .SetPlayer(sprite)
                .SetTeam(cageId)
                .SetHealth()
                .SetCameraView(viewport, 0.5f, cageId)
                .SetScore()
                .SetAmmo()
                .SetHUD(false, showStats: true)
                .Build();


            var animationBindings = new SpriteAnimationBindingsBuilder()
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(1252, 206), new Point(0, 1030))
                        .StateConditions(State.WalkingForward)
                        .Length(40)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingForward)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingBackwards)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Build();

            ComponentManager.Instance.AddComponentToEntity(actionBindings, playerEntity.GetEntityKey());
            ComponentManager.Instance.AddComponentToEntity(animationBindings, playerEntity.GetEntityKey());


                var cageComponent = new CageComponent()
                {
                    CageId = cageId
                };
                ComponentManager.Instance.AddComponentToEntity(cageComponent, playerEntity.GetEntityKey());

            var weaponComponent = new WeaponComponent()
            {
                Damage = 10,
                ClipSize = 100
            };
            ComponentManager.Instance.AddComponentToEntity(weaponComponent, playerEntity.GetEntityKey());

        }
    }
}
