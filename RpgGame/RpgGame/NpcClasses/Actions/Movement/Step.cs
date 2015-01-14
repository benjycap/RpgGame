using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RpgGame.SpriteClasses;

namespace RpgGame.NpcClasses.Actions.Movement
{
    /// <summary>
    /// Defines one peice of a set of transitions used to move an NPC in a predetermined stepwise fashion.
    /// Algorithm for movement is defined in NPC.StepwiseMovement
    /// </summary>
    public struct Step
    {
        #region Fields Region

        int distance;
        Direction direction;

        #endregion

        #region Property Region

        public int Distance
        {
            get { return distance; }
        }

        public Direction Direction
        {
            get { return direction; }
        }

        #endregion

        #region Constructor Region

        public Step(int distance, Direction direction)
        {
            this.distance = Math.Abs(distance);
            this.direction = direction;
        }

        #endregion
    }
}
