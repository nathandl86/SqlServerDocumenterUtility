using System.Collections.Generic;

namespace SqlServerDocumenterUtility.Models
{
    public class TableModel
    {
        public long TableId { get; set; }
        public string TableName { get; set; }
        public int SchemaId { get; set; }
        public string SchemaName { get; set; }
        public IEnumerable<ExtendedPropertyModel> Properties { get; set; }
        public IEnumerable<ColumnModel> Columns { get; set; }
    }
}
