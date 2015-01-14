using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine.BaseClasses;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.Child
{
    // Base class for Child states
    public abstract class ChildBaseState : BaseState
    {
        #region Field Region

        // Field to hold a reference to the state manager which this state is managed by
        protected new ChildFsm fsm;

        #endregion

        #region Constructor Region

        protected ChildBaseState(ChildFsm fsm)
            : base(fsm)
        {
            this.fsm = fsm;
        }

        #endregion

        #region Execute Methods

        public override void Init(Npc npc)
        {
            Reset();

            switch (fsm.NpcID)
            {
                case 0:
                    Entry0(npc);
                    break;
                case 1:
                    Entry1(npc);
                    break;
                case 2:
                    Entry2(npc);
                    break;
            }
        }

        public override void Update(Npc npc, GameTime gameTime)
        {
            switch (fsm.NpcID)
            {
                case 0:
                    Update0(npc, gameTime);
                    break;
                case 1:
                    Update1(npc, gameTime);
                    break;
                case 2:
                    Update2(npc, gameTime);
                    break;
            }
        }

        #endregion

        #region Specific Behaviour Region

        protected virtual void Entry0(Npc npc) { }

        protected virtual void Entry1(Npc npc) { }

        protected virtual void Entry2(Npc npc) { }

        protected virtual void Update0(Npc npc, GameTime gameTime) { }

        protected virtual void Update1(Npc npc, GameTime gameTime) { }

        protected virtual void Update2(Npc npc, GameTime gameTime) { }

        #endregion
    }
}
