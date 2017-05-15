using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    public class MultiplayerMenu : IMenu
    {
        // Dependencies
        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gameManager;
        private readonly ControlsConfig controls;
        private SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

        // players current position
        private TeamState PlayerOneChoice = TeamState.NoTeam;
        private TeamState PlayerTwoChoice = TeamState.NoTeam;
        private TeamState PlayerThreeChoice = TeamState.NoTeam;
        private TeamState PlayerFourChoice = TeamState.NoTeam;

        // used for the keyboard.
        private PlayerIndex currentPlayer;


        // enum so we can keep track on which option
        // we currently are at.
        public enum TeamState
        {
            TeamOne,
            NoTeam,
            TeamTwo,
        }

        public MultiplayerMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.Engine.Dependencies.Game;
            // Adding the options interval and gamemanager.
            controls = new ControlsConfig(0, 2, gameManager);
        }

        // Draws the character names and the button at the option that
        // is the current option that we are positioned at.
        private void DisplayPlayerChoice(TeamState playerChoice, float heightPercentage)
        {
            var viewport = game.GraphicsDevice.Viewport;
            sb.Draw(gameManager.GameContent.TeamOptions, viewport.Bounds, Color.White);
            switch (playerChoice)
            {
                case TeamState.NoTeam:
                    sb.Draw(gameManager.GameContent.GamePadIcon, new Vector2((float)(viewport.Width * 0.4), viewport.Height * heightPercentage), Color.White);
                    break;
                case TeamState.TeamOne:
                    sb.Draw(gameManager.GameContent.GamePadIconHighlight, new Vector2((float)(viewport.Width * 0.2), viewport.Height * heightPercentage), Color.White);
                    break;
                case TeamState.TeamTwo:
                    sb.Draw(gameManager.GameContent.GamePadIconHighlight, new Vector2((float)(viewport.Width * 0.6), viewport.Height * heightPercentage), Color.White);
                    break;

            }
        }

        // Here is all the drawing called for this
        // class, so if some drawing isn't in here
        // then it won't be drawn.
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
            MenuHelper.DrawBackground(spriteBatch, gameManager.GameContent);

            // We check if the players are connected. If they are,
            // we draw their option state on the screen.
            //if(GamePad.GetState(PlayerIndex.One).IsConnected)
            DisplayPlayerChoice(PlayerOneChoice, 0.2f);
            //if (GamePad.GetState(PlayerIndex.Two).IsConnected)
            DisplayPlayerChoice(PlayerTwoChoice, 0.4f);
            //if (GamePad.GetState(PlayerIndex.Three).IsConnected)
            DisplayPlayerChoice(PlayerThreeChoice, 0.6f);
            //if (GamePad.GetState(PlayerIndex.Four).IsConnected)
            DisplayPlayerChoice(PlayerFourChoice, 0.8f);

            spriteBatch.End();


        }

        // The update method for this class
        // that takes care of all the updates, that
        // are to be done.
        public void Update(GameTime gameTime)
        {
            // If doing it with the keyboard.
            // state interval for n player's not the
            // same as for the 3 options states, So
            // we set it to 3 (four players).
            controls.MaxLimit = 3;
            currentPlayer = (PlayerIndex)controls.MoveOptionPositionVertically((int)currentPlayer);
            // Reset
            controls.MaxLimit = 2;
            switch (currentPlayer)
            {
                case PlayerIndex.One:
                    PlayerOneChoice = (TeamState)controls.MoveOptionPositionHorizontally((int)PlayerOneChoice, PlayerIndex.One);
                    break;
                case PlayerIndex.Two:
                    PlayerTwoChoice = (TeamState)controls.MoveOptionPositionHorizontally((int)PlayerTwoChoice, PlayerIndex.Two);
                    break;
                case PlayerIndex.Three:
                    PlayerThreeChoice = (TeamState)controls.MoveOptionPositionHorizontally((int)PlayerThreeChoice, PlayerIndex.Three);
                    break;
                case PlayerIndex.Four:
                    PlayerFourChoice = (TeamState)controls.MoveOptionPositionHorizontally((int)PlayerFourChoice, PlayerIndex.Four);
                    break;

            }

            // which player does the move gamepad
            PlayerOneChoice = (TeamState)controls.MoveOptionPositionHorizontally((int)PlayerOneChoice, PlayerIndex.One);
            PlayerTwoChoice = (TeamState)controls.MoveOptionPositionHorizontally((int)PlayerTwoChoice, PlayerIndex.Two);
            PlayerThreeChoice = (TeamState)controls.MoveOptionPositionHorizontally((int)PlayerThreeChoice, PlayerIndex.Three);
            PlayerFourChoice = (TeamState)controls.MoveOptionPositionHorizontally((int)PlayerFourChoice, PlayerIndex.Four);

            // controls.GoBackButton();

            // Need atleast one player to proceed
            // Proceed if continue is pressed
            if (controls.ContinueButton(GameManager.GameState.CharacterMenu) && (PlayerOneChoice != TeamState.NoTeam || PlayerTwoChoice != TeamState.NoTeam
                || PlayerThreeChoice != TeamState.NoTeam || PlayerFourChoice != TeamState.NoTeam))
                UpdateGameConfigurations();

        }

        private void UpdateGameConfigurations()
        {
            // Clear before each game..
            gameManager.gameConfig.Players.Clear();


            // Add players from option team -> to teams they belong
            if (PlayerOneChoice == TeamState.TeamOne)
                gameManager.gameConfig.Players.Add(new Player() { Index = PlayerIndex.One });
            else if (PlayerOneChoice == TeamState.TeamTwo)
                gameManager.gameConfig.Players.Add(new Player() { Index = PlayerIndex.One });

            if (PlayerTwoChoice == TeamState.TeamOne)
                gameManager.gameConfig.Players.Add(new Player() { Index = PlayerIndex.Two });
            else if (PlayerTwoChoice == TeamState.TeamTwo)
                gameManager.gameConfig.Players.Add(new Player() { Index = PlayerIndex.Two });

            if (PlayerThreeChoice == TeamState.TeamOne)
                gameManager.gameConfig.Players.Add(new Player() { Index = PlayerIndex.Three });
            else if (PlayerThreeChoice == TeamState.TeamTwo)
                gameManager.gameConfig.Players.Add(new Player() { Index = PlayerIndex.Three });

            if (PlayerFourChoice == TeamState.TeamOne)
                gameManager.gameConfig.Players.Add(new Player() { Index = PlayerIndex.Four });
            else if (PlayerFourChoice == TeamState.TeamTwo)
                gameManager.gameConfig.Players.Add(new Player() { Index = PlayerIndex.Four });


            // Must reset values
            currentPlayer = PlayerIndex.One;
            PlayerOneChoice = TeamState.NoTeam;
            PlayerTwoChoice = TeamState.NoTeam;
            PlayerThreeChoice = TeamState.NoTeam;
            PlayerFourChoice = TeamState.NoTeam;
        }
    }
}
