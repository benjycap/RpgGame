using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.NpcTypes;

namespace RpgGame.NpcClasses.Actions.Speech
{
    // Publicly scoped struct to handle creation and use of conversations
    public struct NpcTypedSpeech
    {
        public SpeechBubble speechBubble;
        public string npcType;

        public NpcTypedSpeech(SpeechBubble sb, string nt)
        {
            speechBubble = sb;
            npcType = nt;
        }
    }

    // Class to hold a queue of speech bubbles to be spoken in a specific order by specific NPCs
    public class Conversation
    {
        #region Field Region

        // List of speech bubbles and their assigned npc type
        List<NpcTypedSpeech> conversation;
        // Current speech bubble being displayed
        int currentSpeech;
        // True if any participant in the conversation is speaking
        bool speaking;
        // Stopwatch keeps track of when to start next speech bubble
        Stopwatch stopwatch;
        // Integer for the stopwatch to refer to
        int timeInMS;

        #endregion

        #region Property Region

        public int CurrentSpeech { get { return currentSpeech; } }

        #endregion

        #region Constructor Region

        public Conversation()
        {
            conversation = new List<NpcTypedSpeech>();
            currentSpeech = 0;
            speaking = false;
            stopwatch = new Stopwatch();
        }

        public Conversation(List<NpcTypedSpeech> c)
        {
            conversation = new List<NpcTypedSpeech>(c);
            currentSpeech = 0;
            speaking = false;
            stopwatch = new Stopwatch();
        }

        #endregion

        #region Method Region

        // Run conversation method designed to work with XNA's looping execution model.
        // Each interested state/transition will execute RunConversation on the same conversation, passing their own npc as parameter
        // The speech bubble will only be pushed to the relevant NPC if said NPC is not speaking.
        public void Run(Npc npc)
        {
            // Do nothing if reached end of list
            if (currentSpeech >= conversation.Count)
                return;

            NpcTypedSpeech speech = conversation[currentSpeech];

            if (npc.GetType() == ReturnTypes(speech.npcType) && !speaking)
            {
                npc.PushSpeechBubble(speech.speechBubble);
                currentSpeech += 1;
                speaking = true;
                timeInMS = 1000 * speech.speechBubble.DisplayTime;
                stopwatch.Restart();
            }

            if (stopwatch.ElapsedMilliseconds >= timeInMS)
                speaking = false;

        }

        // Empowers states/transitions to reset a conversation's position to the beginning
        public void Reset()
        {
            currentSpeech = 0;
        }

        private Type ReturnTypes(string npc)
        {
            switch (npc)
            {
                case "Customer":
                    return typeof(CustomerNpc);
                case "Shopkeeper":
                    return typeof(ShopkeeperNpc);
                case "Child":
                    return typeof(ChildNpc);
                default:
                    throw new ArgumentException("Must be a valid NPC type");
            }
        }


        #endregion
    }
}
