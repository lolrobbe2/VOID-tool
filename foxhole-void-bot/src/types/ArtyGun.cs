using System;

namespace FoxholeBot.types
{
    public class ArtyGun
    {
        //artillery bias per level
        public byte Bias;
        //name of the artillery gun
        public string Name;
        //max range of the artillery gun
        public UInt16 MaxRange;
        //min range of the artillery gun
        public UInt16 MinRange;
    }
}
