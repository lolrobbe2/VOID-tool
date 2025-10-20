using FoxholeBot.repositories;
using System;

namespace FoxholeBot.types
{
    public static class ExtensionMethods
    {
        public static T As<T>(this Item item) where T : class, new()
        {
            if (typeof(T) == typeof(StockPileItem))
            {
                var stockItem = new StockPileItem
                {
                    Name = item.Name,
                    Crated = item.Crated.Equals("true"),
                    Count = item.Amount

                };

                return stockItem as T;
            }

            throw new InvalidOperationException($"Conversion to type {typeof(T).Name} is not supported.");
        }
    }
}
