using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Game.Entities;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Wrappers;
using static Game.Menu.States.MultiplayerMenu.TeamState;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    public class MultiplayerMenu : IMenu, ILifecycle
    {
        public MenuNavigator MenuNavigator { get; }
        public PlayerVirtualInputCollection VirtualInputCollection { get; }

        // Dependencies
        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gm;

        // enum so we can keep track on which option
        // we currently are at.
        public enum TeamState
        {
            TeamOne,
            NoTeam,
            TeamTwo,
        }

        private TeamState[] StateOrder =
        {
            TeamOne, NoTeam, TeamTwo
        };

        private GenericButtonNavigator<TeamState>[] PlayerChoices;

        public MultiplayerMenu(GameManager gameManager, MenuNavigator menuNavigator,
            PlayerVirtualInputCollection virtualInputCollection)
        {
            MenuNavigator = menuNavigator;
            this.gm = gameManager;
            game = OutlivedGame.Instance();
            VirtualInputCollection = virtualInputCollection;
            var playerOneChoice = new GenericButtonNavigator<TeamState>(StateOrder, horizontalNavigation: true);
            var playerTwoChoice = new GenericButtonNavigator<TeamState>(StateOrder, horizontalNavigation: true);
            var playerThreeChoice = new GenericButtonNavigator<TeamState>(StateOrder, horizontalNavigation: true);
            var playerFourChoice = new GenericButtonNavigator<TeamState>(StateOrder, horizontalNavigation: true);
            PlayerChoices = new[]
            {
                playerOneChoice, playerTwoChoice, playerThreeChoice, playerFourChoice
            };

            for (var i = 0; i < PlayerChoices.Length; i++)
            {
                PlayerChoices[i].ButtonNavigator.CurrentIndex = 1; // Set start position to second choice "NoTeam"
                PlayerChoices[i].UpdatePosition(VirtualInputCollection.VirtualGamePads[i]);
            }

            //KeyboardPosition =
            //    new GenericButtonNavigator<GenericButtonNavigator<TeamState>>(PlayerChoices,
            //        horizontalNavigation: true);

            // Adding the options interval and gamemanager.
        }

        // Draws the character names and the button at the option that
        // is the current option that we are positioned at.
        private void DisplayPlayerChoice(TeamState playerChoice, float heightPercentage, SpriteBatch sb)
        {
            var viewport = game.GraphicsDevice.Viewport;
            sb.Draw(gm.MenuContent.TeamOptions, viewport.Bounds, Color.White);
            switch (playerChoice)
            {
                case NoTeam:
                    sb.Draw(gm.MenuContent.GamePadIcon,
                        new Vector2((float) (viewport.Width * 0.4), viewport.Height * heightPercentage), Color.White);
                    break;
                case TeamOne:
                    sb.Draw(gm.MenuContent.GamePadIconHighlight,
                        new Vector2((float) (viewport.Width * 0.2), viewport.Height * heightPercentage), Color.White);
                    break;
                case TeamTwo:
                    sb.Draw(gm.MenuContent.GamePadIconHighlight,
                        new Vector2((float) (viewport.Width * 0.6), viewport.Height * heightPercentage), Color.White);
                    break;
            }
        }

        // Here is all the drawing called for this
        // class, so if some drawing isn't in here
        // then it won't be drawn.
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gm.effects.DrawExpandingEffect(sb, gm.MenuContent.Background);

            var heightPercentage = 0.2f;
            foreach (var playerChoice in PlayerChoices)
            {
                DisplayPlayerChoice(playerChoice.CurrentPosition, heightPercentage, sb);
                heightPercentage += 0.2f;
            }

            sb.End();
        }

        // The update method for this class
        // that takes care of all the updates, that
        // are to be done.
        public void Update(GameTime gameTime)
        {
            if (VirtualInputCollection.PlayerOne().Is(Cancel, Pressed))
            {
                     MenuNavigator.GoBack();
            }

            for (var i = 0; i < PlayerChoices.Length; i++)
            {
                PlayerChoices[i].UpdatePosition(VirtualInputCollection.VirtualGamePads[i]);
            }

            if (VirtualInputCollection.PlayerOne().Is(Accept, Pressed))
            {
                var somePlayerHasTeam = PlayerChoices.Any(player => player.CurrentPosition != NoTeam);
                if (somePlayerHasTeam)
                {
                    gm.MenuContent.ClickSound.Play();
                    UpdateGameConfigurations();
                    MenuNavigator.GoTo(GameManager.GameState.CharacterMenu);
                }
            }
        }

        private Dictionary<int, PlayerIndex> IntegerToPlayerIndex = new Dictionary<int, PlayerIndex>
        {
            {0, PlayerIndex.One},
            {1, PlayerIndex.Two},
            {2, PlayerIndex.Three},
            {3, PlayerIndex.Four}
        };

        private void UpdateGameConfigurations()
        {
            // Clear before each game..
            gm.gameConfig.Players.Clear();
            for (var i = 0; i < PlayerChoices.Length; i++)
            {
                if (PlayerChoices[i].CurrentPosition == NoTeam) continue;
                gm.gameConfig.Players.Add(new Player
                {
                    Index = IntegerToPlayerIndex[i],
                    Team = PlayerChoices[i].CurrentPosition
                });
            }
        }

        public void ResetPlayerChoicesState()
        {
            foreach (var playerChoice in PlayerChoices)
            {
                playerChoice.CurrentPosition = playerChoice.Positions[0];
            }
        }

        public void Reset()
        {
        }

        public void BeforeShow()
        {
            ResetPlayerChoicesState();
            
            for (var i = 0; i < PlayerChoices.Length; i++)
            {
                PlayerChoices[i].ButtonNavigator.CurrentIndex = 1; // Set start position to second choice "NoTeam"
                PlayerChoices[i].UpdatePosition(VirtualInputCollection.VirtualGamePads[i]);
            }
        }

        public void BeforeHide()
        {
        }
    }
}