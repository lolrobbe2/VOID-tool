using System.Linq;

namespace FoxholeBot.repositories
{
    public class AssetRepository
    {
        public static string GetAssetFolderLink(string folder, string name)
        {
            return Config.GetAssetUri() + $"/foxhole-void-bot/assets/{folder}/{name}";
        }
        public static string[] GetAssetFileLinks(string folder, string[] files)
        {
            return files.Select(file => GetAssetFolderLink(folder, file)).ToArray();
        }
        public static string[] GetCarouselVideos()
        {
            string[] files = [
                 "demo1.webm",
                "demo2.webm"
                 ];
            return GetAssetFileLinks("videos", files);
        }

        public static string[] GetMapImages()
        {
            string[] files = [
                "MapAshFieldsHex.webp",
                "MapBasinSionnachHex.webp",
                "MapCallahansPassageHex.webp",
                "MapCallumsCapeHex.webp",
                "MapClahstraHexMap.webp",
                "MapClansheadValleyHex.webp",
                "MapDeadLandsHex.webp",
                "MapDrownedValeHex.webp",
                "MapEndlessShoreHex.webp",
                "MapFarranacCoastHex.webp",
                "MapFishermansRowHex.webp",
                "MapGodcroftsHex.webp",
                "MapGreatMarchHex.webp",
                "MapHeartlandsHex.webp",
                "MapHowlCountyHex.webp",
                "MapKalokaiHex.webp",
                "MapKingsCageHex.webp",
                "MapLinnMercyHex.webp",
                "MapLochMorHex.webp",
                "MapMarbanHollow.webp",
                "MapStemaLandingHex.webp"
            ];
            return GetAssetFileLinks("map", files);

        }

        public static string[] GetFlagGifs()
        {
            string[] files = [
                "WindFlagAnim1.gif",
                "WindFlagAnim2.gif",
                "WindFlagAnim3.gif",
                "WindFlagAnim4.gif",
                "WindFlagAnim5.gif",
                ];
            return GetAssetFileLinks("arty/wind/flag", files);
        }

        public static string[] GetWindSockGifs()
        {
            string[] files = [
                "WindSockAnim1.gif",
                "WindSockAnim2.gif",
                "WindSockAnim3.gif",
                "WindSockAnim4.gif",
                ];
            return GetAssetFileLinks("arty/wind/windsock", files);
        }

        public static string[] GetCollonialArtilleryImages()
        {
            string[] files = [
                "120-korindes.webp",
                "trident.webp",
                "conqueror.webp",
                "titan.webp",
                "thunderbolt.webp",
                "sarissa.webp",
                "tempest-cannon"
                ];

            return GetAssetFileLinks("arty/Gun/Collonial", files);
        }
    }
}
