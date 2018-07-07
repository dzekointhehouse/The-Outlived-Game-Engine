using Game.Menu;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using static Game.Menu.OutlivedStates;

namespace Game.Services
{
    public class MenuNavigator
    {
        public Dictionary<GameState, ILifecycle> MenuStates { get; set; }
        public Dictionary<GameState, IMenu> GameStateMenuMap { get; set; }
        private Menu.GameManager GameManager { get; }
        private Stack<GameState> OldStates { get; } = new Stack<GameState>(10);

        public MenuNavigator(Menu.GameManager gameManager)
        {
            GameManager = gameManager;
        }
        
        public void GoBack()
        {
            AssetManager.Instance.Get<SoundEffect>("sound/click2").Play();
            GameManager.CurrentGameState = OldStates.Pop();
        }

        public void GoTo(GameState newState)
        {
            var currentGameState = GameManager.CurrentGameState;
            if (MenuStates.ContainsKey(currentGameState))
            {
                MenuStates[currentGameState].BeforeHide();
            }
            if (MenuStates.ContainsKey(newState))
            {
                MenuStates[newState].BeforeShow();
            }

            OldStates.Push(currentGameState);
            GameManager.SetCurrentState(newState);
        }

        public void Pause()
        {
            if (GameManager.CurrentGameState == GameState.Paused)
            {
                GameManager.SetCurrentState(OldStates.Pop());
            }
            else
            {
                OldStates.Push(GameManager.CurrentGameState);
                GameManager.SetCurrentState(GameState.Paused);
            }
        }

    }
}