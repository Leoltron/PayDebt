using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Infrastructure
{
    public static class BytesExtensions
    {
        public static T FromBinary<T>(this byte[] bytes)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(bytes))
            {
                return (T) formatter.Deserialize(stream);
            }
        }
    }
}