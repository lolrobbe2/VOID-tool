using System;

namespace FoxholeBot.types
{
    public class Item
    {
        private string[] items;
        public string StockpileTitle { get => items[0]; }
        public string StockpileName { get => items[1]; }
        public string SructureType { get => items[2]; }
        public Int16 Amount { get => Int16.Parse(items[3]); }
        public string Name { get => items[4]; }
        public string Crated { get => items[5]; }
        public string PerCrate { get => items[6]; }
        public string Total { get => items[7]; }
        public string Description { get => items[8]; }
        public string CodeName { get => items[8]; } 
        public Item(string rowText)
        {
            items = rowText.Split(' ');//split by tab
        }
    }
}
