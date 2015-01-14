using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.SpriteClasses;

namespace RpgGame.Geometry
{
    // Static class to  facilitate collision detection algorithms
    public static class CollisionDetection
    {
        // Returns true if the two animated sprites intersect, false if not.
        public static bool DoBoxesIntersect(AnimatedSprite sprite1, AnimatedSprite sprite2)
        {
            float rect1LeftPos = sprite1.Position.X;
            float rect1RightPos = sprite1.Position.X + sprite1.Width;
            float rect1TopPos = sprite1.Position.Y;
            float rect1BottomPos = sprite1.Position.Y + sprite1.Height;

            float rect2LeftPos = sprite2.Position.X;
            float rect2RightPos = sprite2.Position.X + sprite2.Width;
            float rect2TopPos = sprite2.Position.Y;
            float rect2BottomPos = sprite2.Position.Y + sprite2.Height;

            return !(rect2LeftPos > rect1RightPos
                || rect2RightPos < rect1LeftPos
                || rect2TopPos > rect1BottomPos
                || rect2BottomPos < rect1TopPos);
        }

        // An overload of the above method which takes a rectangle as one of the parameters
        public static bool DoBoxesIntersect(AnimatedSprite sprite, Rectangle rect)
        {
            float rect1LeftPos = sprite.Position.X;
            float rect1RightPos = sprite.Position.X + sprite.Width;
            float rect1TopPos = sprite.Position.Y;
            float rect1BottomPos = sprite.Position.Y + sprite.Height;

            float rect2LeftPos = rect.X;
            float rect2RightPos = rect.X + rect.Width;
            float rect2TopPos = rect.Y;
            float rect2BottomPos = rect.Y + rect.Height;

            return !(rect2LeftPos > rect1RightPos
                || rect2RightPos < rect1LeftPos
                || rect2TopPos > rect1BottomPos
                || rect2BottomPos < rect1TopPos);
        }
    }
}
