using System;

namespace ELKDataPusher
{
    public class AlbionItemDataComparison
    {
        public long Id { get; set; }
        public DateTime PushTime { get; set; }
        public string ItemName { get; set; }
        public int PriceSource { get; set; }
        public string LocationSource { get; set; }
        public int PriceDestanation { get; set; }
        public string LocationDestanation { get; set; }
        public int Tier { get; set; }
        public int BuyInSourceSellInDestanationProfit { get; set; }
        public int ProfitRate { get; set; }
        public string Category { get; set; }
        public string Bussines { get; set; }

        public AlbionItemDataComparison(AlbionItemData source, AlbionItemData desination, DateTime pushTime)
        {
            ItemName = source.ItemName;
            PushTime = pushTime;
            LocationSource = source.Location;
            PriceSource = source.MinPrice;
            PriceDestanation = desination.MinPrice;
            LocationDestanation = desination.Location;
            Id = long.Parse($"{pushTime.Month}{pushTime.Day}{pushTime.Hour}{pushTime.Minute}");
            Tier = source.Tier;
            Category = source.Category;
            Bussines = source.Bussines;
            BuyInSourceSellInDestanationProfit = PriceDestanation - PriceSource;
            ProfitRate = (int)((BuyInSourceSellInDestanationProfit * 100.0) / PriceSource);
        }

        public override string ToString()
        {
            return $"[{PushTime}]:Buying {ItemName} for {PriceSource} in {LocationSource} and selling for {PriceDestanation} in {LocationDestanation} Profit = {BuyInSourceSellInDestanationProfit} - {ProfitRate}";
        }
    }
}
