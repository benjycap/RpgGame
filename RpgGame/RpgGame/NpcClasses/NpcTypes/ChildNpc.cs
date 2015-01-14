using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.SpriteClasses;
using RpgGame.TileEngine;
using RpgGame.Geometry;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;

namespace RpgGame.NpcClasses.NpcTypes
{
    // Class for Child Npc
    // Each of the 3 children will have an individual instance of Npc and Fsm
    // Occasionally the children will have different behavior from one other
    // In order for the Fsm to determine which child to assign what behaviour, a statically determined ID is assigned upon child instantiation
    public class ChildNpc : Npc
    {
        #region Fields

        // Keep track of the what ID to assign to the next child
        static int numInstances = 0;
        // This instance's ID
        public int ID { get; private set; }

        #endregion

        #region Constructor

        public ChildNpc(Texture2D sprite, Dictionary<Direction, Animation> animation)
            : base(sprite, animation)
        {
            // Assign static value to ID and then increment
            ID = numInstances;
            numInstances++;
            if (ID >= 3)
                throw new Exception("Number of Child instances may not exceed 3");

            Speed = 4f;
        }

        #endregion

        #region Methods

        public override string GetNpcType()
        {
            return "Child";
        }

        #endregion
    }
}
