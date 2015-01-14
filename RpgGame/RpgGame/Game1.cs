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

using RpgGame;
using RpgGame.ClockClasses;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.Geometry;
using RpgGame.StateMachine;
using RpgGame.GameScreens;

namespace RpgGame
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region XNA Fields

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        #endregion

        #region Screen Field Region

        const int screenWidth = 1360;
        const int screenHeight = 768;

        public readonly Rectangle ScreenRectangle;

        #endregion

        #region Game State Region

        GameStateManager stateManager;

        public TitleScreen titleScreenInstance;
        public GamePlayScreen gamePlayScreenInstance;

        #endregion

        #region Component Field Region

        Clock clock;
        NPCManager npcManager;
        List<Conversation> conversations;

        #endregion

        #region Constructor Region

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            ScreenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            Content.RootDirectory = "Content";

            Components.Add(new InputHandler(this));

            clock = new Clock(this, 30);
            // Add as components which enable the XNA framework to initialise and update a module as necessary
            Components.Add(clock);
            // Services provide the functionality to publicly access an instance of a class without having to use anti-pattern global variables
            Services.AddService(typeof(Clock), clock);

            npcManager = new NPCManager();
            Services.AddService(typeof(NPCManager), npcManager);

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            titleScreenInstance = new TitleScreen(this, stateManager);
            gamePlayScreenInstance = new GamePlayScreen(this, stateManager);

            stateManager.ChangeState(titleScreenInstance);
        }

        #endregion

        #region XNA Methods Region

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {      
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load bubble textures
            Texture2D bubbleTexture = Content.Load<Texture2D>(@"Misc\speechbubble");
            SpriteFont bubbleFont = Content.Load<SpriteFont>(@"Fonts\ControlFont");
            new SpeechBubble(bubbleTexture, bubbleFont);

            conversations = MakeConversations();
            Services.AddService(typeof(List<Conversation>), conversations);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            base.Draw(gameTime);
        }

        #endregion

        #region Private Method region

        private List<Conversation> MakeConversations()
        {
            List<Conversation> conversations = new List<Conversation>();
            List<NpcTypedSpeech> conversation = new List<NpcTypedSpeech>();

            conversation.Add(new NpcTypedSpeech(new SpeechBubble("HI BOB!", 2), "Customer"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("Err...", 1), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("My name is Humphrey actual...", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("APPLES", 2), "Customer"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("Ok...", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("Just give me a moment", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("Here you go.", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("How else can I help?", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("SHOELACES", 2), "Customer"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("Alright, anything else whilst I'm up there?", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("...?", 2), "Customer"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("Be right back then", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("Ok, that'll be...", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("FORK HANDLES!", 1), "Customer"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("Excuse me?", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("FORK HANDLES!", 1), "Customer"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("*grumble* But I just got back!", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("Four Candles you said?", 2), "Shopkeeper"));
            conversation.Add(new NpcTypedSpeech(new SpeechBubble("THANKS!", 2), "Customer"));

            conversations.Add(new Conversation(conversation));

            return conversations;
        }

        #endregion
    }
}
