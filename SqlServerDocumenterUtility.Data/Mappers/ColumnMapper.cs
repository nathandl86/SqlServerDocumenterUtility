using SqlServerDocumenterUtility.Models;
using System;
using System.Data;

namespace SqlServerDocumenterUtility.Data.Mappers
{
    public class ColumnMapper : IColumnMapper
    {
        public ColumnModel Map(IDataRecord record) 
        {
            return new ColumnModel
            {
                TableId = Convert.ToInt64(record["ObjectId"]),
                TableName = record["TableName"].ToString(),
                SchemaId = Convert.ToInt32(record["SchemaId"]),
                SchemaName = record["SchemaName"].ToString(),
                ColumnId = Convert.ToInt64(record["ColumnId"]),
                ColumnName = record["ColumnName"].ToString()
            };
        }
    }
}
