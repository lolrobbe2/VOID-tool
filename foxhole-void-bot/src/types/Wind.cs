using System;

namespace FoxholeBot.types
{
    /// <summary>
    /// This enum represents the wind directions and their angles
    /// </summary>
    public enum WindDirection : UInt16
    {
        N = 0,
        NE = 315,
        E = 270,
        SE = 225,
        S = 180, 
        SW = 135,
        W = 90,
        NW = 45,
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
