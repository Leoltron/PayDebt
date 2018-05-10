using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayDebt;

namespace PayDebtTests
{
    [TestClass]
    public class MoneyTests
    {
        [TestMethod]
        public void TestToString()
        {
            var money = new Money(10, Currency.AmericanDollars);
            Assert.AreEqual("$10.00", money.ToString());
        }
    }
}
