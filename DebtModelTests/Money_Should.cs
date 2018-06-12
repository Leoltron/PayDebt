using System;
using System.Globalization;
using DebtModel;
using FluentAssertions;
using NUnit.Framework;

namespace DebtModelTests
{
    [TestFixture]
    public class Money_Should
    {
        Currency oneCurrency = new Currency("one", CultureInfo.CurrentCulture);
        Currency otherCurrency = new Currency("other", CultureInfo.CurrentCulture);

        [Test]
        public void OperatorSum_ShouldThrow_OnDifferentCurrencies()
        {
            var one = new Money(1, oneCurrency);
            var other = new Money(1, otherCurrency);
            Action sum = () =>
            {
                var t = one + other;
            };

            sum.Should().Throw<ArgumentException>();
        }

        [Test]
        public void OperatorSubstraction_ShouldThrow_OnDifferentCurrencies()
        {
            var one = new Money(1, oneCurrency);
            var other = new Money(1, otherCurrency);
            Action sum = () =>
            {
                var t = one - other;
            };

            sum.Should().Throw<ArgumentException>();
        }

        [Test]
        public void OperatorSum_ShouldSubstractKeepCurrency()
        {
            var one = new Money(1, oneCurrency);
            var other = new Money(1, oneCurrency);

            (one + other).Currency.Should().Be(oneCurrency);
        }

        [Test]
        public void OperatorSubstraction_ShouldKeepCurrency()
        {
            var one = new Money(1, oneCurrency);
            var other = new Money(1, oneCurrency);

            (one - other).Currency.Should().Be(oneCurrency);
        }

        [Test]
        public void OperatorSum_ShouldSumValues()
        {
            var one = new Money(4, oneCurrency);
            var other = new Money(1, oneCurrency);

            (one + other).Value.Should().Be(5);
        }

        [Test]
        public void OperatorSubstract_ShouldSubstractValues()
        {
            var one = new Money(4, oneCurrency);
            var other = new Money(1, oneCurrency);

            (one - other).Value.Should().Be(3);
        }

        [Test]
        public void OperatorEqual_ShouldReturnTrue_OnSelf()
        {
            var one = new Money(4, oneCurrency);

            (one == one).Should().BeTrue();
        }

        [Test]
        public void OperatorEqual_ShouldReturnTrue_OnSame()
        {
            var one = new Money(4, oneCurrency);
            var other = new Money(4, oneCurrency);

            (one == other).Should().BeTrue();
        }

        [Test]
        public void OperatorEqual_ShouldReturnFalse_OnDiffentCurrencies()
        {
            var one = new Money(4, oneCurrency);
            var other = new Money(4, otherCurrency);

            (one == other).Should().BeFalse();
        }

        [Test]
        public void OperatorEqual_ShouldReturnFalse_OnDiffentValues()
        {
            var one = new Money(4, oneCurrency);
            var other = new Money(3, oneCurrency);

            (one == other).Should().BeFalse();
        }
    }
}
