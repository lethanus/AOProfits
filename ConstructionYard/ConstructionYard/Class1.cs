using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace ConstructionYard
{
    public class Class1
    {

        [TestCase(10, "Rock2", 580)]
        public void CalcMinValue(int itemCount, string productCode, int expectedMinValue)
        {
            int minValue = MinValue(itemCount, GetPrices(productCode));

            Assert.AreEqual(expectedMinValue, minValue);
        }

        [TestCase(10, "Rock2", 580)]
        public void CalcMinValueForTheStoneBlock2(int itemCount, string productCode, int expectedMinValue)
        {

            int minValue = MinValue(itemCount, GetPrices(productCode));

            int stoneBlockPV = minValue * 1;

            Assert.AreEqual(expectedMinValue, stoneBlockPV);
        }

        [TestCase(10, "Rock2", 920)]
        public void CalcMaxValue(int itemCount, string productCode, int expectedMaxValue)
        {
            int minValue = MaxValue(itemCount, GetPrices(productCode));

            Assert.AreEqual(expectedMaxValue, minValue);
        }

        [TestCase(10, "Rock2", 920)]
        [TestCase(10, "Rock3", 1840+920)]
        public void CalcMaxValueForTheStoneBlock2(int itemCount, string productCode, int expectedMaxValue)
        {

            int minValue = MaxValue(itemCount, GetPrices(productCode));

            int stoneBlockPV = minValue * 1;

            Assert.AreEqual(expectedMaxValue, stoneBlockPV);
        }

        private int MinValue(int count, IEnumerable<int> prices)
        {
            return count * prices.Min();
        }

        private int MaxValue(int count, IEnumerable<int> prices)
        {
            return count * prices.Max();
        }

        private IEnumerable<int> GetPrices(string itemName)
        {
            switch(itemName)
            {
                case "Rock2": return new int[] { 58, 71, 92 }; break;
                case "Rock3": return new int[] { 58, 71, 92 }; break;
            }
            return null;
        }
    }
}
