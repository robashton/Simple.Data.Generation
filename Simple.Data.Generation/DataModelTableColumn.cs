using System;

namespace Simple.Data.Generation
{
    public class DataModelTableColumn
    {
        private readonly string columnName;

        public DataModelTableColumn(string columnName)
        {
            this.columnName = columnName;
        }

        public Type Type { get; private set; }

        public void SetType(Type columnType)
        {
            this.Type = columnType;
        }
    }
}
