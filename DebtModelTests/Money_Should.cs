using System;
using System.Globalization;
using DebtModel;
using FluentAssertions;
using NUnit.Framework;

namespace DebtModelTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class Money_Should
    {
        private readonly Currency oneCurrency = new Currency("one", CultureInfo.CurrentCulture);
        private readonly Currency otherCurrency = new Currency("other", CultureInfo.CurrentCulture);

        [Test]
        public void OperatorSum_ShouldThrowOnDifferentCurrencies()
        {
            var one = new Money(1, oneCurrency);
            var other = new Money(1, otherCurrency);
            Action sum = () =>
            {
                // ReSharper disable once UnusedVariable
                var t = one + other;
            };

            sum.Should().Throw<ArgumentException>();
        }

        [Test]
        public void OperatorSubstraction_ShouldThrowOnDifferentCurrencies()
        {
            var one = new Money(1, oneCurrency);
            var other = new Money(1, otherCurrency);
            Action sum = () =>
            {
                // ReSharper disable once UnusedVariable
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
        public void OperatorEqual_ShouldReturnTrueOnSelf()
        {
            var one = new Money(4, oneCurrency);

            // ReSharper disable once EqualExpressionComparison
#pragma warning disable CS1718 // Выполнено сравнение с той же переменной
            (one == one).Should().BeTrue();
#pragma warning restore CS1718 // Выполнено сравнение с той же переменной
        }

        [Test]
        public void OperatorEqual_ShouldReturnTrueOnSame()
        {
            var one = new Money(4, oneCurrency);
            var other = new Money(4, oneCurrency);

            (one == other).Should().BeTrue();
        }

        [Test]
        public void OperatorEqual_ShouldReturnFalseOnDiffentCurrencies()
        {
            var one = new Money(4, oneCurrency);
            var other = new Money(4, otherCurrency);

            (one == other).Should().BeFalse();
        }

        [Test]
        public void OperatorEqual_ShouldReturnFalseOnDiffentValues()
        {
            var one = new Money(4, oneCurrency);
            var other = new Money(3, oneCurrency);

            (one == other).Should().BeFalse();
        }
    }
}
