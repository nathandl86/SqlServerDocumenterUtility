using SqlServerDocumenterUtility.Models;
using System;
using System.Data;

namespace SqlServerDocumenterUtility.Data.Mappers
{
    public class TableMapper : ITableMapper
    {
        public TableModel Map(IDataRecord record)
        {
            return new TableModel
            {
                TableId = Convert.ToInt64(record["ObjectId"]),
                TableName = record["ObjectName"].ToString(),
                SchemaId = Convert.ToInt32(record["SchemaId"]),
                SchemaName = record["SchemaName"].ToString()
            };
        }
    }
}
