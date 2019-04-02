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
        [TestCase(10,5.8,58)]
        public void CalculatingPV(int itemCount, double itemValue, double expectedPV)
        {
            double pv = PV(itemCount, itemValue);

            Assert.AreEqual(expectedPV, pv);
        }

        [TestCase(10, 5.8, 116)]
        public void CalculatingPVForTheStoneBlock2(int itemCount, double itemValue, double expectedPV)
        {

            double pv = PV(itemCount, itemValue);

            double stoneBlockPV = pv * 2;

            Assert.AreEqual(expectedPV, stoneBlockPV);
        }


        private double PV(int count, double price)
        {
            return count * price;
        }
    }
}
