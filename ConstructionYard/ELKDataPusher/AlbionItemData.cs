using System;

namespace ELKDataPusher
{
    public class AlbionItemData
    {
        public string Id { get; set; }
        public DateTime PushTime { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int Quality { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public string Location { get; set; }
        public int Tier { get; set; }
        public string Category { get; set; }
        public string Bussines { get; set; }

        public AlbionItemData()
        {

        }
        public AlbionItemData(string itemCode, DateTime pushTime, string location, int min_price, int max_price, int quality)
        {
            ItemName = AlbionItemMappingsHelper.GetItemName(itemCode) + (quality > 1 ? $" ({quality})" : "");
            ItemCode = itemCode;
            PushTime = pushTime;
            Location = location;
            MinPrice = min_price;
            MaxPrice = max_price;
            Quality = quality;
            Id = Guid.NewGuid().ToString("D");
            Tier = AlbionItemMappingsHelper.GetItemTier(itemCode);
            Category = AlbionItemMappingsHelper.GetItemCategory(itemCode);
            Bussines = AlbionItemMappingsHelper.GetItemBussines(itemCode);
        }

        public override string ToString()
        {
            return $"[{PushTime}]:{ItemName}-{ItemCode} Q={Quality} for ({MinPrice}-{MaxPrice}) in {Location}";
        }
    }
}
