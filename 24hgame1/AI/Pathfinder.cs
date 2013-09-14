using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hgame1.AI
{
    static class Pathfinder
    {
        static public int[,] PathMap { get; private set; }

        public static void setPathMap(int[,] _pathmap)
        {
            PathMap = _pathmap;
        }
    }
}
