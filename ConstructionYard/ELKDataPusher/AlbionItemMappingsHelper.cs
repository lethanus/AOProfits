using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ELKDataPusher
{
    public class AlbionItemMappingsHelper
    {
        private static string path = @"C:\Emil\Projects\AlbionData\itemCategories.txt";
        private static Dictionary<string, AlbionItemMappings> mappings = new Dictionary<string, AlbionItemMappings>();

        public static List<string> GetAllCodes()
        {
            LoadMappings();
            return mappings.Keys.ToList();
        }
        public static int GetItemTier(string itemCode)
        {
            LoadMappings();
            return mappings[itemCode].Tier;
        }

        public static string GetItemCategory(string itemCode)
        {
            LoadMappings();
            return mappings[itemCode].Category;
        }

        internal static string GetItemName(string itemCode)
        {
            LoadMappings();
            return mappings[itemCode].ItemName;
        }

        public static string GetItemBussines(string itemCode)
        {
            LoadMappings();
            return mappings[itemCode].Bussines;
        }

        private static void LoadMappings()
        {
            if(mappings.Keys.Count == 0)
            {
                var lines = File.ReadAllLines(path);
                var results = lines.Select(x => x.Split(',')).Select(x => new AlbionItemMappings
                {
                    ItemCode = x[0],
                    ItemName = x[1],
                    Category = x[2],
                    Bussines = x[3],
                    Tier = Int32.Parse(x[4])
                }
                ).ToList();
                var temp = new List<AlbionItemMappings>();
                foreach(var item in results.Where(x=>x.Tier > 3))
                {
                    temp.Add(new AlbionItemMappings
                    {
                        ItemCode = item.ItemCode + "_LEVEL1@1",
                        ItemName = "[U] " + item.ItemName,
                        Tier = item.Tier,
                        Bussines = item.Bussines,
                        Category = item.Category
                    });
                    temp.Add(new AlbionItemMappings
                    {
                        ItemCode = item.ItemCode + "_LEVEL2@2",
                        ItemName = "[R] " + item.ItemName,
                        Tier = item.Tier,
                        Bussines = item.Bussines,
                        Category = item.Category
                    });
                    temp.Add(new AlbionItemMappings
                    {
                        ItemCode = item.ItemCode + "_LEVEL3@3",
                        ItemName = "[E] " + item.ItemName,
                        Tier = item.Tier,
                        Bussines = item.Bussines,
                        Category = item.Category
                    });
                    temp.Add(new AlbionItemMappings
                    {
                        ItemCode = item.ItemCode + "@1",
                        ItemName = "[U] " +  item.ItemName,
                        Tier = item.Tier,
                        Bussines = item.Bussines,
                        Category = item.Category
                    });
                    temp.Add(new AlbionItemMappings
                    {
                        ItemCode = item.ItemCode + "@2",
                        ItemName = "[R] " + item.ItemName,
                        Tier = item.Tier,
                        Bussines = item.Bussines,
                        Category = item.Category
                    });
                    temp.Add(new AlbionItemMappings
                    {
                        ItemCode = item.ItemCode + "@3",
                        ItemName = "[E] " + item.ItemName,
                        Tier = item.Tier,
                        Bussines = item.Bussines,
                        Category = item.Category
                    });
                }

                results.AddRange(temp);
                mappings = results.ToDictionary(x => x.ItemCode, y => y);
            }
        }
    }
}
