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

namespace RpgGame.StateMachine.Child
{
    public class ChildFsm : BaseFsm
    {
        #region General Fields

        public readonly Vector2 atDoorPosition = new Vector2(198, 416);

        bool isCaught;
        
        #endregion

        #region Public Properties

        public int NpcID { get { return ((ChildNpc)npc).ID; } }

        public Vector2 CurrentPosition { get { return npc.Position; } }

        // Expose child npc
        public Npc Child { get { return npc; } }

        public bool IsPlaying { get { return (currentState.GetType() == typeof(Playing)); } }

        // Used by PlayingToCaught transition
        public bool IsCaught
        {
            get { return isCaught; }
            set { isCaught = value; }
        }

        #endregion

        #region Constructor Region

        public ChildFsm(ChildNpc npc) 
            : base(npc)
        {
            isCaught = false;
        }

        protected override StateDictionary ConstructStateDictionary()
        {
            StateDictionary dict = new StateDictionary();
            dict.Add("Caught", typeof(Caught));
            dict.Add("Enter", typeof(Enter));
            dict.Add("Exit", typeof(Exit));
            dict.Add("Playing", typeof(Playing));
            return dict;
        }

        // Set the current state when the FSM is instantiated
        protected override void InitialiseCurrentState()
        {
            currentState = stateInstances["Exit"];
            currentStateName = "Exit";
        }

        #endregion
    }
}
