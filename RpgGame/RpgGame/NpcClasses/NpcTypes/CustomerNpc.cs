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
    public class CustomerNpc : Npc
    {

        #region Constructor

        public CustomerNpc(Texture2D sprite, Dictionary<Direction, Animation> animation)
            : base(sprite, animation)
        {
        }

        #endregion

        #region Methods

        public override string GetNpcType()
        {
            return "Customer";
        }

        #endregion
    
    }
}
