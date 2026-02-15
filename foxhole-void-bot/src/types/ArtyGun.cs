using System;

namespace FoxholeBot.types
{
    public enum Faction : byte
    {
        COLONIAL,
        WARDEN
    }

    public enum Type : UInt16
    {
        SMALL_SHELL = 120,
        MEDIUM_SHELL = 150,
        LARGE_SHELL = 300,
        ROCKET = 30
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
        //how many shells can be fired per minute maximum
        public float FireRate;

        public Type Type;

    };
    public class Guns {
        public static ArtyGun[] artyGuns = [
            new ()
            {
                Name = "“Korindes” field gun",
                Bias = 10,
                MinRange = 100,
                MaxRange = 250,
                InacuracyMin = 22.5f,
                InacuracyMax = 30f,
                Faction = Faction.COLONIAL,
                ImageName="120-korindes.webp",
                FireRate = 9.6f,
                Type = Type.SMALL_SHELL
            },
            new (){
                Name = "AC-b “Trident”",
                Bias = 10,
                MinRange = 100,
                MaxRange = 225,
                InacuracyMin = 2.5f,
                InacuracyMax = 8.5f,
                Faction = Faction.COLONIAL,
                ImageName = "trident.webp",
                FireRate= 18.75f,
                Type = Type.SMALL_SHELL
            },
            new (){
                Name = "Conqueror",
                Bias = 10,
                MinRange = 100,
                MaxRange = 200,
                InacuracyMin = 2.5f,
                InacuracyMax = 8.5f,
                Faction = Faction.COLONIAL,
                ImageName = "conqueror.webp",
                FireRate=27.3f,
                Type=Type.SMALL_SHELL
            },
            new (){
                Name="Titan 120",
                Bias=10,
                MinRange = 100,
                MaxRange = 200,
                InacuracyMin = 2.5f,
                InacuracyMax = 8.5f,
                Faction = Faction.COLONIAL,
                ImageName="titan.webp",
                FireRate=27.3f,
                Type=Type.SMALL_SHELL
            },
            new(){
                Name="50-500 “Thunderbolt” Cannon",
                Bias=10,
                MinRange=200,
                MaxRange=350,
                InacuracyMin=32.5f,
                InacuracyMax=40f,
                Faction= Faction.COLONIAL,
                ImageName="thunderbolt.webp",
                FireRate=7.1f,
                Type=Type.MEDIUM_SHELL
            },
            new(){
                Name="Lance-46 “Sarissa”",
                Bias=10,
                MinRange=120,
                MaxRange=250,
                InacuracyMin=25f,
                InacuracyMax=35f,
                Faction=Faction.COLONIAL,
                ImageName="sarissa.webp",
                FireRate=20f,
                Type=Type.MEDIUM_SHELL
            },            
            new (){
                Name="R-17 “Retiarius” Skirmisher",
                Bias=10,
                MinRange = 375,
                MaxRange = 500,
                InacuracyMin = 37.5f,
                InacuracyMax = 51f,
                Faction = Faction.COLONIAL,
                ImageName="retiarius.webp",
                FireRate=15f,
                Type=Type.ROCKET
            },
            new (){
                Name="Titan 150",
                Bias=10,
                MinRange = 100,
                MaxRange = 225,
                InacuracyMin = 2.5f,
                InacuracyMax = 8.5f,
                Faction = Faction.COLONIAL,
                ImageName="titan.webp",
                FireRate=27.3f,
                Type=Type.MEDIUM_SHELL
            },
            new(){
                Name="Tempest Cannon RA-2",
                Bias=10,
                MinRange=350,
                MaxRange=500,
                InacuracyMin=50,
                InacuracyMax=50,
                Faction=Faction.COLONIAL,
                ImageName="tempest-cannon.webp",
                FireRate=10f,
                Type=Type.LARGE_SHELL,
            },
             new(){
                Name="Storm Cannon",
                Bias=50,
                MinRange=400,
                MaxRange=1000,
                InacuracyMin=50,
                InacuracyMax=50,
                Faction=Faction.COLONIAL,
                ImageName="storm-cannon.webp",
                FireRate=10f,
                Type=Type.LARGE_SHELL,
            },
            new(){
                Name="Tempest Cannon RA-2",
                Bias=50,
                MinRange=350,
                MaxRange=500,
                InacuracyMin=50,
                InacuracyMax=50,
                Faction=Faction.WARDEN,
                ImageName="tempest-cannon.webp",
                FireRate=10f,
                Type=Type.LARGE_SHELL,
            },
             new(){
                Name="Storm Cannon",
                Bias=50,
                MinRange=400,
                MaxRange=1000,
                InacuracyMin=50,
                InacuracyMax=50,
                Faction=Faction.WARDEN,
                ImageName="storm-cannon.webp",
                FireRate=10f,
                Type=Type.LARGE_SHELL,
            }
    ];
    };

       
}
