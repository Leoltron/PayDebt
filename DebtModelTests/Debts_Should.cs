using System;
using System.Collections.Generic;
using System.Globalization;
using DebtModel;
using FakeItEasy;
using FluentAssertions;
using Infrastructure;
using NUnit.Framework;

namespace DebtModelTests
{
    [TestFixture]
    public class Debts_Should
    {
        public IEntityStorageAccess<int, Debt> StorageAccess { get; set; }
        public Debt[] DebtsArray = {
            new Debt(1, new Contact("Alexander"),
                new Money(5, new Currency("Bucks", CultureInfo.CurrentCulture)), null, DateTime.Now),
            new Debt(3, new Contact("Leonid"),
                new Money(-7, new Currency("Tenge", CultureInfo.CurrentCulture)),
                null, DateTime.Now)
        };

        [SetUp]
        public void SetUp()
        {
            StorageAccess = A.Fake<IEntityStorageAccess<int, Debt>>();
            A.CallTo(() => StorageAccess.LoadEntities())
                .Returns(DebtsArray);
        }

        [Test]
        public void GetNextId_ShouldReturnIntegersFromZero_GivenZeroDebtsFromStorage()
        {
            var debts = DebtModel.Debts.LoadFrom(A.Fake<IEntityStorageAccess<int, Debt>>());

            for (int i = 0; i < 10; i++)
            {
                debts.GetNextId().Should().Be(i);
            }
        }

        [Test]
        public void GetNextId_ShouldReturnIntegersAfterMaxIdOccured_GivenSeveralDebtsFromStorage()
        {
            var debts = DebtModel.Debts.LoadFrom(StorageAccess);

            for (int i = 4; i < 10; i++)
            {
                debts.GetNextId().Should().Be(i);
            }
        }

        [Test]
        public void TryGetDebt_ShouldReturnFalse_ForNonExistingId()
        {
            var debts = DebtModel.Debts.LoadFrom(StorageAccess);

            debts.TryGetDebt(0, out var debt).Should().BeFalse();
        }

        [Test]
        public void TryGetDebt_ShouldReturnTrue_ForExistingId()
        {
            var debts = DebtModel.Debts.LoadFrom(StorageAccess);

            debts.TryGetDebt(1, out var debt).Should().BeTrue();
        }

        [Test]
        public void TryGetDebt_ShouldSetOutDebtProperly_ForExistingId()
        {
            var debts = DebtModel.Debts.LoadFrom(StorageAccess);

            debts.TryGetDebt(1, out var debt);

            debt.Should().Be(DebtsArray[0]);
        }

        [Test]
        public void Add_ShouldSaveDebtInternally()
        {
            var debts = DebtModel.Debts.LoadFrom(StorageAccess);
            var originalDebt = new Debt(2, new Contact("Aidar"), new Money(1000, new Currency("Rubles", CultureInfo.CurrentCulture)), null, DateTime.Now);

            debts.Add(originalDebt, StorageAccess);

            debts.TryGetDebt(2, out var debt);
            debt.Should().Be(originalDebt);
        }

        [Test]
        public void Add_ShouldSaveDebtToStorage()
        {
            var debts = DebtModel.Debts.LoadFrom(StorageAccess);
            var originalDebt = new Debt(2, new Contact("Aidar"), new Money(1000, new Currency("Rubles", CultureInfo.CurrentCulture)), null, DateTime.Now);

            debts.Add(originalDebt, StorageAccess);

            A.CallTo(() => StorageAccess.SaveEntity(originalDebt)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SaveTo_ShouldSaveDebtsToStorage()
        {
            var debts = DebtModel.Debts.LoadFrom(StorageAccess);

            debts.SaveTo(StorageAccess);

            foreach (var debt in DebtsArray)
            {
                A.CallTo(() => StorageAccess.SaveEntity(debt)).MustHaveHappenedOnceExactly();
            }
        }
    }
}