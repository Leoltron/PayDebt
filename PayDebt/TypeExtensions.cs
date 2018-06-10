using System;

namespace PayDebt
{
    public static class TypeExtensions
    {
        public static bool IsEqualOrSubclassOf(this Type type, Type otherType)
        {
            return type == otherType || type.IsSubclassOf(otherType);
        }
    }
}