﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private GameViewports gameViewports;
        private Dictionary<PlayerIndex, Viewport> viewports;
        private bool createdCamera = false;
        private GameMap maps;

        public GamePlayers(GameConfig config,  GameViewports gameViewports)
        {
            this.config = config;
            this.gameViewports = gameViewports;
            this.viewports = gameViewports.GetViewports();
        }


        public void CreatePlayers(GameMap maps)
        {
            this.maps = maps;
            foreach (var player in config.Players)
            {
                if (player.Team == MultiplayerMenu.TeamState.TeamOne)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            this.InitPlayerOne(player);
                            break;
                        case PlayerIndex.Two:
                            this.InitPlayerTwo(player);
                            break;
                        case PlayerIndex.Three:
                            this.InitPlayerThree(player);
                            break;
                        case PlayerIndex.Four:
                            this.InitPlayerFour(player);
                            break;
                    }
                }
                if (player.Team == MultiplayerMenu.TeamState.TeamTwo)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            this.InitPlayerOne(player);
                            break;
                        case PlayerIndex.Two:
                            this.InitPlayerTwo(player);
                            break;
                        case PlayerIndex.Three:
                            this.InitPlayerThree(player);
                            break;
                        case PlayerIndex.Four:
                            this.InitPlayerFour(player);
                            break;
                    }
                }
            }
        }


        // TODO should get spawn positions depending on map
        private void InitPlayerOne(Player player)
        {
            var actionBindings1 = new ActionBindingsBuilder()
                .SetAction(Keys.W, EventConstants.WalkForward) //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FirePistolWeapon)
                .SetAction(Keys.F, EventConstants.LightStatus)
                .SetAction(Keys.F, EventConstants.LightStatus)
                .SetAction(Keys.R, EventConstants.ReloadWeapon)
                .Build();

            CreatePlayer(
                sprite: player.SpriteName,
                actionBindings: actionBindings1,
                position: maps.spawnPositionOne, // spawn point
                viewport: viewports[player.Index],
                characterType: player.CharacterType, cageId: player.CameraId);
        }

        private void InitPlayerTwo(Player player)
        {
            var actionBindings2 = new ActionBindingsBuilder()
                .SetAction(Keys.I, EventConstants.WalkForward)
                .SetAction(Keys.K, EventConstants.WalkBackward)
                .SetAction(Keys.J, EventConstants.TurnLeft)
                .SetAction(Keys.L, EventConstants.TurnRight)
                .SetAction(Keys.O, EventConstants.FirePistolWeapon)
                .SetAction(Keys.U, EventConstants.TurnAround)
                .SetAction(Keys.H, EventConstants.LightStatus)
                .SetAction(Keys.P, EventConstants.ReloadWeapon)
                .Build();

            CreatePlayer(
                sprite: player.SpriteName,
                actionBindings: actionBindings2,
                position: maps.spawnPositionTwo, // spawn point,
                viewport: viewports[player.Index],
                characterType: player.CharacterType, cageId: player.CameraId);
        }

        private void InitPlayerThree(Player player)
        {
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FirePistolWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();

            CreatePlayer(
                sprite: player.SpriteName,
                actionBindings: actionBindings,
                position: maps.spawnPositionThree, // spawn point,
                viewport: viewports[player.Index],
                characterType: player.CharacterType, cageId: player.CameraId);
        }

        private void InitPlayerFour(Player player)
        {
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FirePistolWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();

            CreatePlayer(
                sprite: player.SpriteName,
                actionBindings: actionBindings,
                position: maps.spawnPositionFour, // spawn point,
                viewport: viewports[player.Index],
                characterType: player.CharacterType, cageId: player.CameraId);
        }

        //The multitude of options here is for easy debug purposes
        private void CreatePlayer(string sprite, ActionBindings actionBindings, Vector2 position, Viewport viewport, CharacterType characterType, int cageId = 0, bool disabled = false)
        {
            if (disabled) return;

            var light = new Spotlight()
            {
                Position = position,
                Scale = new Vector2(850f),
                Radius = (float) 0.0001,
                Intensity = (float) 0.6,
                ShadowType = ShadowType.Solid // Will not lit hulls themselves
            };
            EntityBuilder playerEntity = new EntityBuilder()
                .SetPosition(position, 500)
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
                //.SetCameraView(viewport, 0.5f, cageId)
                .SetScore()
                .SetHUD(false, showStats: true);

            // Add camera view only if there are two teams or if it is the first camera,
            // and the same .
            if (gameViewports.IsTeamOne && gameViewports.IsTeamTwo || 
                gameViewports.IsTeamOne && !gameViewports.IsTeamTwo && cageId == 1 && !createdCamera
                || !gameViewports.IsTeamOne && gameViewports.IsTeamTwo && cageId == 1 && !createdCamera)
            {
                createdCamera = true;
                var cameraView = new CameraViewComponent()
                {
                    CameraId = cageId,
                    View = viewport,
                    MinScale = 0.5f,
                    MaxScale = 1.5f
                };
                ComponentManager.Instance.AddComponentToEntity(cameraView, playerEntity.GetEntityKey());
            }
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


            var cageComponent = ComponentManager.Instance.ComponentFactory.NewComponent<CageComponent>();
            cageComponent.CageId = cageId;
            ComponentManager.Instance.AddComponentToEntity(cageComponent, playerEntity.GetEntityKey());

            var weaponComponent = ComponentManager.Instance.ComponentFactory.NewComponent<WeaponComponent>();
            switch (characterType)
            {
                case CharacterType.Bob:
                    weaponComponent.Damage = 40;
                    weaponComponent.ClipSize = 16;
                    weaponComponent.WeaponType = WeaponComponent.WeaponTypes.Pistol;
                    playerEntity.SetAmmo(weaponComponent.ClipSize, weaponComponent.ClipSize * 4);
                    break;
                case CharacterType.Edgar:
                    Debug.WriteLine("1111");
                    weaponComponent.Damage = 10;
                    weaponComponent.ClipSize = 128;
                    weaponComponent.WeaponType = WeaponComponent.WeaponTypes.Rifle;
                    playerEntity.SetAmmo(weaponComponent.ClipSize, weaponComponent.ClipSize * 4);
                    break;
                case CharacterType.Ward:
                    weaponComponent.Damage = 75;
                    weaponComponent.ClipSize = 16;
                    weaponComponent.WeaponType = WeaponComponent.WeaponTypes.Shotgun;
                    playerEntity.SetAmmo(weaponComponent.ClipSize, weaponComponent.ClipSize * 4);
                    break;
                case CharacterType.Jimmy:
                    weaponComponent.Damage = 100;
                    weaponComponent.ClipSize = 16;
                    weaponComponent.WeaponType = WeaponComponent.WeaponTypes.Pistol;
                    playerEntity.SetAmmo(weaponComponent.ClipSize, weaponComponent.ClipSize * 4);
                    break;
            }

            playerEntity.Build();
            ComponentManager.Instance.AddComponentToEntity(weaponComponent, playerEntity.GetEntityKey());
        }
    }
}