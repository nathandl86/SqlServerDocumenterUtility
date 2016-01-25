using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDocumenterUtility.Models
{
    public class ExtendedPropertyModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public int? SchemaId { get; set; }
        public string SchemaName { get; set; }
        public long? TableId { get; set; }
        public string TableName { get; set; }
        public long? ColumnId { get; set; }
        public string ColumnName { get; set; }
    }
}
