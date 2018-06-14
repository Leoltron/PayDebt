using System.Collections.Generic;
using DebtModel;
using FakeItEasy;
using Infrastructure;
using NUnit.Framework;

namespace DebtModelTests
{
    [TestFixture]
    public class Debts_Should
    {
        public IEntityStorageAccess<int, Debt> StorageAccess { get; set; }

        [SetUp]
        public void SetUp()
        {
            StorageAccess = A.Fake<IEntityStorageAccess<int, Debt>>();
            A.CallTo(() => StorageAccess.LoadEntities())
                .Returns(new[] {new Debt(1, new Contact("Alexander"), ), })
        }

        [Test]
        public void GetNextId_ShouldReturnIntegersFromZero()
        {
            var debts = new Debts(new Dictionary<int, Debt>());
        }
    }
}