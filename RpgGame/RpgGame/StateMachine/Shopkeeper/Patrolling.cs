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

namespace RpgGame.StateMachine.Shopkeeper
{
    public class Patrolling : ShopkeeperBaseState
    {
        #region Field Region

        // Random number generator
        Random random;
        // Array to hold the random values which will be generated for movement between shelves
        int[] randomValues;
        // Second stopwatch (in addition to base class stopwatch) to be used for speech
        Stopwatch speechStopwatch;
        // Array to contain times at which the state should request a transition to Idling
        TimeSpan[] idlingTransitionTimes;

        #endregion

        #region Constructor Region

        public Patrolling(ShopkeeperFSM fsm)
            : base(fsm)
        {
            // Store predetermined transition times in array
            idlingTransitionTimes = setupTransitionTimes();
            Reset();
            // Instantiate helper classes
            random = new Random();
            speechStopwatch = new Stopwatch();
        }

        private TimeSpan[] setupTransitionTimes()
        {
            TimeSpan[] times = new TimeSpan[1];
            times[0] = new TimeSpan(16, 0, 0);
            return times;
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc)
        {
            // Face shopkeeper upwards
            npc.CurrentAnimation = Direction.Up;
            // Set stopwatches to zero and start
            stopwatch.Restart();
            speechStopwatch.Restart();
            // Make random values for movement between shelves
            randomValues = MakeRandomValues();
            // Create a path from npc to new random destination according to randomly generated value above
            // Send the path to the shopkeeper
            npc.SendPath(PathProducer.GetSimplePath(npc.Position, new Vector2(randomValues[0], npc.Position.Y)));
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            // If randomly allocated time between 3 and 10 seconds has passed, move NPC according to its path
            if (stopwatch.ElapsedMilliseconds >= randomValues[1])
                npc.StepwiseMovement();

            // If it's path is complete, face NPC up and regenerate random values
            if (npc.PathComplete && stopwatch.ElapsedMilliseconds >= randomValues[1])
            {
                npc.CurrentAnimation = Direction.Up;
                npc.IsAnimating = false;
                stopwatch.Restart();
                randomValues = MakeRandomValues();
                npc.SendPath(PathProducer.GetSimplePath(npc.Position, new Vector2(randomValues[0], npc.Position.Y)));
            }

            // Independently from movement logic above, every 12 seconds push a random speech bubble from list to NPC
            if (speechStopwatch.ElapsedMilliseconds >= 12000)
            {
                speechStopwatch.Restart();
                SendSpeechBubble(random.Next(0, 3), npc, false);
            }

            // If in game time is 12pm, change state to Serving
            if (fsm.Time == new TimeSpan(12, 0, 0))
                fsm.ChangeState("Serving");  

            // If in game time is one of the Idling transition times, change state to Idling
            // Possible Bug: If NPC is moving for the entire duration of in game minute, it will miss it's window to
            // transition. However this would require a very fast game to real time clock ratio which is unintended
            if (idlingTransitionTimes.Contains(fsm.Time) && !npc.Moving)
                fsm.ChangeState("Idling");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Idling":
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.behindCounterPosition, InitialDirection.Horizontal));
                    return;
                case "Serving":
                    Reset();
                    npc.Speed = 1.5f;
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.behindCounterPosition, InitialDirection.Horizontal));
                    stopwatch.Start();
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Idling":
                    npc.StepwiseMovement();

                    if (npc.PathComplete)
                        fsm.transitionComplete();
                    return;
                case "Serving":
                    // Make greeting to customer, then head to counter
                    SendSpeechBubble(3, npc, true);
                    npc.CurrentAnimation = Direction.Down;
                    if (stopwatch.ElapsedMilliseconds >= 2000)
                        npc.StepwiseMovement();

                    if (fsm.AtCounter)
                    {
                        fsm.transitionComplete();
                    }
                    return;
            }
        }

        #endregion

        #region Private Methods

        private int[] MakeRandomValues()
        {
            int[] rand = new int[2];
            rand[0] = 32 * random.Next(1, 9);
            rand[1] = 1000 * random.Next(3, 11);
            return rand;
        }

        protected override SpeechBubble[] SetSpeech()
        {
            SpeechBubble[] speech = new SpeechBubble[4];
            speech[0] = new SpeechBubble("\"A History Of Existentialism\"... Why do I stock this?", 3);
            speech[1] = new SpeechBubble("I should probably consider getting a smaller shop.", 3);
            speech[2] = new SpeechBubble("Sorting things is so therapeutic!", 3);
            speech[3] = new SpeechBubble("Oh, a customer! Be right with you.", 2);
            return speech;
        }

        #endregion
    }
}
