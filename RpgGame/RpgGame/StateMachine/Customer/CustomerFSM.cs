using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.SpriteClasses;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.NpcClasses.NpcTypes;
using RpgGame.ClockClasses;
using RpgGame.StateMachine.BaseClasses;
using RpgGame.StateMachine.Shopkeeper;

namespace RpgGame.StateMachine.Customer
{
    public class CustomerFSM : BaseFsm
    {
        #region General Fields

        // Locations used multiple times throughout the state machine
        public readonly Vector2 inFrontCounterPosition = new Vector2(544, 230);
        public readonly Vector2 atDoorPosition = new Vector2(198, 416);

        #endregion

        #region Public Properties

        // Return whether customer is at counter
        public bool IsAtCounter { get { return (npc.Position == inFrontCounterPosition); } }
        // Return whether customer is at front door
        public bool IsAtDoor { get { return (npc.Position == atDoorPosition); } }
        // Checks with shopkeeper's FSM whether shopkeeper is at counter
        public bool ShopkeeperAtCounter { get { return FsmManager.SFsm.AtCounter; } }
        // True if shopkeeper is in serving state
        public bool ShopkeeperServing { get { return FsmManager.SFsm.IsServing; } }
        
        #endregion

        #region Constructor Region

        public CustomerFSM(CustomerNpc npc)
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
            dict.Add("AtCounter", typeof(AtCounter));
            dict.Add("Chasing", typeof(Chasing));
            dict.Add("Outside", typeof(Outside));
            return dict;
        }

        #endregion
    }
}
