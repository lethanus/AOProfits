using System;
using System.IO;
using System.Collections.Generic;
using Nest;
using Elasticsearch.Net;
using System.Linq;

namespace ELKDataPusher
{
    public class ELKPusher
    {
        private ElasticClient CreateClient(string indexName)
        {
            var uris = new[]
            {
                new Uri("http://localhost:9200"),

            };

            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool)
                .DefaultIndex(indexName);
            var client = new ElasticClient(settings);

            return client;
        }
        public void PushItemData(ICollection<AlbionData> dataToPush, bool recreate = false)
        {
            string indexName = "albion-item";
            var client = CreateClient(indexName);
            var indexConfig = new IndexState
            {
                Settings = new IndexSettings { NumberOfReplicas = 0, NumberOfShards = 2}
            };
            if (client.IndexExists(indexName).Exists && recreate)
            {
                client.DeleteIndex(indexName);
            }

            if (!client.IndexExists(indexName).Exists)
            {
                client.CreateIndex(indexName, j => j
                .InitializeUsing(indexConfig)
                .Mappings(m => m.Map<AlbionData>(mp => mp.AutoMap())));
            }
            int i = 0;
            foreach (var obj in dataToPush)
            {
                obj.Id += i++;
                client.IndexDocument<AlbionData>(obj);
                //Console.WriteLine("Done!!! " + obj);
            }
            Console.WriteLine($"Writen {dataToPush.Count} items for index = {indexName}");
        }

        public void PushItemComparisonData(ICollection<AlbionDataComparison> dataToPush, bool recreate = false)
        {
            string indexName = "albion-item-comparison";
            var client = CreateClient(indexName);
            var indexConfig = new IndexState
            {
                Settings = new IndexSettings { NumberOfReplicas = 0, NumberOfShards = 2 }
            };
            if (client.IndexExists(indexName).Exists && recreate)
            {
                client.DeleteIndex(indexName);
            }

            if (!client.IndexExists(indexName).Exists)
            {
                client.CreateIndex(indexName, j => j
                .InitializeUsing(indexConfig)
                .Mappings(m => m.Map<AlbionDataComparison>(mp => mp.AutoMap())));
            }
            int i = 0;
            foreach (var obj in dataToPush)
            {
                obj.Id += i++;
                client.IndexDocument<AlbionDataComparison>(obj);
                //Console.WriteLine("Done!!! " + obj);
            }
            Console.WriteLine($"Writen {dataToPush.Count} items for index = {indexName}");
        }

        public IReadOnlyCollection<AlbionData> GetData(string indexName, DateTime date)
        {
            var client = CreateClient(indexName);

            var searchResponse = client.Search<AlbionData>(s => s.From(0).Size(10000)
                .Query(q => q.DateRange(r => r
                        .Field(f => f.PushTime)
                        .GreaterThanOrEquals(date)
                        .LessThan(date.AddDays(1))
                    )
                )
            );
            

            Console.WriteLine($"Got {searchResponse.Documents.Count} items for index = {indexName}");
            return searchResponse.Documents;
        }
    }

    public class AlbionData
    {
        public int Id { get; set; }
        public DateTime PushTime { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        public string Location { get; set; }
        public int Tier { get; set; }
        public string Category { get; set; }
        public string Bussines { get; set; }

        public AlbionData(string name, DateTime pushTime, string location, int price)
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

    public class AlbionItemMappingsHelper
    {
        private static string path = @"C:\Emil\Projects\AlbionData\itemCategories.txt";
        private static Dictionary<string, AlbionItemMappings> mappings = new Dictionary<string, AlbionItemMappings>();
        public static int GetItemTier(string name)
        {
            var cleanName = name.Replace("[U]", "").Replace("[R]", "").Replace("[E]", "").Trim();
            LoadMappings();
            var key = mappings.Keys.First(x => x.Contains(cleanName));
            return mappings[key].Tier;
        }

        public static string GetItemCategory(string name)
        {
            var cleanName = name.Replace("[U]", "").Replace("[R]", "").Replace("[E]", "").Trim();
            LoadMappings();
            var key = mappings.Keys.First(x => x.Contains(cleanName));
            return mappings[key].Category;
        }

        public static string GetItemBussines(string name)
        {
            var cleanName = name.Replace("[U]", "").Replace("[R]", "").Replace("[E]", "").Trim();
            LoadMappings();
            var key = mappings.Keys.First(x => x.Contains(cleanName));
            return mappings[key].Bussines;
        }

        private static void LoadMappings()
        {
            if(mappings.Keys.Count == 0)
            {
                var lines = File.ReadAllLines(path);
                mappings = lines.Select(x => x.Split(',')).Select(x => new AlbionItemMappings
                {
                    ItemName = x[0],
                    Category = x[1],
                    Bussines = x[2],
                    Tier = Int32.Parse(x[3])
                }
                ).ToDictionary(x => x.ItemName, y => y);
            }
        }
    }
    public class AlbionItemMappings
    {
        public string ItemName { get; set; }
        public int Tier { get; set; }
        public string Category { get; set; }
        public string Bussines { get; set; }
    }

    public class AlbionDataComparison
    {
        public int Id { get; set; }
        public DateTime PushTime { get; set; }
        public string ItemName { get; set; }
        public int PriceSource { get; set; }
        public string LocationSource { get; set; }
        public int PriceDestanation { get; set; }
        public string LocationDestanation { get; set; }
        public int Tier { get; set; }
        public int BuyInSourceSellInDestanationProfit { get; set; }
        public string Category { get; set; }
        public string Bussines { get; set; }

        public override string ToString()
        {
            return $"[{PushTime}]:Buying {ItemName} for {PriceSource} in {LocationSource} and selling for {PriceDestanation} in {LocationDestanation} Profit = {BuyInSourceSellInDestanationProfit}";
        }
    }
}
