using SqlServerDocumenterUtility.Data.Mappers;
using SqlServerDocumenterUtility.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SqlServerDocumenterUtility.Data.Dals
{
    /// <summary>
    /// Dal Class for interacting with tables
    /// </summary>
    public class TableDal : ITableDal
    {
        private readonly ITableMapper _tableMapper;
        private readonly IDalHelper _dalHelper;

        #region Sql command templates

        /// <summary>
        /// Sql command to get the table in the database
        /// </summary>
        private const string SQL_TEMPLATE_GET_TABLES = @"
    SELECT
        o.[object_id] AS ObjectId
        , o.name AS ObjectName
        , t.[schema_id] AS SchemaId
        , s.name AS SchemaName       
        , t.create_date AS CreatedDate
        , t.modify_date AS LastModifiedDate
    FROM 
        sys.objects o
        INNER JOIN sys.Tables t ON t.[object_id] = o.[object_id]
        INNER JOIN sys.schemas s ON s.[schema_id] = o.[schema_id]
    ORDER BY 
        s.name,
        o.name;";

        #endregion

        public TableDal(ITableMapper tableMapper, IDalHelper dalHelper)
        {
            _tableMapper = tableMapper;
            _dalHelper = dalHelper;
        }

        /// <summary>
        /// Method to get a collection of the tables available to add extended properties
        /// </summary>
        /// <param name="connectionString">Connection String used to return tables</param>
        /// <returns></returns>
        public DalResponseModel<IList<TableModel>> GetTables(string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var result = _dalHelper.RetrieveList<TableModel>(new DalHelperModel<TableModel>
                    {
                        CommandText = SQL_TEMPLATE_GET_TABLES,
                        Connection = conn,
                        Mapper = _tableMapper.Map
                    });

                    return new DalResponseModel<IList<TableModel>>
                    {
                        HasError = false,
                        Result = result
                    };
                }
            }
            catch(Exception ex)
            {
                return new DalResponseModel<IList<TableModel>>
                {
                    Exception = ex,
                    HasError = true
                };
            }
        }
    }
}
