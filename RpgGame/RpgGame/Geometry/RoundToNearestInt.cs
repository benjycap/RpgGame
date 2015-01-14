using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Geometry
{
    // Accepts float arguement, return closest integer value.
    public static class RoundToNearestInt
    {
        public static int Round(float n)
        {
            int x = (int)((n - Math.Floor(n) < 0.5) ? Math.Floor(n) : Math.Ceiling(n));
            return x;
        }
    }
}
