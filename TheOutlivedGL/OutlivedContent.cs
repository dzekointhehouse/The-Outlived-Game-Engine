using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Spelkonstruktionsprojekt.ZEngine.Managers;

namespace Game
{
    /// <summary>
    /// Game content used in the menu.
    /// </summary>
    public class OutlivedContent
    {
        private Microsoft.Xna.Framework.Game game;


        public OutlivedContent(Microsoft.Xna.Framework.Game game)
        {
            this.game = game;

            //// Intro
            //IntroVideo = game.Content.Load<Video>("version1");
            //// Click
            
            //// Multiplayer




            //CharacterBackground = game.Content.Load<Texture2D>("Images/zombieCharacter");


        }


        public void LoadContent()
        {
            AssetManager.Instance.Add<SpriteFont>(game.Content, "Fonts/Score");
            AssetManager.Instance.Add<SpriteFont>(game.Content, "Fonts/ZMenufont");
            AssetManager.Instance.Add<SpriteFont>(game.Content, "ZEone");

            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Characters/character_hb");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Characters/character_he");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Characters/character_hw");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Characters/character_hj");

            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/background3");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/movingfog");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/mainoptions");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/paused");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/credits");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/about");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/teamoptions");

            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/gamemodemenu_he");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/gamemodemenu_hs");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Menu/gamemodemenu_hb");

            //GameModeHiglightExtinction = game.Content.Load<Texture2D>("Images/Menu/gamemodemenu_he");
            //GameModeHiglightSurvival = game.Content.Load<Texture2D>("Images/Menu/gamemodemenu_hs");
            //GameModeHiglightBlockworld = game.Content.Load<Texture2D>("Images/Menu/gamemodemenu_hb");

            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/GameOver/GameOver");


           // AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Keyboard/esc");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Keyboard/enter");
            //AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Keyboard/left");

            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Gamepad/gamepad");
            AssetManager.Instance.Add<Texture2D>(game.Content, "Images/Gamepad/gamepad_h");



            AssetManager.Instance.Add<SoundEffect>(game.Content, "sound/click2");
            AssetManager.Instance.Add<Song>(game.Content, "Sound/bg_menumusic");

            //ClickSound = game.Content.Load<SoundEffect>("sound/click2");

        }
    }
}
