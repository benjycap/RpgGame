using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.NpcTypes;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.Customer
{
    public class Chasing : CustomerBaseState
    {
        #region Field Region

        // Boolean to check if customer has spoken to player during this run of the state
        bool spoken;
        // Int to keep track of which speechbubble's the customer has pushed
        int currentSpeech;

        #endregion

        #region Constructor Region

        public Chasing(CustomerFSM fsm)
            : base(fsm)
        {
            spoken = false;
            currentSpeech = 0;
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc)
        {
            // Reset has been overriden to ensure spoken is set to false
            Reset();

            // Here we check on the conversation's position so we can gauge if we need to reset the currentSpeech for this Chasing state
            // (Intended behaviour is that the current speech bubble sent is reset each day)
            // This method can be utilised as we know that the conversation is reset once a day
            // Thus we can avoid adding an unnecessary property to the FSM (e.g. bool IsNewDay)
            if (fsm.Conversations[0].CurrentSpeech == 0)
                currentSpeech = 0;
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            // Commence chasing interaction with player
            npc.Chase(fsm.PlayerSprite, true);

            // If the result of chase is a collision with player, speak to him
            if (npc.Colliding && !spoken)
            {
                SendSpeechBubble(currentSpeech, npc, true);
                spoken = true;
                if (currentSpeech < speech.Length - 1)
                    currentSpeech += 1;
            }

            // Once the shopkeeper returns to the counter, change state to AtCounter
            if (fsm.ShopkeeperAtCounter)
                fsm.ChangeState("AtCounter");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "AtCounter":
                    npc.Speed = 2f;
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.inFrontCounterPosition));
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "AtCounter":
                    npc.StepwiseMovement();
                    if (npc.PathComplete && !fsm.IsAtCounter)
                        npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.inFrontCounterPosition));

                    if (fsm.IsAtCounter)
                        fsm.transitionComplete();

                    return;
            }
        }

        #endregion

        #region Internal Methods

        protected override SpeechBubble[] SetSpeech()
        {
            speech = new SpeechBubble[4];
            speech[0] = new SpeechBubble("You have shiny hair!", 1.5);
            speech[1] = new SpeechBubble("Wanna be friends? I know cool stuff.", 2.5);
            speech[2] = new SpeechBubble("6 x 9 is meaning of life in base 13", 2.5);
            speech[3] = new SpeechBubble("I hear a voice saying \"Placeholder Text\"...?", 3);
            return speech;
        }

        protected override void Reset()
        {
            base.Reset();

            spoken = false;
        }

        #endregion
    }
}
