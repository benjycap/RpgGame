using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RpgGame.StateMachine.BaseClasses;

namespace RpgGame.StateMachine
{
    // StateInstanceDictionary is a class used to create and reference the state instances used by the FSM
    // The list is populated in the constructor which takes a StateDictionary as a parameter
    public class StateInstanceDictionary : Dictionary<string, BaseState>
    {
        // Hide default constructor
        private StateInstanceDictionary() { }

        // The FSM is passed so that it can be referenced by the states being instantiated
        public StateInstanceDictionary(StateDictionary stateDict, BaseFsm fsm)
        {
            BaseState state;

            // Here we create our list of states dynamically according to the state dictionary
            foreach (KeyValuePair<string, Type> kvp in stateDict)
            {
                string stateName = kvp.Key;
                Type stateType = kvp.Value;

                // Here we utilise the technique of reflection to create an class instance, given its type
                state = (BaseState)Activator.CreateInstance(stateType, new object[] { fsm });
                // Add instance to the dictionary
                this.Add(stateName, state);
            }
        }
    }
}
