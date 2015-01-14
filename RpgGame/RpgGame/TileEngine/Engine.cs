using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace RpgGame.TileEngine
{
    // Class to hold the width and height of tiles
    public class Engine
    {
        #region Field Region

        static int tileWidth;

        static int tileHeight;

        #endregion

        #region Property Region

        public static int TileWidth
        {
            get { return tileWidth; }
        }

        public static int TileHeight
        {
            get { return tileHeight; }
        }

        #endregion

        #region Constructors

        public Engine(int tileWidth, int tileHeight)
        {
            // n.b. cannot static members with instance syntax. Must use the name of the type.
            Engine.tileWidth = tileWidth;
            Engine.tileHeight = tileHeight;
        }

        #endregion

        #region Methods

        public static Point VectorToCell(Vector2 position)
        {
            return new Point((int)position.X / tileWidth, (int)position.Y / tileHeight);
        }

        #endregion
        
    }
}
