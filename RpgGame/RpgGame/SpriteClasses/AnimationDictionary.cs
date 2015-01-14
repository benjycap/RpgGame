using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.SpriteClasses
{
    // Class extends dictionary type to allow for animation cloning
    public class AnimationDictionary : Dictionary<Direction, Animation>
    {
        #region Clone

        public AnimationDictionary Clone()
        {
            AnimationDictionary clone = new AnimationDictionary();

            foreach (KeyValuePair<Direction, Animation> entry in this)
            {
                clone.Add(entry.Key, (Animation)entry.Value.Clone());
            }

            return clone;
        }

        #endregion
    }
}
