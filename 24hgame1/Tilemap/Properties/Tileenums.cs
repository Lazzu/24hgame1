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
        WallWindow,
        Door
    }

    public enum Walltype
    {
        Full,
        Half
    }

    /// <summary>
    /// Is door shatterable
    /// </summary>
    public enum Doortype
    {
        Wood,
        Iron
    }

    /// <summary>
    /// Is door open or closed
    /// </summary>
    public enum Doorstate
    {
        Open,
        Closed
    }

    /// <summary>
    /// Direction door is facing
    /// </summary>
    public enum Doordirection
    {
        North,
        East,
        South,
        West
    }

    /// <summary>
    /// Window brokestate
    /// </summary>
    public enum WindowState{
        Intact,
        Broken
    }
}
