using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.TileEngine;
using RpgGame.NpcClasses.Actions.Speech;

namespace RpgGame.NpcClasses
{
    // Class to manage position and routines of all NPCs in the game
    public class NPCManager
    {
        #region Field Region

        List<Npc> npcs;

        #endregion

        #region Properties

        public List<Npc> NPCs
        {
            get { return npcs; }
        }

        #endregion

        #region Constructor

        public NPCManager()
        {
            npcs = new List<Npc>();
        }

        #endregion

        #region XNA Methods

        public void Update(GameTime gameTime)
        {
            // Iterate through NPCs
            foreach (Npc npc in npcs)
            {
                // Update NPC
                npc.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            // Iterate through NPCs
            foreach (Npc npc in npcs)
            {
                // Draw NPC
                npc.Draw(gameTime, spriteBatch, camera);
            }
        }

        #endregion

        #region Methods

        // Method to add an NPC to be Managed
        public void AddNPC(Npc npc)
        {
            npcs.Add(npc);
        }

        #endregion
    }
}
