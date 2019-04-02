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

        [TestCase(10, new int[] { 58, 71, 92 }, 580)]
        public void CalcMinValue(int itemCount, int[] itemPrices, int expectedMinValue)
        {
            int minValue = MinValue(itemCount, itemPrices);

            Assert.AreEqual(expectedMinValue, minValue);
        }

        [TestCase(10, new int[] { 58, 71, 92 }, 1160)]
        public void CalcMinValueForTheStoneBlock2(int itemCount, int[] itemPrices, int expectedMinValue)
        {

            int minValue = MinValue(itemCount, itemPrices);

            int stoneBlockPV = minValue * 2;

            Assert.AreEqual(expectedMinValue, stoneBlockPV);
        }

        [TestCase(10, new int[] { 58, 71, 92 }, 920)]
        public void CalcMaxValue(int itemCount, int[] itemPrices, int expectedMaxValue)
        {
            int minValue = MaxValue(itemCount, itemPrices);

            Assert.AreEqual(expectedMaxValue, minValue);
        }

        [TestCase(10, new int[] { 58, 71, 92 }, 1840)]
        public void CalcMaxValueForTheStoneBlock2(int itemCount, int[] itemPrices, int expectedMaxValue)
        {

            int minValue = MaxValue(itemCount, itemPrices);

            int stoneBlockPV = minValue * 2;

            Assert.AreEqual(expectedMaxValue, stoneBlockPV);
        }

        private int MinValue(int count, int[] prices)
        {
            return count * prices.Min();
        }

        private int MaxValue(int count, int[] prices)
        {
            return count * prices.Max();
        }
    }
}
