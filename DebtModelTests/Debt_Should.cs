using System;
using System.Globalization;
using DebtModel;
using FluentAssertions;
using NUnit.Framework;

namespace DebtModelTests
{
    [TestFixture]
    public class Debt_Should
    {
        public Currency currency = new Currency("wfen", CultureInfo.CurrentCulture);

        [Test]
        public void IsMyDebt_ShouldBeTrue_OnNegativeAmount()
        {
            new Debt(0, new Contact("oifed"), new Money(-3, currency), "", DateTime.Now)
                .IsMyDebt.Should().BeTrue();
        }

        [Test]
        public void IsMyDebt_ShouldBeFalse_OnPositiveAmount()
        {
            new Debt(0, new Contact("oifed"), new Money(3, currency), "", DateTime.Now)
                .IsMyDebt.Should().BeFalse();
        }

        [Test]
        public void IsTheirDebt_ShouldBeFalse_OnNegativeAmount()
        {
            new Debt(0, new Contact("oifed"), new Money(-3, currency), "", DateTime.Now)
                .IsTheirDebt.Should().BeFalse();
        }

        [Test]
        public void IsTheirDebt_ShouldBeTrue_OnPositiveAmount()
        {
            new Debt(0, new Contact("oifed"), new Money(3, currency), "", DateTime.Now)
                .IsTheirDebt.Should().BeTrue();
        }

        [Test]
        public void Constructor_ShouldThrow_OnZeroMoney()
        {
            Action create = () =>
            {
                var t = new Debt(0, new Contact("oifed"), new Money(0, currency), "", DateTime.Now);
            };

            create.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Constructor_ShouldSetHasPaymentDateTrue_OnGivenPaymentDate()
        {
            new Debt(0, new Contact("oifed"), new Money(3, currency), "", DateTime.Now, DateTime.Now)
                .HasPaymentDate.Should().BeTrue();
        }

        [Test]
        public void Constructor_ShouldSetHasPaymentDateFalse_OnGivenPaymentDate()
        {
            new Debt(0, new Contact("oifed"), new Money(3, currency), "", DateTime.Now)
                .HasPaymentDate.Should().BeFalse();
        }
    }
}