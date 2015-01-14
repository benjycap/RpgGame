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

namespace RpgGame.StateMachine.Customer
{
    public class Outside : CustomerBaseState
    {
        #region Constructor Region

        public Outside(CustomerFSM fsm)
            : base(fsm)
        {
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc)
        {
            // Set npc to invisible
            npc.Visible = false;
            // If first pass, set customers position accordingly
            if (fsm.FirstPass)
            {
                npc.CurrentAnimation = Direction.Up;
                npc.Position = fsm.atDoorPosition;
            }
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            // When time hits 12 oclock, customer is scheduled to change state to AtCounter
            if (fsm.Time == new TimeSpan(12, 0, 0))
                fsm.ChangeState("AtCounter");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "AtCounter":
                    npc.Visible = true;
                    npc.Speed = 2f;

                    path = PathProducer.GetSimplePath(npc.Position, fsm.inFrontCounterPosition, InitialDirection.Vertical);
                    npc.SendPath(path);

                    // As this transition is run once per day, we can place the command to reset the conversation here
                    fsm.Conversations[0].Reset();
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "AtCounter":
                    if (npc.CurrentStep == 0)
                        npc.StepwiseMovement();

                    if (npc.CurrentStep == 1)
                    {
                        if (npc.CurrentAnimation != Direction.Right)
                        {
                            npc.CurrentAnimation = Direction.Right;
                            npc.IsAnimating = false;
                        }

                        if (stopwatch.IsRunning == false)
                            stopwatch.Start();

                        if (npc.Speed != 4f)
                            npc.Speed = 4f;

                        if (stopwatch.ElapsedMilliseconds >= 1000)
                            npc.StepwiseMovement();
                    }

                    if (fsm.IsAtCounter)
                        fsm.transitionComplete();
                    
                    return;
            }
        }

        #endregion
    }
}
