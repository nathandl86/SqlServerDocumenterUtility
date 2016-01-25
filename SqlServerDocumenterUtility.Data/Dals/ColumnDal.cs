using SqlServerDocumenterUtility.Data.Mappers;
using SqlServerDocumenterUtility.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ColumnMapperClass = SqlServerDocumenterUtility.Data.Mappers.ColumnMapper;

namespace SqlServerDocumenterUtility.Data.Dals
{
    /// <summary>
    /// Dal Class for interacting with table columns
    /// </summary>
    public class ColumnDal : IColumnDal
    {
        #region Injectibles

        private IColumnMapper _columnMapper;
        internal IColumnMapper ColumnMapper
        {
            private get { return _columnMapper ?? (_columnMapper = new ColumnMapperClass()); }
            set { _columnMapper = value; }
        }

        #endregion

        #region Sql command templates

        /// <summary>
        /// SQL command template to get columns for a database table
        /// 
        /// 0 = Object_Id
        /// </summary>
        private const string SQL_TEMPLATE_GET_COLUMNS_BY_TABLE = @"
    SELECT 
        o.[object_id] AS ObjectId
        , s.[schema_id] AS SchemaId
        , s.name AS SchemaName
        , t.name AS TableName
        , c.column_id AS ColumnId
        , c.name AS ColumnName 
    FROM 
        sys.objects o
        INNER JOIN sys.tables t ON t.[object_id] = o.[object_id]
        INNER JOIN sys.schemas s ON s.[schema_id] = t.[schema_id]
        INNER JOIN sys.columns c ON c.[object_id] = t.[object_id]
    WHERE 
        o.[object_id] = @pObjectId;";

        #endregion

        /// <summary>
        /// Method to get the columns for an object (table)
        /// </summary>
        /// <param name="objectId">The internal id for the table in Sql Server</param>
        /// <param name="connectionString">connection string for the database</param>
        /// <returns></returns>
        public DalResponseModel<IList<ColumnModel>> GetColumns(long objectId, string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var result = DalHelper.RetrieveList<ColumnModel>(new DalHelperModel<ColumnModel>
                    {
                        CommandText = SQL_TEMPLATE_GET_COLUMNS_BY_TABLE,
                        Connection = conn,
                        Parameters = new List<SqlParameter> { new SqlParameter("@pObjectId", objectId) },
                        Mapper = ColumnMapper.Map
                    });

                    return new DalResponseModel<IList<ColumnModel>>
                    {
                        HasError = false,
                        Result = result
                    };
                }
            }
            catch(Exception ex)
            {
                return new DalResponseModel<IList<ColumnModel>>
                {
                    Exception = ex,
                    HasError = true,
                    Result = null
                };
            }
        }
    }
}
