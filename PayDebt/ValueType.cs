using System.Linq;
using System.Reflection;
using System.Text;

namespace PayDebt
{
    public class ValueType<T>
    {
        private static PropertyInfo[] properties = null;
        private int hashCode;
        private bool calculated = false;

        private PropertyInfo[] GetProperties()
        {
            return properties ??
                   (properties = GetType()
                       .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .OrderBy(p => p.Name).ToArray());
        }

        public bool Equals(T other)
        {
            if (other == null) return false;
            if (other.GetType() != GetType()) return false;
            return GetProperties()
                .All(p => Equals(p.GetValue(this), p.GetValue(other)));
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            return (bool)obj
                .GetType()
                .GetMethod("Equals", new[] { obj.GetType() })
                .Invoke(obj, new object[] { this });
        }

        public override int GetHashCode()
        {
            if (calculated)
                return hashCode;
            calculated = true;
            return hashCode = GetProperties()
                .Select(p => p.GetValue(this).GetHashCode())
                .Aggregate((x, y) => x * 397 ^ y);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(GetType().Name);
            builder.Append('(');
            builder.Append(string.Join("; ", GetProperties().Select(p => p.Name + ": " + p.GetValue(this))));
            builder.Append(')');
            return builder.ToString();
        }
    }
}