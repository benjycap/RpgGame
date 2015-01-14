using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.StateMachine.Shopkeeper;
using RpgGame.StateMachine.Customer;
using RpgGame.StateMachine.Child;
using RpgGame.StateMachine.Thief;

namespace RpgGame.StateMachine
{
    // Class holds public references to shop FSMs so they can communicate
    public static class FsmManager
    {
        #region Field Region

        static ShopkeeperFSM sFsm;
        static CustomerFSM cFsm;
        static List<ChildFsm> chFsmList;
        static ThiefFSM tFsm;

        #endregion

        #region Property Region

        public static ShopkeeperFSM SFsm { get { return sFsm; } }

        public static CustomerFSM CFsm { get { return cFsm; } }

        public static List<ChildFsm> ChFsmList { get { return chFsmList; } }

        public static ThiefFSM TFsm { get { return tFsm; } }

        #endregion

        #region Execute Region

        public static void Execute(GameTime gameTime)
        {
            sFsm.Execute(gameTime);
            cFsm.Execute(gameTime);
            foreach (ChildFsm chFsm in chFsmList)
                chFsm.Execute(gameTime);
            tFsm.Execute(gameTime);

            Console.WriteLine("Thief: " + (!tFsm.InTransition ? tFsm.CurrentState : tFsm.CurrentState + " -> " + tFsm.TransitionState));
        }

        #endregion

        #region Method Region

        public static void AddShopkeeperFsm(ShopkeeperFSM fsm)
        {
            sFsm = fsm;
        }

        public static void AddCustomerFsm(CustomerFSM fsm)
        {
            cFsm = fsm;
        }

        public static void AddChildFsm(ChildFsm fsm)
        {
            if (chFsmList == null)
                chFsmList = new List<ChildFsm>();

            // Throw exception if attempting to add a child with the same ID
            foreach (ChildFsm existingFsm in chFsmList)
                if (fsm.NpcID == existingFsm.NpcID)
                    throw new Exception("Adding more than one child to the referrer with the same ID is illegal");
            
            // Throw exception if list position and ID do not match as certain behaviour depends on this being consistant
            if (fsm.NpcID != chFsmList.Count)
                throw new Exception("Child Fsm ID must match the position in the FSM list");

            // Else add to list
            chFsmList.Add(fsm);
        }

        public static void AddThiefFsm(ThiefFSM fsm)
        {
            tFsm = fsm;
        }

        #endregion
    }
}
