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
    // State for when shopkeeper is interacting with customer
    public class Serving : ShopkeeperBaseState
    {
        #region Field Region

        // Random generator
        Random random;

        #endregion

        #region Constructor Region

        public Serving(ShopkeeperFSM fsm)
            : base(fsm)
        {
            random = new Random();
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc)
        {
            Reset();
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            if (fsm.AtCounter)
            {
                npc.CurrentAnimation = Direction.Left;
                if (fsm.CustomerAtCounter)
                {
                    fsm.Conversations[0].Run(npc);
                    if (stopwatch.IsRunning)
                        stopwatch.Reset();
                }
            }

            if (fsm.Conversations[0].CurrentSpeech == 6 || fsm.Conversations[0].CurrentSpeech == 12 || fsm.Conversations[0].CurrentSpeech == 17)
            {
                if (!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds >= 1000)
                    npc.StepwiseMovement();

                // If at counter, create path from counter to random shelf
                if (fsm.AtCounter && fsm.CustomerAtCounter)
                    npc.SendPath(PathProducer.GetSimplePath(fsm.behindCounterPosition, new Vector2(32 * random.Next(2, 9), 82), InitialDirection.Vertical));

                // If arrived at shelf, create path from shelves back to counter and execute after waiting 1 second (to collect the item)
                if (fsm.AtShelves && !npc.Moving)
                {
                    stopwatch.Start();
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.behindCounterPosition, InitialDirection.Horizontal));
                    npc.CurrentAnimation = Direction.Up;
                    npc.IsAnimating = false;
                }
            }

            if (fsm.Conversations[0].CurrentSpeech == 19 && fsm.CustomerAtDoor)
                fsm.ChangeState("Idling");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Idling":
                    npc.Speed = 1f;
                    Reset();
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Idling":
                    // If the first speech bubble hasn't been sent, send it
                    if (!speechSent[0])
                        SendSpeechBubble(0, npc, true);
                    // If it has, check the npc has finished talking then send the other
                    else if (!npc.Speaking && !speechSent[1])
                        SendSpeechBubble(1, npc, true);

                    // Transition is complete if npc has finished speaking second speech bubble
                    if (!npc.Speaking && speechSent[1])
                        fsm.transitionComplete();

                    return;
            }
        }

        #endregion

        #region Internal Method Region

        protected override SpeechBubble[] SetSpeech()
        {
            SpeechBubble[] speech = new SpeechBubble[2];
            speech[0] = new SpeechBubble("What an interesting man...", 2);
            speech[1] = new SpeechBubble("Wait, did he pay me?", 2);
            return speech;
        }

        #endregion
    }
}
