using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FoxholeBot.types
{
    public class InvetoryReport
    {
        Item[] Items { get; set; }
        public InvetoryReport(string fileText) 
        {
            var items = fileText.Split('\n').Skip(1);
            Items = items.Select(rowText =>new Item(rowText)).ToArray();
        }

        public Item GetItem(string name)
        {
            return Items.FirstOrDefault(item => item.Name == name);
        }

        public Item[] GetStockpile(string name)
        {
            return Items.Where(item => item.StockpileName == name).ToArray();
        }

        public Dictionary<string, Item[]> SplitByStockpile()
        {
            return Items
                .GroupBy(item => item.StockpileName)
                .ToDictionary(group => group.Key, group => group.ToArray());
        }
    }
}
