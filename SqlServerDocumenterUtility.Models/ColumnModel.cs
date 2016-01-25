using System.Collections.Generic;

namespace SqlServerDocumenterUtility.Models
{
    public class ColumnModel
    {
        public long TableId { get; set; }
        public string TableName { get; set; }
        public long ColumnId { get; set; }
        public string ColumnName {get;set;}
        public int SchemaId { get; set; }
        public string SchemaName { get; set; }
        public IEnumerable<ExtendedPropertyModel> Properties { get; set; }
    }
}
