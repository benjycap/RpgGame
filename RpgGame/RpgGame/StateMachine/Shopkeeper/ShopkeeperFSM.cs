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
using RpgGame.StateMachine.Customer;

namespace RpgGame.StateMachine.Shopkeeper
{
    public class ShopkeeperFSM : BaseFsm
    {
        #region General Fields

        // Location used multiple times throughout the state machine
        public readonly Vector2 behindCounterPosition = new Vector2(602, 230);
        // In front of counter
        public readonly Vector2 inFrontCounterPosition = new Vector2(542, 230);
        // Above the counter
        public readonly Vector2 topLeftCounterPosition = new Vector2(384, 82);
        // Starting position to chase children
        public readonly Vector2 chaseStartingPosition = new Vector2(384, 146);
        // Position the NPC goes to sleep
        public readonly Vector2 upstairsPosition = new Vector2(644, 32);
        // Shelves area
        public readonly Rectangle inFrontShelves = new Rectangle(32, 82, 256, 1);
        // Behind counter area
        public readonly Rectangle behindCounterArea = new Rectangle(600, 66, 168, 350);

        #endregion

        #region Public Properties

        // When property is called, check whether the shopkeeper's position is at the behind the counter position
        public bool AtCounter { get { return (npc.Position == behindCounterPosition); } }

        // Return whether the shopkeeper is somewhere at the shelves
        public bool AtShelves { get { return inFrontShelves.Contains((int)npc.Position.X, (int)npc.Position.Y); } }

        // True if shopkeeper is in Serving state
        public bool IsServing { get { return (currentState.GetType() == typeof(Serving)); } }

        // Check with Customer FSM if customer is at counter
        public bool CustomerAtCounter { get { return (FsmManager.CFsm.IsAtCounter); } }

        // Check with Customer FSM if customer is at front door
        public bool CustomerAtDoor { get { return (FsmManager.CFsm.IsAtDoor); } }

        // Expose shopkeeper npc
        public Npc Shopkeeper { get { return npc; } }

        #endregion

        #region Constructor Region

        public ShopkeeperFSM(ShopkeeperNpc npc) 
            : base(npc)
        {
        }

        // Set the current state when the FSM is instantiated depending on in game clock
        protected override void InitialiseCurrentState()
        {
            if (Time <= new TimeSpan(9, 0, 0) || Time >= new TimeSpan(17, 30, 0))
            {
                currentState = stateInstances["Sleeping"];
                currentStateName = "Sleeping";
            }
            else
            {
                currentState = stateInstances["Idling"];
                currentStateName = "Idling";
            }
        }

        protected override StateDictionary ConstructStateDictionary()
        {
            StateDictionary dict = new StateDictionary();
            dict.Add("Sleeping", typeof(Sleeping));
            dict.Add("Idling", typeof(Idling));
            dict.Add("Patrolling", typeof(Patrolling));
            dict.Add("Serving", typeof(Serving));
            dict.Add("Babysitting", typeof(Babysitting));
            return dict;
        }

        #endregion
    }
}
