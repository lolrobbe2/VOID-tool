using BeleidsPlanApi.src.database.Repo;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace FoxholeBot.repositories
{
    [FirestoreData]
    public class StockPile
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Region { get; set; }

        [FirestoreProperty]
        public string Subregion { get; set; }

        [FirestoreProperty]
        public string Code { get; set; }
    }

    public class StockpilesRepository
    {
        Cache<StockPile[]> _cache;
        CollectionReference _stockpilesCollection;
        public StockpilesRepository(IMemoryCache cache, FirestoreDb firestore) 
        {
            _cache = new Cache<StockPile[]>(cache);
            _stockpilesCollection = firestore.Collection("stockpiles");
        }
        public async Task<StockPile[]> GetAllStockpilesAsync()
        {
            QuerySnapshot snapshot = await _stockpilesCollection.GetSnapshotAsync();

            StockPile[] stockpiles = snapshot.Documents
                .Select(doc => doc.ConvertTo<StockPile>())
                .ToArray();
            return stockpiles;
        }

        public async Task CreateStockPile(string name, string region, string subregion,string code)
        {
            DocumentReference docRef = _stockpilesCollection.Document($"{region}_{subregion}_{name}");
            StockPile item = new StockPile();
            item.Name = name;
            item.Region = region;
            item.Subregion = subregion;
            item.Code = code;

            await docRef.CreateAsync(item);
        }

        public async Task<StockPile[]> GetRegionStockpiles(string region)
        {
            if(_cache.CacheGet(region,out StockPile[] stockPiles)) 
            {
                return stockPiles;
            }
            return _cache.CacheSet((await GetAllStockpilesAsync()).Where(stockpile => stockpile.Region == region).ToArray(), region);            
        }

        public async Task<StockPile[]> RemoveStockpile(string region, string name)
        {
            StockPile? stockpile = (await GetRegionStockpiles(region)).First(stockpile => stockpile.Name == name);
            if (stockpile == null) return await GetRegionStockpiles(region);
            DocumentReference docRef = _stockpilesCollection.Document($"{stockpile.Region}_{stockpile.Subregion}_{stockpile.Name}");
            await docRef.DeleteAsync();

            _cache.CacheInvalidate(region);
            return await GetRegionStockpiles(region);
        }

        public async Task<string[]> GetStockpileRegions()
        {
            return (await GetAllStockpilesAsync()).Select(stockpile => stockpile.Region).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }
    }
}
