using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FoxholeBot.types
{
    public class InvetoryReport
    {
        Item[] Items { get; set; }
        public InvetoryReport(string fileText) 
        {
            Items = fileText.Split('\r').Select(rowText =>new Item(rowText)).ToArray();
        }

        public Item GetItem(string name)
        {
            return Items.FirstOrDefault(item => item.Name == name);
        }

        public Item[] GetStockpile(string name)
        {
            return Items.Where(item => item.StockpileName == name).ToArray();
        }
    }
}
