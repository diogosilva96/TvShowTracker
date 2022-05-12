using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Infrastructure.Utilities
{
    public static class CsvConverter
    {
        public static byte[] GetCsvBytes<T>(IEnumerable<T> data) where T : class => Encoding.UTF8.GetBytes(BuildCsvString(data));

        private static string BuildCsvString<T>(IEnumerable<T> data) where T : class
        {
            //very simple implementation of CSV to model parsing, does not handle every case
            var stringBuilder = new StringBuilder();
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);
            writeHeaders();
            writeData();

            return stringBuilder.ToString();

            void writeHeaders() => stringBuilder.AppendLine(string.Join(", ", propertyInfos.Select(p => p.Name)));
            
            void writeData()
            {
                foreach (var item in data)
                {
                    stringBuilder.AppendLine(string.Join(", ", propertyInfos.Select(p => p.GetValue(item, null))));
                }
            }
        }
    }
}
