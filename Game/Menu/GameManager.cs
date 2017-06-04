using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Menu;
using Game.Menu.States;
using Game.Menu.States.GameModes;
using Game.Menu.States.GameModes.DeathMatch;
using Game.Menu.States.GameModes.Extinction;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Spelkonstruktionsprojekt.ZEngine.Diagnostics;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using ZEngine.Systems;
using ZEngine.Wrappers;

namespace Game
{
    public class GameManager
    {
        public const float MinScale = 1.0f;
        public const float MaxScale = 2.0f;
        public static float Scale { get; set; } = 1.0f;
        public static bool MoveHigher { get; set; } = true;
        public static bool MoveRight { get; set; } = true;
        public static ZEngineLogger Logger { get; } = new ZEngineLogger();
        
        public MenuContent MenuContent { get; }

        // Here we just say that the first state is the Intro
        protected internal GameState CurrentGameState = GameState.Intro;

        protected internal GameState PreviousGameState;
        //        protected internal KeyboardState OldKeyboardState;
        //        protected internal GamePadState OldGamepadState;

        public VirtualGamePad Controller { get; set; }
        public MenuNavigator MenuNavigator { get; set; }

        protected internal FullSystemBundle Engine;
        protected internal Viewport Viewport;

        protected internal SpriteBatch spriteBatch = GameDependencies.Instance.SpriteBatch;

        // To keep track of the game configurations made
        protected internal GameConfig gameConfig;

        private IMenu mainMenu;
        private IMenu gameModesMenu;
        private IMenu characterMenu;
        private IMenu credits;
        private IMenu gameIntro;
        private IMenu survivalGame;
        private IMenu pausedMenu;
        private IMenu aboutMenu;
        private IMenu multiplayerMenu;
        private IMenu gameOver;
        private IMenu gameOverCredits;

        // Game states
        public enum GameState
        {
            Intro,
            MainMenu,
            GameModesMenu,
            CharacterMenu,
            MultiplayerMenu,
            PlaySurvivalGame,
            PlayDeathMatch,
            PlayExtinction,
            Quit,
            Credits,
            Paused,
            About,
            GameOver,
            GameOverCredits,
        };

        public Dictionary<GameState, IMenu> GameStateMenuMap;
        private PlayerVirtualInputCollection virtualInputCollection;

        public void SetCurrentState(GameState state)
        {
            CurrentGameState = state;
        }

        public GameManager(FullSystemBundle gameBundle)
        {
            Engine = gameBundle;
            Engine.Logger = Logger;
            Viewport = Engine.Dependencies.Game.GraphicsDevice.Viewport;
            MenuContent = new MenuContent(gameBundle.Dependencies.Game);
            gameConfig = new GameConfig();

            virtualInputCollection = new PlayerVirtualInputCollection(new[]
            {
                new VirtualGamePad(0, isKeyboardControlled: true),
                new VirtualGamePad(1),
                new VirtualGamePad(2),
                new VirtualGamePad(3)
            });
            
            MenuNavigator = new MenuNavigator(this);

            var gameModeDependencies = new GameModeDependencies()
            {
                GameConfig = gameConfig,
                MenuNavigator = MenuNavigator,
                SystemsBundle = Engine,
                Viewport = Viewport,
                VirtualInputs = virtualInputCollection
            };
            
            // initializing the states, remember:
            // all the states need to exist in the 
            // manager.
            mainMenu = new MainMenu(this, virtualInputCollection.PlayerOne(), MenuNavigator);
            gameModesMenu = new GameModeMenu(this, MenuNavigator, virtualInputCollection.PlayerOne());
            characterMenu = new CharacterMenu(this, virtualInputCollection);
            credits = new Credits(this, MenuNavigator, virtualInputCollection.PlayerOne());
            gameIntro = new GameIntro(this, MenuNavigator, virtualInputCollection.PlayerOne());
            pausedMenu = new PausedMenu(this, MenuNavigator, virtualInputCollection);
            multiplayerMenu = new MultiplayerMenu(this, MenuNavigator, virtualInputCollection);
            aboutMenu = new AboutMenu(this, MenuNavigator, virtualInputCollection.PlayerOne());
            gameOver = new GameOver(this, MenuNavigator, virtualInputCollection.PlayerOne());
            gameOverCredits = new GameOverCredits(this, MenuNavigator, virtualInputCollection.PlayerOne());

            var gameModeSurvival = new Survival(gameModeDependencies);
            var deathMatch = new DeathMatch(gameModeDependencies);
            var extinction = new Extinction(gameModeDependencies);

            GameStateMenuMap = new Dictionary<GameState, IMenu>
            {
                {GameState.Intro, gameIntro},
                {GameState.MainMenu, mainMenu},
//                {GameState.PlaySurvivalGame, survivalGame},
                {GameState.PlaySurvivalGame, gameModeSurvival},
                {GameState.PlayDeathMatch, deathMatch},
                {GameState.PlayExtinction, extinction},
                {GameState.Quit, mainMenu},
                {GameState.GameModesMenu, gameModesMenu},
                {GameState.CharacterMenu, characterMenu},
                {GameState.Credits, credits},
                {GameState.Paused, pausedMenu},
                {GameState.MultiplayerMenu, multiplayerMenu},
                {GameState.About, aboutMenu},
                {GameState.GameOver, gameOver},
                {GameState.GameOverCredits,  gameOverCredits}
            };
            var lifecycleStates = new Dictionary<GameState, ILifecycle>
            {
//                {GameState.PlaySurvivalGame, (ILifecycle) survivalGame},
                {GameState.PlaySurvivalGame, gameModeSurvival},
                {GameState.PlayDeathMatch, deathMatch},
                {GameState.PlayExtinction, extinction},
                {GameState.MultiplayerMenu, (ILifecycle) multiplayerMenu},
                {GameState.CharacterMenu, (ILifecycle) characterMenu},
                {GameState.GameOver, (ILifecycle) gameOver}
            };

            MenuNavigator.GameStateMenuMap = GameStateMenuMap;
            MenuNavigator.LifecycleStates = lifecycleStates;
        }

        // Draw method consists of a switch case with all
        // the different states that we have, depending on which
        // state we are we use that state's draw method.
        public void Draw(GameTime gameTime)
        {
            if (CurrentGameState == GameState.Paused)
            {
                Engine.Dependencies.Game.GraphicsDevice.Viewport = Viewport;
            }

            if (CurrentGameState == GameState.Quit)
            {
                Engine.Dependencies.Game.Exit();
            }
            else if (GameStateMenuMap.ContainsKey(CurrentGameState))
            {
                GameStateMenuMap[CurrentGameState].Draw(gameTime, spriteBatch);
            }
        }

        // Same as the draw method, the update method
        // we execute is the one of the current state.
        public void Update(GameTime gameTime)
        {
            foreach (var virtualGamePad in virtualInputCollection.VirtualGamePads)
            {
                virtualGamePad.UpdateKeyboardState();
            }

            if (CurrentGameState == GameState.Paused)
            {
                Engine.Dependencies.Game.GraphicsDevice.Viewport = Viewport;
            }

            if (CurrentGameState == GameState.Quit)
            {
                Engine.Dependencies.Game.Exit();
            }
            else if (GameStateMenuMap.ContainsKey(CurrentGameState))
            {
                GameStateMenuMap[CurrentGameState].Update(gameTime);
            }

            foreach (var virtualGamePad in virtualInputCollection.VirtualGamePads)
            {
                virtualGamePad.MoveCurrentStatesToOld();
            }
        }
    }
}