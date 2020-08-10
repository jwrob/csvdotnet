using System.IO;
using System.Threading.Tasks;

namespace CsvDotNet
{
    public static class CsvReaderExtensions
    {
        public static CsvReader<T> FromCsv<T>(this string input) where T : class
        {
            return new CsvReader<T>(input);
        }

        public static async Task<CsvReader<T>> FromCsv<T>(this Stream stream) where T : class
        {
            using var reader = new StreamReader(stream);
            string unformatted = await reader.ReadToEndAsync();
            return new CsvReader<T>(unformatted);
        }
    }
}
