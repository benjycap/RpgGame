using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace RpgGame.TileEngine
{
    public class CollisionMap
    {
        #region Field Region

        List<Rectangle> collisionZones;

        #endregion

        #region Property Region

        public List<Rectangle> CollisionZones
        {
            get { return collisionZones; }
        }

        #endregion

        #region Constructor

        // Default constructor
        public CollisionMap()
        {
            collisionZones = new List<Rectangle>();
        }

        // Deep copy constructor
        public CollisionMap(List<Rectangle> cmap)
        {
            collisionZones = new List<Rectangle>(cmap);
        }

        #endregion

        #region Methods Region

        public void AddRectangle(Rectangle rect)
        {
            collisionZones.Add(rect);
        }

        #endregion
    }
}
