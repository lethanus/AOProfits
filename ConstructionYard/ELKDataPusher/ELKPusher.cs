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
        public void PushItemData(ICollection<AlbionItemData> dataToPush, bool recreate = false)
        {
            if (dataToPush.Count > 0)
            {
                string indexName = "albion-item";
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
                    .Mappings(m => m.Map<AlbionItemData>(mp => mp.AutoMap())));
                }
                foreach (var obj in dataToPush)
                {
                    client.IndexDocument<AlbionItemData>(obj);
                    //Console.WriteLine("Done!!! " + obj);
                }
                Console.WriteLine($"Writen {dataToPush.Count} items for index = {indexName}");
            }
        }

        public void PushItemComparisonData(ICollection<AlbionItemDataComparison> dataToPush, bool recreate = false)
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
                .Mappings(m => m.Map<AlbionItemDataComparison>(mp => mp.AutoMap())));
            }
            foreach (var obj in dataToPush)
            {
                client.IndexDocument<AlbionItemDataComparison>(obj);
                //Console.WriteLine("Done!!! " + obj);
            }
            Console.WriteLine($"Writen {dataToPush.Count} items for index = {indexName}");
        }

        public IReadOnlyCollection<AlbionItemData> GetItemData(string indexName, DateTime date)
        {
            var client = CreateClient(indexName);
            var searchDate = new DateTime(date.Year, date.Month, date.Day);
            var searchResponse = client.Search<AlbionItemData>(s => s.From(0).Size(10000)
                .Query(q => q.DateRange(r => r
                        .Field(f => f.PushTime)
                        .GreaterThanOrEquals(searchDate)
                        .LessThan(searchDate.AddDays(1))
                    )
                )
            );
            

            Console.WriteLine($"Got {searchResponse.Documents.Count} items for index = {indexName}");
            return searchResponse.Documents;
        }
    }
}
