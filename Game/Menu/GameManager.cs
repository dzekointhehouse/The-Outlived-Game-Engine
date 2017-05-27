using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Menu;
using Game.Menu.States;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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

        // Game states
        public enum GameState
        {
            Intro,
            MainMenu,
            GameModesMenu,
            CharacterMenu,
            MultiplayerMenu,
            PlaySurvivalGame,
            Quit,
            Credits,
            Paused,
            About,
            GameOver
        };

        public Dictionary<GameState, IMenu> GameStateMenuMap;
        private PlayerVirtualInputCollection virtualInputCollection;
        private Dictionary<GameState, ILifecycle> LifecycleStates;
        
        public void SetCurrentState(GameState state)
        {
            if (LifecycleStates.ContainsKey(CurrentGameState))
            {
                LifecycleStates[CurrentGameState].BeforeHide();
            }
            if (LifecycleStates.ContainsKey(state))
            {
                LifecycleStates[state].BeforeShow();
            }
            PreviousGameState = CurrentGameState;
            CurrentGameState = state;
        }
        
        public GameManager(FullSystemBundle gameBundle)
        {
            Engine = gameBundle;
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

            // initializing the states, remember:
            // all the states need to exist in the 
            // manager.
            mainMenu = new MainMenu(this, virtualInputCollection.PlayerOne(), MenuNavigator);
            gameModesMenu = new GameModeMenu(this, MenuNavigator, virtualInputCollection.PlayerOne());
            characterMenu = new CharacterMenu(this, virtualInputCollection);
            credits = new Credits(this, MenuNavigator, virtualInputCollection.PlayerOne());
            gameIntro = new GameIntro(this, MenuNavigator, virtualInputCollection.PlayerOne());
            survivalGame = new InGame(this, MenuNavigator, virtualInputCollection.PlayerOne());
            pausedMenu = new PausedMenu(this, MenuNavigator, virtualInputCollection);
            multiplayerMenu = new MultiplayerMenu(this, MenuNavigator, virtualInputCollection);
            aboutMenu = new AboutMenu(this, MenuNavigator, virtualInputCollection.PlayerOne());
            gameOver = new GameOver(this, MenuNavigator, virtualInputCollection.PlayerOne());
            GameStateMenuMap = new Dictionary<GameState, IMenu>
            {
                {GameState.Intro, gameIntro},
                {GameState.MainMenu, mainMenu},
                {GameState.PlaySurvivalGame, survivalGame},
                {GameState.Quit, mainMenu},
                {GameState.GameModesMenu, gameModesMenu},
                {GameState.CharacterMenu, characterMenu},
                {GameState.Credits, credits},
                {GameState.Paused, pausedMenu},
                {GameState.MultiplayerMenu, multiplayerMenu},
                {GameState.About, aboutMenu},
                {GameState.GameOver, gameOver}
            };
            LifecycleStates = new Dictionary<GameState, ILifecycle>
            {
                {GameState.PlaySurvivalGame, (ILifecycle) survivalGame},
                {GameState.MultiplayerMenu, (ILifecycle) multiplayerMenu},
                {GameState.CharacterMenu, (ILifecycle) characterMenu}
            };
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
            else if(GameStateMenuMap.ContainsKey(CurrentGameState))
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
            else if(GameStateMenuMap.ContainsKey(CurrentGameState))
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