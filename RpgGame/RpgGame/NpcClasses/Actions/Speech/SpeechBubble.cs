using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.ClockClasses;

namespace RpgGame.NpcClasses.Actions.Speech
{
    /// <summary>
    /// Class to handle the display of a speech bubble displayed above an entity.
    /// SpeechBubble holds information for the text to be displayed and for how long.
    /// Each NPC has a single SpeechBubble field which will be updated by the FSM as appropriate.
    /// </summary>
    public class SpeechBubble
    {
        #region Field Region

        // Text to be displayed
        string text;
        // Length of time the text is to be displayed for
        TimeSpan displayTime;
        // Used to keep track of how long text has been displayed already
        TimeSpan timeElapsed;
        // Upon activation the below update method will be enabled for the duration of displayTime
        bool isActive;
        // Texture for the speech bubble that the text will go inside.
        // The field is static for efficiency. The texture will only be loaded once into memory,
        // when a specialised SpeechBubble constrcutor is instantiated for the first time in Game1.LoadContent.
        static Texture2D _bubbleTexture;
        // Sprite font. Same pattern as described for bubbleTexture.
        static SpriteFont _font;
        // Rectangle to which the speech bubble will be drawn to.
        // Declaring in a class wide scope instead of a local scope is more efficient and reduces garbage
        Rectangle bubbleDimensions;
        // Vector for text position
        Vector2 textPosition;
        // Width and Height of text
        Vector2 textDimensions;

        #endregion

        #region Property Region

        // Exposes isActive for the FSM
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public int DisplayTime
        {
            get { return displayTime.Seconds; } 
        }

        #endregion

        #region Constructor

        // General use constructor
        public SpeechBubble(string text, double timeInSeconds)
        {
            this.text = text;
            this.displayTime = TimeSpan.FromSeconds(timeInSeconds);            
            this.timeElapsed = TimeSpan.Zero;
            this.isActive = false;
            textDimensions = _font.MeasureString(text);
        }

        // Deep copy constructor
        public SpeechBubble(SpeechBubble speech)
        {
            this.text = speech.text;
            this.displayTime = speech.displayTime;
            this.timeElapsed = TimeSpan.Zero;
            this.isActive = false;
            textDimensions = _font.MeasureString(text);
        }

        // Contstructor to be called on game load in order to load texture and font
        public SpeechBubble(Texture2D bubbleTexture, SpriteFont font)
        {
            _bubbleTexture = bubbleTexture;
            _font = font;
        }

        #endregion

        #region XNA Methods

        public void Update(GameTime gameTime)
        {
            // If isActive has been set to true
            if (isActive)
            {
                timeElapsed += gameTime.ElapsedGameTime;

                if (timeElapsed > displayTime)
                {
                    isActive = false;
                    timeElapsed = TimeSpan.Zero;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Npc npc)
        {
            // Execute drawing of the speech bubble if it is active
            // NPC parameter is passed so that the SpeechBubble instance knows where to draw to
            if (isActive)
            {
                bubbleDimensions = new Rectangle((int)(npc.Position.X + 16), (int)(npc.Position.Y - textDimensions.Y), (int)textDimensions.X, (int)textDimensions.Y);
                textPosition = new Vector2(npc.Position.X + 16, npc.Position.Y - textDimensions.Y - 5);
                spriteBatch.Draw(_bubbleTexture, bubbleDimensions, Color.White);
                spriteBatch.DrawString(_font, text, textPosition, Color.Red);
            }
        }

        #endregion
    }
}
