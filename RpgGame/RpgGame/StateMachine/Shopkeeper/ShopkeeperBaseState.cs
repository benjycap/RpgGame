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
using RpgGame.StateMachine.Shopkeeper;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.Shopkeeper
{
    /// <summary>
    /// Base type for all States utilised by shopkeeper in the game.
    /// </summary>
    public abstract class ShopkeeperBaseState : BaseState
    {
        #region Field Region

        // Field to hold a reference to the state manager which this state is managed by
        protected new ShopkeeperFSM fsm;

        #endregion

        #region Constructor Region

        protected ShopkeeperBaseState(ShopkeeperFSM fsm)
            : base(fsm)
        {
            this.fsm = fsm;
        }

        #endregion

    }
}
