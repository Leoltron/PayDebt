using System;

namespace PayDebt
{
    public abstract class ScalarType<TValue, TUnit> 
        : ValueType<ScalarType<TValue, TUnit>> 
        where TUnit : IEquatable<TUnit>
    {
        public TUnit Unit { get; }
        public TValue Value { get; }

        protected ScalarType(TValue value, TUnit unit)
        {
            Unit = unit;
            Value = value;
        }

        protected static void CheckUnitEquals
            (ScalarType<TValue, TUnit> first, ScalarType<TValue, TUnit> second)
        {
            if (Equals(first.Unit, second.Unit)) return;
            throw new ArgumentException(
                $"Unit of measurement of {first} ({first.Unit}) and {second} ({second.Unit}) are not equal");
        }
    }
}