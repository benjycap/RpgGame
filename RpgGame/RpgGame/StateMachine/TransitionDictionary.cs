using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RpgGame.StateMachine.BaseClasses;

namespace RpgGame.StateMachine
{
    // This class holds the legal transitions between states.
    // Each FSM has a transition dictionary, loaded from TransitionData.xml via XMLImporter
    public class TransitionDictionary : Dictionary<string, List<string>>
    {
    }
}
