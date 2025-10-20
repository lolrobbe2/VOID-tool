using BeleidsPlanApi.src.database.Repo;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;
#nullable enable
namespace FoxholeBot.repositories
{
    [FirestoreData]
    public class StockPileItem
    {
        [FirestoreProperty]
        public required UInt16 Count { get; set; }

        [FirestoreProperty]
        public required string Name { get; set; }

        [FirestoreProperty]
        public required bool Crated { get; set; }

    }

    [FirestoreData]
    public class StockPileDocument
    {
        [FirestoreProperty]
        public StockPileItem[] Items { get; set; } = Array.Empty<StockPileItem>();
    }

    public class StockPileItemsRepository
    {
        Cache<StockPileItem[]> _cache;
        CollectionReference _stockpileItemsCollection;

        public StockPileItemsRepository(IMemoryCache cache, FirestoreDb firestore)
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
        public async Task<StockPileItem[]> GetStockPileItemsAsync(StockPile stockpile)
        {
            return await GetStockPileItemsAsync(stockpile.Region,stockpile.Subregion,stockpile.Name);
        }

        public async Task<StockPileItem[]> SetStockPileItemsAsync(string region, string subregion, string name, StockPileItem[] items)
        {
            string stockpileName = $"{region}_{subregion}_{name}";
            DocumentReference stockpileDocument = _stockpileItemsCollection.Document(stockpileName);
            await stockpileDocument.SetAsync(new StockPileDocument
            {
                Items = items
            });
            return _cache.CacheSet(items,stockpileName);
        }
        public async Task<StockPileItem[]> SetStockPileItemsAsync(StockPile stockpile, StockPileItem[] items)
        {
            return await SetStockPileItemsAsync(stockpile.Region, stockpile.Subregion, stockpile.Name, items);
        }

        public async Task<StockPileItem[]> AddStockpileItems(string region, string subregion, string name, StockPileItem[] items)
        {
            StockPileItem[] stockPileItems = await GetStockPileItemsAsync(region,subregion,name);
            List<StockPileItem> mergedStockPileItems = new List<StockPileItem>();
            foreach (StockPileItem item in stockPileItems)
            {
                StockPileItem? duplicate = items.FirstOrDefault(duplicate => duplicate.Name == item.Name);
                if(duplicate != null) {
                    item.Count += duplicate.Count;
                }
                mergedStockPileItems.Add(item);
            }

            return await SetStockPileItemsAsync(region,subregion,name,mergedStockPileItems.ToArray());
        }

        public  async Task<StockPileItem[]> AddStockpileItems(StockPile stockpile ,StockPileItem[] items)
        {
            return await AddStockpileItems(stockpile.Region,stockpile.Subregion,stockpile.Name,items);
        }

        public async Task<StockPileItem[]> RemoveStockpileItems(string region, string subregion, string name, StockPileItem[] items)
        {
            StockPileItem[] stockPileItems = await GetStockPileItemsAsync(region, subregion, name);
            List<StockPileItem> mergedStockPileItems = new List<StockPileItem>();
            foreach (StockPileItem item in stockPileItems)
            {
                StockPileItem? duplicate = items.FirstOrDefault(duplicate => duplicate.Name == item.Name);
                if (duplicate != null)
                {
                    if (duplicate.Count >= item.Count) 
                        continue;
                    
                    item.Count -= duplicate.Count;
                }
                mergedStockPileItems.Add(item);
            }

            return await SetStockPileItemsAsync(region,subregion,name,mergedStockPileItems.ToArray());
        }
        public async Task<StockPileItem[]> RemoveStockpileItems(StockPile stockpile, StockPileItem[] items)
        {
            return await RemoveStockpileItems(stockpile.Region, stockpile.Subregion, stockpile.Name, items);
        }
        public async Task ClearStockpileItems(StockPile stockPile)
        {
            await ClearStockpileItems(stockPile.Region, stockPile.Subregion, stockPile.Name);
        }

        public async Task ClearStockpileItems(string region, string subregion, string name)
        {
            string stockpileName = $"{region}_{subregion}_{name}";
            _cache.CacheInvalidate(stockpileName);
            DocumentReference stockpileDocument = _stockpileItemsCollection.Document(stockpileName);
           await stockpileDocument.DeleteAsync();
        }
    }
}