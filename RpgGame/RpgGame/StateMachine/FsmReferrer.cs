using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RpgLibrary.StateMachine.Customer;
using RpgLibrary.StateMachine.Shopkeeper;

namespace RpgLibrary.StateMachine
{
    // Class holds public references to shop FSMs so they can communicate
    public static class FsmReferrer
    {
        #region Field Region

        static ShopkeeperFSM sFsm;
        static CustomerFSM cFsm;

        #endregion

        #region Property Region

        public static ShopkeeperFSM SFsm { get { return sFsm; } }

        public static CustomerFSM CFsm { get { return cFsm; } }

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

        #endregion
    }
}
