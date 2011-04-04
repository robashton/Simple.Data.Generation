using System;
using System.Collections.Concurrent;

namespace Simple.Data.Generation
{
    public class DataModelTable
    {
        private readonly string tableName;

        private readonly ConcurrentDictionary<string, DataModelTableColumn> columns =
            new ConcurrentDictionary<string, DataModelTableColumn>(StringComparer.InvariantCultureIgnoreCase);

        public DataModelTable(string tableName)
        {
            this.tableName = tableName;
        }

        public DataModelTableColumn Column(string columnName)
        {
            DataModelTableColumn column = null;
            if (columns.TryGetValue(columnName, out column)) return column;
            column = new DataModelTableColumn(columnName);
            columns[columnName] = column;
            return column;
        }
    }
}