using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RpgGame
{
    public class InputHandler : GameComponent
    {
        #region Keyboard Field Region

        static KeyboardState keyboardState;
        static KeyboardState lastKeyboardState;

        #endregion

        #region Keyboard Property Region

        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        public static KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }

        #endregion

        #region Constructor Region

        // Game parameter is passed in order to faciliate XNA methods
        public InputHandler(Game game)
            : base(game)
        {
            keyboardState = Keyboard.GetState();
        }

        #endregion

        #region XNA methods

        // Method is called 
        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        #endregion

        #region Keyboard Region

        // Key has just been released
        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
                lastKeyboardState.IsKeyDown(key);
        }

        // Key has just been pushed down
        public static bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
                lastKeyboardState.IsKeyUp(key);
        }

        // Key is currently down
        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        // Reset keyboard state
        public static void Flush()
        {
            lastKeyboardState = keyboardState;
        }

        #endregion

    }
}
