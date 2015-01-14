using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RpgGame;
using RpgGame.Controls;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.GameScreens
{
    // Class for base game state which other game screens will derive from.
    // Inherits from GameState (which inherits from DrawableGameComponent)
    public abstract partial class BaseGameState : GameState
    {
        #region Fields region

        protected Game1 gameRef;

        protected ControlManager ControlManager;

        protected PlayerIndex playerIndexInControl;

        protected SpriteFont menuFont;

        #endregion

        #region Properties region
        #endregion

        #region Constructor Region

        public BaseGameState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            gameRef = (Game1)game;
        }

        #endregion

        #region XNA Method Region

        protected override void LoadContent()
        {
            ContentManager Content = Game.Content;
            menuFont = Content.Load<SpriteFont>(@"Fonts\ControlFont");
            
            ControlManager = new ControlManager(menuFont);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}
