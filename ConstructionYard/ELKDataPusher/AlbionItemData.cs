using System;

namespace ELKDataPusher
{
    public class AlbionItemData
    {
        public int Id { get; set; }
        public DateTime PushTime { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        public string Location { get; set; }
        public int Tier { get; set; }
        public string Category { get; set; }
        public string Bussines { get; set; }

        public AlbionItemData()
        {

        }
        public AlbionItemData(string name, DateTime pushTime, string location, int price)
        {
            ItemName = name;
            PushTime = pushTime;
            Location = location;
            Price = price;
            Id = Int32.Parse($"{pushTime.Month}{pushTime.Day}{pushTime.Hour}{pushTime.Minute}");
            Tier = AlbionItemMappingsHelper.GetItemTier(ItemName);
            Category = AlbionItemMappingsHelper.GetItemCategory(ItemName);
            Bussines = AlbionItemMappingsHelper.GetItemBussines(ItemName);
        }

        public override string ToString()
        {
            return $"[{PushTime}]:{ItemName} for {Price} in {Location}";
        }
    }
}
