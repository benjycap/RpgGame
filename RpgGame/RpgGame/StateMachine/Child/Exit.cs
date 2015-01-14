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
    public class Exit : ChildBaseState
    {
        #region Constructor Region

        public Exit(ChildFsm fsm)
            : base(fsm)
        {
        }

        #endregion

        #region State Methods

        public override void Init(Npc npc)
        {
            // On first pass set initial position and hide npc
            if (fsm.FirstPass)
            {
                npc.Position = fsm.atDoorPosition;
                npc.Visible = false;
            }
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            if (fsm.Time == new TimeSpan(14, 0, 0))
                fsm.ChangeState("Enter");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        { }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Enter":
                    fsm.transitionComplete();
                    return;
            }
        }

        #endregion
    }
}
