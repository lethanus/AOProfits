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

        [TestCase(10, new double[] { 5.8, 7.1, 9.2 }, 58)]
        public void CalcMinValue(int itemCount, double[] itemPrices, double expectedMinValue)
        {
            double minValue = MinValue(itemCount, itemPrices);

            Assert.AreEqual(expectedMinValue, minValue);
        }

        [TestCase(10, new double[] { 5.8, 7.1, 9.2 }, 116)]
        public void CalcMinValueForTheStoneBlock2(int itemCount, double[] itemPrices, double expectedMinValue)
        {

            double minValue = MinValue(itemCount, itemPrices);

            double stoneBlockPV = minValue * 2;

            Assert.AreEqual(expectedMinValue, stoneBlockPV);
        }

        [TestCase(10, new double[] { 5.8, 7.1, 9.2 }, 92)]
        public void CalcMaxValue(int itemCount, double[] itemPrices, double expectedMaxValue)
        {
            double minValue = MaxValue(itemCount, itemPrices);

            Assert.AreEqual(expectedMaxValue, minValue);
        }

        [TestCase(10, new double[] { 5.8, 7.1, 9.2 }, 184)]
        public void CalcMaxValueForTheStoneBlock2(int itemCount, double[] itemPrices, double expectedMaxValue)
        {

            double minValue = MaxValue(itemCount, itemPrices);

            double stoneBlockPV = minValue * 2;

            Assert.AreEqual(expectedMaxValue, stoneBlockPV);
        }

        private double MinValue(int count, double[] prices)
        {
            return count * prices.Min();
        }

        private double MaxValue(int count, double[] prices)
        {
            return count * prices.Max();
        }
    }
}
