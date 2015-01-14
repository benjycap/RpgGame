using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XRpgLibrary.Clock
{

    // THIS WHOLE CLASS IS RIDUCULOUS
    public class ClockDisplay
    {
        #region Field Region

        SpriteFont font;
        Clock clock;

        #endregion

        #region Constructor

        public ClockDisplay(Game game, Clock clock)
        {
            ContentManager Content = game.Content;
            font = Content.Load<SpriteFont>(@"Fonts\ControlFont");
            this.clock = clock;
        }

        #endregion

        #region XNA Methods

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                font,
                clock.Time.ToString(),
                new Vector2(100, 100),
                Color.White);
        }

        #endregion
    }
}
