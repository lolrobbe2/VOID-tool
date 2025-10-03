using System;

namespace FoxholeBot.types
{
    public enum Faction : byte
    {
        COLONIAL,
        WARDEN
    }
    public class ArtyGun
    {
        //name of the artillery gun
        public string Name;
        //artillery bias per level
        public byte Bias;
        //max range of the artillery gun
        public UInt16 MaxRange;
        //min range of the artillery gun
        public UInt16 MinRange;
        //min inaccuracy radius
        public float InacuracyMin;
        //max innaccuracy radius
        public float InacuracyMax;
        //wich faction the artillery gun belongs to
        public Faction Faction;
        //name of the image
        public string ImageName;

    };
    public class Guns {
        public static ArtyGun[] artyGuns = [
            new ()
            {
                Name = "\"Korindes\" field gun",
                Bias = 10,
                MaxRange = 250,
                MinRange = 100,
                InacuracyMin = 22.5f,
                InacuracyMax = 30f,
                Faction = Faction.COLONIAL,
                ImageName="120-korindes.webp"
            },
    ];
    };

       
}
