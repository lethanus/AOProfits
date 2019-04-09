using System.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ELKDataPusher
{
    class Program
    {
        static void Main(string[] args)
        {
            ELKPusher eLKPusher = new ELKPusher();
            var now = DateTime.Now;

            //PushData(now.AddDays(-1), eLKPusher, @"C:\Emil\Projects\AlbionData\data_2019_04_09.txt");
            //PushData(now, eLKPusher, @"C:\Emil\Projects\AlbionData\data.txt");

            CalculateProfits(now.AddDays(-1), eLKPusher);
            CalculateProfits(now, eLKPusher);



            Console.ReadLine();
        }
        static private void PushData(DateTime dateTime, ELKPusher eLKPusher, string fileName)
        {
            var now = dateTime;
            var items = File.ReadAllLines(fileName).Where(x => !x.StartsWith("===")).Select(x => x.Split(',')).Select(x => 
                new AlbionItemData( x[0], now, x[1], Int32.Parse(x[2]))).ToList();
            eLKPusher.PushItemData(items);

        }

        static private void CalculateProfits(DateTime dateTime, ELKPusher eLKPusher)
        {

            var res = eLKPusher.GetItemData("albion-item", dateTime);
            var grouped = res.GroupBy(x => x.ItemName).ToDictionary(g => g.Key, g => g.ToList());

            var comparisons = new List<AlbionItemDataComparison>();
            foreach (var key in grouped.Keys)
            {
                var group = grouped[key];
                //Console.WriteLine($"Group = {key}");
                if (group.Count > 1)
                {
                    var byLocations = group.GroupBy(x => x.Location).ToDictionary(g => g.Key, g => g.ToList());
                    foreach (var location in byLocations.Keys)
                    {
                        var item = byLocations[location].OrderByDescending(x => x.PushTime).First();
                        foreach (var otherLocation in byLocations.Keys.Where(k => k != location))
                        {
                            var otherItem = byLocations[otherLocation].OrderByDescending(x => x.PushTime).First();
                            //Console.WriteLine($"{item.ItemName} price = {item.Price} in {item.Location} vs price = {otherItem.Price} in {otherItem.Location} - ABS diff = {Math.Abs(item.Price-otherItem.Price)}");
                            comparisons.Add(new AlbionItemDataComparison(item, otherItem, dateTime));
                        }
                    }
                }
            }
            foreach (var i in comparisons)
            {
                Console.WriteLine(i.ToString());
            }
            eLKPusher.PushItemComparisonData(comparisons);
        }
    }
}
