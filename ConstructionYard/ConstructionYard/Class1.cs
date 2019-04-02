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
        [Test]
        public void CalculatingPV()
        {
            int itemCount = 10;
            double itemValue = 5.8;

            double pv = itemCount * itemValue;

            Assert.AreEqual(58, pv);
        }

        [Test]
        public void CalculatingPVForTheStoneBlock2()
        {
            int itemCount = 10;
            double itemValue = 5.8;

            double pv = itemCount * itemValue;

            double stoneBlockPV = pv * 2;

            Assert.AreEqual(116, stoneBlockPV);
        }
    }
}
