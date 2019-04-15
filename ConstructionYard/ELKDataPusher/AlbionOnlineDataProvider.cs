using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace ELKDataPusher
{
    public class AlbionOnlineDataProvider
    {
        public List<AlbionItemData> GetData(string itemCode)
        {
            var data = new List<AlbionItemData>();
            var json = GetAsync($"https://www.albion-online-data.com/api/v2/stats/prices/{itemCode}").Result;

            var items = JsonConvert.DeserializeObject<List<AlbionOnlineData>>(json);
            foreach (var x in items)
            {
                //Console.WriteLine(x);
                data.Add(new AlbionItemData(x.item_id, DateTime.Now, x.city, x.sell_price_min, x.sell_price_max, x.quality));
            }
            return data;
        }

        private async Task<string> GetAsync(string path)
        {
            string json = "";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
            }
            return json;
        }

    }
}
