using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine.BaseClasses;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.Thief
{
    /// <summary>
    /// Base type for all States utilised by shopkeeper in the game.
    /// </summary>
    public abstract class ThiefBaseState : BaseState
    {
        #region Field Region

        // Field to hold a reference to the state manager which this state is managed by
        protected new ThiefFSM fsm;

        #endregion

        #region Constructor Region

        protected ThiefBaseState(ThiefFSM fsm)
            : base(fsm)
        {
            this.fsm = fsm;
        }

        #endregion

    }
}
