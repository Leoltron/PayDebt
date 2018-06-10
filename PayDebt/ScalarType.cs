using Java.Lang;

namespace PayDebt
{
    public class ScalarType<TValue, TUnit> : ValueType<ScalarType<TValue, TUnit>>
    {
        public TUnit Unit { get; }
        public TValue Value { get; }

        protected static void CheckUnitEquals
            (ScalarType<TValue, TUnit> first, ScalarType<TValue, TUnit> second)
        {
            if (Equals(first.Unit, second.Unit)) return;
            throw new IllegalArgumentException(
                $"Unit of measurement of {first} ({first.Unit}) and {second} ({second.Unit}) are not equal");
        }

        

    }
}