using System;

namespace FoxholeBot.types
{
    /// <summary>
    /// This enum represents the wind directions and their angles
    /// </summary>
    public enum WindDirection : UInt16
    {
        NORTH = 0,
        NORTH_EAST = 45,
        EAST = 90,
        SOUTH_EAST = 135,
        SOUTH = 180, 
        SOUTH_WEST = 225,
        WEST = 270,
        NORTH_WEST = 315,
    }

    public enum WindStrenght : byte
    {
        CALM = 1,
        BREEZE = 2,
        GUST = 3,
        GALE = 4,
        STORM = 5,

    }
}
