using System;

namespace ELKDataPusher
{
    public class AlbionOnlineData
    {
        public string item_id { get; set; }
        public string city { get; set; }
        public int quality { get; set; }
        public int sell_price_min { get; set; }
        public DateTime sell_price_min_date { get; set; }
        public int sell_price_max { get; set; }
        public DateTime sell_price_max_date { get; set; }
        public int buy_price_min { get; set; }
        public DateTime buy_price_min_date { get; set; }
        public int buy_price_max { get; set; }
        public DateTime buy_price_max_date { get; set; }
        public override string ToString()
        {
            return $"{item_id} {quality} in {city} min price = {sell_price_min} max price = {sell_price_max}";
        }
    }
}
