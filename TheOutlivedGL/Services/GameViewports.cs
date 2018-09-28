using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Menu.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Services
{
    public class GameViewports
    {
        public Viewport defaultView;
        public Viewport leftTopView;
        public Viewport leftBottomView;
        public Viewport RightTopView;
        public Viewport RightBottomView;
        public Viewport top, bottom;
        private GameConfig config;
        public bool IsTeamOne { get; set; } = false;
        public bool IsTeamTwo { get; set; } = false;

        private Dictionary<PlayerIndex, Viewport> playerView = new Dictionary<PlayerIndex, Viewport>(4);

        public GameViewports(GameConfig gameConfig, Viewport viewport)
        {
            this.config = gameConfig;
            defaultView = viewport;
        }

        public void InitializeViewports()
        {
            var nPlayers = config.Players.Count;


            // Initializing the viewports 

            leftTopView = defaultView;
            leftBottomView = defaultView;
            RightTopView = defaultView;
            RightBottomView = defaultView;

            // Halft the width
            // and half the height = top left corner
            leftTopView.Width = leftTopView.Width / 2;
            leftTopView.Height = leftTopView.Height / 2;

            // Splitting the view in half
            // bottom views start is the top views height.
            // the height is the remaining of the pixels.
            leftBottomView.Width = leftBottomView.Width / 2;
            leftBottomView.Y = leftTopView.Height;
            leftBottomView.Height = defaultView.Height / 2;

            // start from the lefts width ending
            // width is the halft of the default
            // height is half of the viewport
            RightTopView.X = leftTopView.Width;
            RightTopView.Width = RightTopView.Width / 2;
            RightTopView.Height = RightTopView.Height / 2;

            // Splitting the view in half
            // bottom views start from the middle.
            // the height is halft of the default height.
            RightBottomView.Y = RightTopView.Height;
            RightBottomView.Height = RightBottomView.Height / 2;
            RightBottomView.X = leftBottomView.Width;
            RightBottomView.Width = RightBottomView.Width / 2;

            // Redo the vieport for two players;
            top = defaultView;
            bottom = defaultView;
            top.Height = top.Height / 2;
            bottom.Y = top.Height;
            bottom.Height = bottom.Height / 2;
            
            foreach (var player in config.Players)
            {
                if (player.Team == MultiplayerMenu.TeamStates.TeamOne)
                    IsTeamOne = true;
                if (player.Team == MultiplayerMenu.TeamStates.TeamTwo)
                    IsTeamTwo = true;
            }


            switch (nPlayers)
            {
                case 1:
                    var player = config.Players[0];
                    player.CameraId = 1;
                    playerView.Add(player.Index, defaultView);
                    break;
                case 2:
                    {
                        if (IsTeamOne && IsTeamTwo)
                        {
                            var playerOne = config.Players[0];
                            playerOne.CameraId = 1;
                            var playerTwo = config.Players[1];
                            playerTwo.CameraId = 2;
                            playerView.Add(playerOne.Index, top);
                            playerView.Add(playerTwo.Index, bottom);
                        }
                        else
                        {
                            var playerOne = config.Players[0];
                            playerOne.CameraId = 1;
                            var playerTwo = config.Players[1];
                            playerTwo.CameraId = 1;
                            playerView.Add(playerOne.Index, defaultView);
                            playerView.Add(playerTwo.Index, defaultView);

                        }
                    }
                    break;
                case 3:
                    {
                        if (IsTeamOne && IsTeamTwo)
                        {
                            var playerOne = config.Players[0];
                            playerOne.CameraId = 1;
                            var playerTwo = config.Players[1];
                            playerTwo.CameraId = 2;
                            var playerThree = config.Players[2];
                            playerThree.CameraId = 3;
                            playerView.Add(playerOne.Index, leftTopView);
                            playerView.Add(playerTwo.Index, leftBottomView);
                            playerView.Add(playerThree.Index, RightTopView);

                        }
                        else
                        {
                            var playerOne = config.Players[0];
                            playerOne.CameraId = 1;
                            var playerTwo = config.Players[1];
                            playerTwo.CameraId = 1;
                            var playerThree = config.Players[2];
                            playerThree.CameraId = 1;
                            playerView.Add(playerOne.Index, defaultView);
                            playerView.Add(playerTwo.Index, defaultView);
                            playerView.Add(playerThree.Index, defaultView);

                        }
                    }
                    break;
                case 4:
                    {
                        if (IsTeamOne && IsTeamTwo)
                        {
                            var playerOne = config.Players[0];
                            playerOne.CameraId = 1;
                            var playerTwo = config.Players[1];
                            playerTwo.CameraId = 2;
                            var playerThree = config.Players[2];
                            playerThree.CameraId = 3;
                            var playerFour = config.Players[3];
                            playerFour.CameraId = 4;
                            playerView.Add(playerOne.Index, leftTopView);
                            playerView.Add(playerTwo.Index, leftBottomView);
                            playerView.Add(playerThree.Index, RightTopView);
                            playerView.Add(playerFour.Index, RightBottomView);

                        }
                        else
                        {
                            var playerOne = config.Players[0];
                            playerOne.CameraId = 1;
                            var playerTwo = config.Players[1];
                            playerTwo.CameraId = 1;
                            var playerThree = config.Players[2];
                            playerThree.CameraId = 1;
                            var playerFour = config.Players[3];
                            playerFour.CameraId = 1;
                            playerView.Add(playerOne.Index, defaultView);
                            playerView.Add(playerTwo.Index, defaultView);
                            playerView.Add(playerThree.Index, defaultView);
                            playerView.Add(playerFour.Index, defaultView);

                        }
                    }
                    break;
                default:
                    return;
            }
        }

        public Dictionary<PlayerIndex, Viewport> GetViewports()
        {
            return playerView;
        }
    }
}
