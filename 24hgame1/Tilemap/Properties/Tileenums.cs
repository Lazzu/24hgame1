using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hgame1.Tilemap.Properties
{
    public enum Tiletype
    {
        Floor,
        FloorCeiling,
        Wall,
        Door
    }

    public enum Doorstate
    {
        Open,
        Closed
    }

    public enum Doordirection
    {
        North,
        East,
        South,
        West
    }
}
