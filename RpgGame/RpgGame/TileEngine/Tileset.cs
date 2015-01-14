using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.TileEngine
{
    // Class to hold information on a tileset:
    // Tileset texture, Width/Heigh of tiles and number of tiles.
    public class Tileset
    {
        #region Field Region

        Texture2D image;
        int tileWidthInPixels;
        int tileHeightInPixels;
        int tilesWide;
        int tilesHigh;
        Rectangle[] sourceRectangles;

        #endregion

        #region Property Region

        public Texture2D Texture
        {
            get { return image; }
            set { image = value; }
        }

        public int TileWidth
        {
            get { return tileWidthInPixels; }
            set { tileWidthInPixels = value; }
        }

        public int TileHeight
        {
            get { return tileHeightInPixels; }
            set { tileHeightInPixels = value; }
        }

        public int TilesWide
        {
            get { return tilesWide; }
            private set { tilesWide = value; }
        }

        public int TilesHigh
        {
            get { return tilesHigh; }
            private set { tilesHigh = value; }
        }

        public Rectangle[] SourceRectangles
        {
            // Return a hard copy otherwise modifying a rectangle will modify it inside the class as well
            get { return (Rectangle[])sourceRectangles.Clone(); }
        }

        #endregion

        #region Constructor Region

        public Tileset(Texture2D image, int tilesWide, int tilesHigh, int tileWidth, int tileHeight)
        {
            Texture = image;
            TilesWide = tilesWide;
            TilesHigh = tilesHigh;
            TileWidth = tileWidth;
            TileHeight = tileHeight;

            int tiles = tilesWide * tilesHigh;

            sourceRectangles = new Rectangle[tiles];

            int tile = 0;

            for (int y = 0; y < tilesHigh; y++)
                for (int x = 0; x < tilesWide; x++)
                {
                    sourceRectangles[tile] = new Rectangle(
                        x * (tileWidth),
                        y * (tileHeight),
                        tileWidth,
                        tileHeight);
                    tile++;
                }
        }

        #endregion

        #region Method Region
        #endregion

    }
}
