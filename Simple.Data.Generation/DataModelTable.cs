using System;
using System.Collections.Concurrent;

namespace Simple.Data.Generation
{
    public class DataModelTable
    {
        private readonly ConcurrentDictionary<string, DataModelTableColumn> columns =
            new ConcurrentDictionary<string, DataModelTableColumn>(StringComparer.InvariantCultureIgnoreCase);

        public DataModelTableColumn GetColumn(string columnName)
        {
            DataModelTableColumn column = null;
            if (columns.TryGetValue(columnName, out column)) return column;
            return column;
        }
    }
}