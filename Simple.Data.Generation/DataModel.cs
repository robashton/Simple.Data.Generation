using System;
using System.Collections.Concurrent;

namespace Simple.Data.Generation
{
    public class DataModel
    {
        private readonly ConcurrentDictionary<string, DataModelTable> tables =
            new ConcurrentDictionary<string, DataModelTable>(StringComparer.InvariantCultureIgnoreCase);

        public DataModelTable Table(string tableName)
        {
            DataModelTable table = null;
            if (tables.TryGetValue(tableName, out table)) return table;

            table = new DataModelTable(tableName);
            tables[tableName] = table;
            return table;
        }
    }
}