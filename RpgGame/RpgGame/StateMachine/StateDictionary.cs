using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.StateMachine
{
    // This class is used to hold the relationship between the string name of a state and the actual type.
    // An instance must be instantiated and populated in each FSM.
    public class StateDictionary : Dictionary<string, Type>
    {
    }
}
