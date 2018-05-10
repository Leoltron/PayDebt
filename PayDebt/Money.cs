using Java.Lang;

namespace PayDebt
{
    public struct Money
    {
        public decimal Amount { get; }
        public Currency Currency { get; }

        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public override string ToString()
        {
            return Amount < 0 ? (-Amount).ToString("C", Currency) : Amount.ToString("C", Currency);
        }

        private static void CheckCurrencyEquals(Money m1, Money m2)
        {
            if (!Equals(m1.Currency, m2.Currency))
                throw new IllegalArgumentException(
                    $"Currencies of {m1} ({m1.Currency.Name}) and {m2} ({m2.Currency.Name}) are not equal");
        }

        public static Money operator +(Money m1, Money m2)
        {
            CheckCurrencyEquals(m1, m2);
            return new Money(m1.Amount + m2.Amount, m1.Currency);
        }

        public static Money operator -(Money m1, Money m2)
        {
            CheckCurrencyEquals(m1, m2);
            return new Money(m1.Amount - m2.Amount, m1.Currency);
        }

        public static bool operator ==(Money m1, Money m2)
        {
            return Equals(m1, m2);
        }

        public static bool operator !=(Money m1, Money m2)
        {
            return !(m1 == m2);
        }

        public bool Equals(Money other)
        {
            return Amount == other.Amount && Currency.Equals(other.Currency);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Money money && Equals(money);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Amount.GetHashCode() * 397) ^ Currency.GetHashCode();
            }
        }
    }
}