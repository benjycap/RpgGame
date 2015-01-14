using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.TileEngine
{
    // Class to hold information on the location of a specific tile:
    // Tileset and index of that tileset.
    public class Tile
    {
        #region Field Reigon

        int tileIndex;
        int tileset;

        #endregion

        #region Property Region

        public int TileIndex
        {
            get { return tileIndex; }
            private set { tileIndex = value; }
        }

        public int TileSet
        {
            get { return tileset; }
            private set { tileset = value; }
        }

        #endregion

        #region Constructor Region

        public Tile(int tileIndex, int tileset)
        {
            TileIndex = tileIndex;
            TileSet = tileset;
        }

        #endregion

    }
}
