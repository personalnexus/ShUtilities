using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShUtilities.Collections
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> items, bool readOnly = false)
        {
            var result = new DataTable();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                result.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType).ReadOnly = readOnly;
            }

            foreach (T item in items)
            {
                var row = result.NewRow();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    row[propertyInfo.Name] = propertyInfo.GetValue(item);
                }
                result.Rows.Add(row);
            }
            return result;
        }
    }
}
