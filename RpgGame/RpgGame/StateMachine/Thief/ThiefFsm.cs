using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.SpriteClasses;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.NpcTypes;
using RpgGame.ClockClasses;
using RpgGame.StateMachine.BaseClasses;

namespace RpgGame.StateMachine.Thief
{
    public class ThiefFSM : BaseFsm
    {
        #region General Fields

        public readonly Vector2 atDoorPosition = new Vector2(198, 416);
        public readonly Vector2 shelfPosition = new Vector2(64, 82);

        #endregion

        #region Public Properties

        #endregion

        #region Constructor Region

        public ThiefFSM(ThiefNpc npc)
            : base(npc)
        {
        }

        // Set the current state when the FSM is instantiated depending on in game clock
        protected override void InitialiseCurrentState()
        {
            currentState = stateInstances["Outside"];
            currentStateName = "Outside";
        }

        protected override StateDictionary ConstructStateDictionary()
        {
            StateDictionary dict = new StateDictionary();
            dict.Add("Outside", typeof(Outside));
            dict.Add("Thieving", typeof(Thieving));
            dict.Add("Caught", typeof(Caught));
            return dict;
        }

        #endregion
    }
}
