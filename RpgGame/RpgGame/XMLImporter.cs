using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using RpgGame.NpcClasses;
using RpgGame.StateMachine;

namespace RpgGame
{
    public static class XMLImporter
    {
        public static TransitionDictionary GetTransitions(Npc npc)
        {
            XDocument xmlFile = XDocument.Load("TransitionData.xml");

            TransitionDictionary transitions = new TransitionDictionary();

            foreach (var node in xmlFile.Descendants("NPC"))
            {
                // Proceed if the type attribute matches the npc's GetNpcType implementation
                if (node.Attribute("type").Value == npc.GetNpcType())
                {
                    // Iterate over every element with the name NPC/State/Transition
                    foreach (var element in node.Elements("State").Elements("Transition"))
                    {
                        // Get the from and to states in the transition map according to the XML file
                        string fromState = element.Parent.Attribute("name").Value;
                        string toState = element.Value;

                        // Create new dictionary entry if necessary
                        if (!transitions.Keys.Contains(fromState))
                            transitions.Add(fromState, new List<string>());

                        // Add transition to the appropriate key's list
                        transitions[fromState].Add(toState);
                    }
                }
            }

            return transitions;
        }        
    }
}
