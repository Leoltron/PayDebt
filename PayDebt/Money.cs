using System;

namespace PayDebt
{
    public class Money : ScalarType<decimal, Currency>
    {
        public decimal Amount => Value;
        public Currency Currency => Unit;

        public Money(decimal amount, Currency currency)
            : base(amount, currency)
        {
        }

        public override string ToString()
        {
            return Math.Abs(Amount).ToString("C", Currency);
        }

        public static Money operator +(Money m1, Money m2)
        {
            CheckUnitEquals(m1, m2);
            return new Money(m1.Amount + m2.Amount, m1.Currency);
        }

        public static Money operator -(Money m1, Money m2)
        {
            CheckUnitEquals(m1, m2);
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
    }
}