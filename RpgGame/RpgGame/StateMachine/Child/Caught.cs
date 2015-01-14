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

namespace RpgGame.StateMachine.Child
{
    public class Caught : ChildBaseState
    {
        #region Constructor Region

        public Caught(ChildFsm fsm)
            : base(fsm)
        {
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc)
        {
            npc.AbortPath();
            npc.SendPath(PathProducer.GetSimplePath(npc.Position, fsm.atDoorPosition));
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            npc.StepwiseMovement();

            if (npc.PathComplete)
                fsm.ChangeState("Exit");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Exit":
                    npc.Visible = false;
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Exit":
                    fsm.transitionComplete();
                    return;
            }
        }

        #endregion
    }
}
