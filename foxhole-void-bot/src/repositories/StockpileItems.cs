using BeleidsPlanApi.src.database.Repo;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace FoxholeBot.repositories
{
    [FirestoreData]
    public class StockPileItem
    {
        [FirestoreProperty]
        public byte Count { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }
    }
    public class StockPileItemsRepositories
    {
        Cache<StockPileItem[]> _cache;
        CollectionReference _stockpileItemsCollection;

        public StockPileItemsRepositories(IMemoryCache cache, FirestoreDb firestore)
        {
            _cache = new Cache<StockPileItem[]>(cache);
            _stockpileItemsCollection = firestore.Collection("StockpileItems");
        }

        public async Task<StockPileItem[]> GetStockPileItemsAsync(string region, string subregion, string name)
        {
            string stockpileName = $"{region}_{subregion}_{name}";
            if (_cache.CacheGet(stockpileName, out StockPileItem[] items))
            {
                return items;
            }

            DocumentSnapshot stockpileDocument = await _stockpileItemsCollection.Document(stockpileName).GetSnapshotAsync();
            return _cache.CacheSet(stockpileDocument.GetValue<StockPileItem[]>("Items"), stockpileName);
        }

        public async Task<StockPileItem[]> AddStockpileItems(string region, string subregion, string name,StockPileItem[] items)
        {

        }
    }
}