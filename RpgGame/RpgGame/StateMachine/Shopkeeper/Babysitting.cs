using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine.Child;
using RpgGame.StateMachine;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.Shopkeeper
{
    public class Babysitting : ShopkeeperBaseState
    {
        #region Field Region

        // Current child being chased
        Npc child;

        #endregion

        #region Constructor Region

        public Babysitting(ShopkeeperFSM fsm)
            : base(fsm)
        {
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc)
        {
            child = GetNearestChild(npc);
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            // get the details of the nearest child
            child = GetNearestChild(npc);
            // If method returns no child, they have all been caught and shopkeeper should transition
            if (child == null)
            {
                fsm.ChangeState("Idling");
                return;
            }
            // Chase child
            npc.Chase(child);
            // Prevent Shopkeeper from ever actually catching a child by trapping it in a corner by limiting it's movement around the edges of the shop floor
            if (npc.Position.X <= 64) npc.Position = new Vector2(64, npc.Position.Y);
            if (npc.Position.X >= 380) npc.Position = new Vector2(380, npc.Position.Y);
            if (npc.Position.Y <= 146) npc.Position = new Vector2(npc.Position.X, 146);
            if (npc.Position.Y >= 288) npc.Position = new Vector2(npc.Position.X, 288);
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Idling":
                    Reset();
                    npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.topLeftCounterPosition));
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Idling":
                    SendSpeechBubble(0, npc, true);

                    if (!npc.Speaking)
                    {
                        npc.StepwiseMovement();
                        if (npc.Position == fsm.topLeftCounterPosition)
                            npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.behindCounterPosition, InitialDirection.Horizontal));

                        if (fsm.AtCounter)
                            fsm.transitionComplete();
                    }
                    return;
            }
        }

        #endregion

        #region Private Methods

        private Npc GetNearestChild(Npc npc)
        {
            List<ChildFsm> list = FsmManager.ChFsmList;
            Npc child = null;
            float minDistance = float.PositiveInfinity;

            // Iterate through each Child's Fsm
            foreach (ChildFsm chFsm in list)
            {
                // Skip child if it is already caught
                if (chFsm.IsCaught)
                    continue;

                // Else update return field to new child if it is closer
                if (Vector2.Distance(npc.Position, chFsm.Child.Position) < minDistance)
                {
                    minDistance = (Vector2.Distance(npc.Position, chFsm.Child.Position));
                    child = chFsm.Child;
                }
            }

            return child;
        }

        #endregion

        #region Base Override Methods

        protected override SpeechBubble[] SetSpeech()
        {
            return new SpeechBubble[] { new SpeechBubble("I'm gonna tell your parents about this!!", 2) };
        }

        #endregion
    }
}
