using System;
using System.Collections.Generic;
using Nest;
using Elasticsearch.Net;

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

        public override string ToString()
        {
            return $"[{PushTime}]:{ItemName} for {Price} in {Location}";
        }
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
