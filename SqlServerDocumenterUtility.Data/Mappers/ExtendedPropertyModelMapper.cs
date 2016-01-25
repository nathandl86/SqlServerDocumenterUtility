using SqlServerDocumenterUtility.Models;
using System;
using System.Data;

namespace SqlServerDocumenterUtility.Data.Mappers
{
    public class ExtendedPropertyModelMapper : IExtendedPropertyModelMapper
    {
        public ExtendedPropertyModel Map(IDataRecord record) {
            return new ExtendedPropertyModel
            {
                TableId = record["ObjectId"] == DBNull.Value ? (long?)null : Convert.ToInt64(record["ObjectId"]),
                SchemaName = record["SchemaName"].ToString(),
                TableName = record["ObjectName"].ToString(),
                ColumnId = record["ColumnId"] == DBNull.Value ? (long?)null : Convert.ToInt64(record["ColumnId"]),
                ColumnName = record["ColumnName"].ToString(),
                Name = record["PropertyName"].ToString(),
                Text = record["PropertyValue"].ToString()
            };
        }
    }
}
