using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine;
using RpgGame.StateMachine.BaseClasses;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.Customer
{
    public class AtCounter : CustomerBaseState
    {
        #region Constructor Region

        public AtCounter(CustomerFSM fsm)
            : base(fsm)
        {
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc)
        {
            // Ensure customer is facing correct direction at counter
            npc.CurrentAnimation = Direction.Right;
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            // If both shopkeeper is at the counter and in a serving state, run conversation
            if (fsm.ShopkeeperAtCounter && fsm.ShopkeeperServing)
                fsm.Conversations[0].Run(npc);

            // Wheneever the shopkeeper leaves the counter, change state to chasing
            if (!fsm.ShopkeeperAtCounter)
                fsm.ChangeState("Chasing");

            // When the conversation reaches the end and customer has finished speaking, change state to outside
            if (fsm.Conversations[0].CurrentSpeech == 19 && !npc.Speaking)
                fsm.ChangeState("Outside");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Chasing":
                    npc.CurrentAnimation = Direction.Left;
                    stopwatch.Restart();
                    return;
                case "Outside":
                    Reset();
                    npc.SendPath(PathProducer.GetSimplePath(fsm.inFrontCounterPosition, fsm.atDoorPosition, InitialDirection.Horizontal));
                    npc.Speed = 4f;
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Chasing":
                    if (stopwatch.ElapsedMilliseconds >= 1000)
                    {
                        stopwatch.Reset();
                        fsm.transitionComplete();
                    }
                    return;
                case "Outside":
                    npc.StepwiseMovement();
                    if (npc.PathComplete)
                        fsm.transitionComplete();
                    return;
            }
        }

        #endregion
    }
}
