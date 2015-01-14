using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.SpriteClasses;
using RpgGame.TileEngine;
using RpgGame.NpcClasses;
using RpgGame.NpcClasses.Actions.Movement;
using RpgGame.NpcClasses.Actions.Speech;
using RpgGame.StateMachine;

namespace RpgGame.NpcClasses.NpcTypes
{
    public class ShopkeeperNpc : Npc
    {
        #region Constructor

        public ShopkeeperNpc(Texture2D sprite, Dictionary<Direction, Animation> animation)
            : base(sprite, animation)
        {
            Speed = 1f;
        }

        #endregion

        #region Methods

        public override string GetNpcType()
        {
            return "Shopkeeper";
        }

        #endregion
    }
}
