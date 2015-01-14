using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.BaseClasses
{
    public abstract class BaseState
    {
        #region Fields

        // Field to hold a reference to the state manager which this state is managed by
        protected BaseFsm fsm;

        // Path for use by NPC
        protected Queue<Step> path;
        // Speech bubbles
        protected SpeechBubble[] speech;
        // Boolean array to keep track of whether a speech bubble has already been sent to NPC
        protected bool[] speechSent;
        // Stopwatch for use by transitions
        protected Stopwatch stopwatch;

        #endregion

        #region Constructor Region

        protected BaseState(BaseFsm fsm)
        {
            stopwatch = new Stopwatch();
            this.fsm = fsm;
        }

        #endregion

        #region State Methods

        // Action to take upon first entry into the state
        public abstract void Init(Npc npc);

        // Action to take with each update cycle
        public abstract void Update(Npc npc, GameTime gameTime);

        public void InitTransition(Npc npc, string transitionState)
        {
            if (!fsm.ValidStates.Contains(transitionState))
                throw new ArgumentException("State for transitioning to must be valid state as defined in FSM's StateDictionary");

            InitTransitionExt(npc, transitionState);
        }

        // Extension method to be overridden by derived states
        protected abstract void InitTransitionExt(Npc npc, string transitionState);

        // The UpdateTransition method is the one called by the FSM to carry out the NPC logic. 
        // UpdateTransitionEx is the method that is overridden in derived states.
        // This technique is to ensure that the code below, used to check that the transitionState 
        // parameter is valid, is not overridden.
        public void UpdateTransition(Npc npc, GameTime gameTime, string transitionState)
        {
            if (!fsm.ValidStates.Contains(transitionState))
                throw new ArgumentException("State for transitioning to must be valid state as defined in FSM's StateDictionary");

            UpdateTransitionExt(npc, gameTime, transitionState);
        }

        // Extension method to be overridden by derived states
        protected abstract void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState);

        #endregion

        #region NPC Communication Methods

        protected virtual void SendSpeechBubble(int i, Npc npc, bool single = true)
        {
            // Handle case where speechSent has not been initialised
            if (speechSent == null)
            {
                speechSent = new bool[speech.Length];
                for (int j = 0; j < speechSent.Length; j++)
                    speechSent[j] = false;
            }

            // If the relevant bool is set to false, send speech bubble to NPC and trip flag. Else do nothing.
            if (!speechSent[i])
            {
                npc.PushSpeechBubble(new SpeechBubble(speech[i]));
                // This bool is to facilitate methods that may require the same speech bubble being sent multiple times
                if (single)
                    speechSent[i] = true;
            }
        }

        #endregion

        #region Internal State Methods

        protected virtual SpeechBubble[] SetSpeech() { return null; }

        protected virtual void Reset()
        {
            speech = SetSpeech();

            if (speech != null)
            {
                if (speechSent == null)
                    speechSent = new bool[speech.Length];

                for (int i = 0; i < speechSent.Length; i++)
                    speechSent[i] = false;
            }

            stopwatch.Reset();
        }

        #endregion
    }
}
