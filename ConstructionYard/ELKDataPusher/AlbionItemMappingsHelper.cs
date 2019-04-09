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
}
