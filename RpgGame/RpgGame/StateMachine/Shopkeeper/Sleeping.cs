using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.Shopkeeper
{
    /// <summary>
    /// State class to describe behaviour when Shopkeeper is sleeping
    /// </summary>
    public class Sleeping : ShopkeeperBaseState
    {
        #region Constructor Region

        public Sleeping(ShopkeeperFSM fsm)
            : base(fsm)
        {
        }

        #endregion

        #region State Methods

        // Action to take upon arrival into the state
        public override void Init(Npc npc)
        {
            // Upon entry to this state NPC has walked to the door to his room
            // Set NPC to invisible to create illusion of having walked through door
            npc.Visible = false;
            // Set NPC position if necessary
            if (fsm.FirstPass)
                npc.Position = fsm.upstairsPosition;
        }

        // Action to take with each update cycle
        public override void Update(Npc npc, GameTime gameTime)
        {
            // Scheduled action: when time reaches 9am, set shopkeeper to visible and change state to idling
            if (fsm.Time == new TimeSpan(9, 0, 0))
                fsm.ChangeState("Idling");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Idling":
                    Reset();
                    npc.Visible = true;
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.behindCounterPosition, InitialDirection.Vertical));
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Idling":
                    // Send first speech bubble
                    SendSpeechBubble(0, npc, true);

                    // Move according to predetermined path
                    npc.StepwiseMovement();

                    // Once target destination reached, wait one real time second then send second speach bubble
                    if (npc.PathComplete)
                    {
                        if (!stopwatch.IsRunning)
                            stopwatch.Start();
                        if (stopwatch.ElapsedMilliseconds >= 1000)
                            SendSpeechBubble(1, npc, true);
                    }

                    // If second speech bubble has been send and npc is no longer speaking, all jobs in the transition are done
                    if (speechSent[1] == true && !npc.Speaking)
                        fsm.transitionComplete();

                    return;
            }
        }

        #endregion

        #region Internal Method Region
    
        protected override SpeechBubble[] SetSpeech()
        {
            SpeechBubble[] speech = new SpeechBubble[2];
            speech[0] = new SpeechBubble("Time to get to work!", 2);
            speech[1] = new SpeechBubble("Err... Have you been here all night?", 3);
            return speech;
        }

        #endregion
    }
}
