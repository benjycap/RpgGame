using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.TileEngine
{
    public class World
    {
        #region Field Region

        // List of TileMaps which hold the data necessary to draw the level's map
        List<TileMap> levels;
        // List of CollisionMaps which designate specific rectangles in a level as impassable
        List<CollisionMap> collisionMaps;
        // Current level being drawn & updated
        int currentLevel;

        #endregion

        #region Property Region

        public int CurrentLevel
        {
            get { return currentLevel; }
            set
            {
                if (value < 0 || value >= levels.Count)
                    throw new IndexOutOfRangeException();
                if (levels[value] == null)
                    throw new NullReferenceException();
                if (levels.Count != collisionMaps.Count)
                    throw new Exception("Number of levels not equal to number of collision maps.");
                currentLevel = value;
            }
        }

        public CollisionMap CurrentCollision
        {
            get { return collisionMaps[currentLevel]; }
        }

        #endregion

        #region Constructor Region

        public World()
        {
            // List of levels starts empty
            levels = new List<TileMap>();
            // List of collision maps
            collisionMaps = new List<CollisionMap>();
            // Current level must be set before first use or an exception will be thrown
            currentLevel = -1;
        }

        #endregion

        #region XNA Methods

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            levels[currentLevel].Draw(spriteBatch, camera);
        }

        #endregion

        #region Method Region

        public void PushLevel(TileMap tileMap, CollisionMap colMap)
        {
            levels.Add(tileMap);
            collisionMaps.Add(colMap);
        }

        #endregion
    }
}
