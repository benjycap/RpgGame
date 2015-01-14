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

namespace RpgGame.StateMachine.Thief
{

    public class Outside : ThiefBaseState
    {
        #region Constructor Region

        public Outside(ThiefFSM fsm)
            : base(fsm)
        {
        }

        #endregion

        #region State Methods

        // Action to take upon arrival into the state
        public override void Init(Npc npc)
        {
            npc.Visible = false;
            npc.Speed = 1f;
            if (fsm.FirstPass)
                npc.Position = fsm.atDoorPosition;
        }

        // Action to take with each update cycle
        public override void Update(Npc npc, GameTime gameTime)
        {
            if (fsm.Time == new TimeSpan(0, 30, 0))
                fsm.ChangeState("Thieving");
        }

        #endregion

        #region Transition Methods

        protected override void InitTransitionExt(Npc npc, string transitionState)
        {
            switch (transitionState)
            {
                case "Thieving":
                    npc.Visible = true;
                    return;
            }
        }

        protected override void UpdateTransitionExt(Npc npc, GameTime gameTime, string transitionState)
        {
            switch (transitionState)
            {
                case "Thieving":
                    fsm.transitionComplete();
                    return;

            }
        }

        #endregion

        #region Internal Method Region

        #endregion
    }
}
