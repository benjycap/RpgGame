using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.SpriteClasses;

namespace RpgGame.StateMachine.BaseClasses
{
    public class FsmInitialiser : BaseFsm
    {
        #region Constructor region

        public FsmInitialiser(Game game, AnimatedSprite player)
            : base(game, player)
        { }

        #endregion

        #region Abstract Methods

        protected override StateDictionary ConstructStateDictionary() { return null; }

        protected override void InitialiseCurrentState() { }

        #endregion
    }
}
