using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using RpgGame.SpriteClasses;

namespace RpgGame.Geometry
{
    // Algorithm used to determine the distance in pixels between the a pair of sprites
    public static class EntityDistance
    {
        public static float GetDistance(AnimatedSprite sprite1, AnimatedSprite sprite2)
        {
            Rectangle rect1, rect2;

            int sprite1X = RoundToNearestInt.Round(sprite1.Position.X);
            int sprite1Y = RoundToNearestInt.Round(sprite1.Position.Y);

            int sprite2X = RoundToNearestInt.Round(sprite2.Position.X);
            int sprite2Y = RoundToNearestInt.Round(sprite2.Position.Y);

            rect1 = new Rectangle(sprite1X, sprite1Y, sprite1.Width, sprite1.Height);
            rect2 = new Rectangle(sprite2X, sprite2Y, sprite2.Width, sprite2.Height);

            Vector2 distance = new Vector2(rect1.X - rect2.X, rect1.Y - rect2.Y);

            return RoundToNearestInt.Round(Math.Abs(distance.Length()));
        }
    }
}
