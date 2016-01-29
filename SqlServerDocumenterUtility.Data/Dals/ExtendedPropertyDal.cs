using SqlServerDocumenterUtility.Data.Mappers;
using SqlServerDocumenterUtility.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlServerDocumenterUtility.Data.Dals
{
    /// <summary>
    /// Dal Class for interacting with extended properties for database tables and their columns
    /// </summary>
    public class ExtendedPropertyDal : IExtendedPropertyDal
    {
        private readonly IExtendedPropertyModelMapper _modelMapper;
        private readonly IDalHelper _dalHelper;
        
        #region Sql command templates

        /// <summary>
        /// Sql Command template for adding exteneded property
        /// </summary>
        private const string SQL_TEMPLATE_ADD = @"
    DECLARE @command VARCHAR(2000);
    
    SET @pPropertyName = REPLACE(REPLACE(@pPropertyName, '''', ''''''), ']', ']]');
    SET @pPropertyValue = REPLACE(REPLACE(@pPropertyValue, '''', ''''''), ']', ']]');
    SET @pSchema = REPLACE(REPLACE(@pSchema, '''', ''''''), ']', ']]');
    SET @pTableName = REPLACE(REPLACE(@pTableName, '''', ''''''), ']', ']]');
    SET @pColumnName = REPLACE(REPLACE(@pColumnName, '''', ''''''), ']', ']]');

    SET @command = 'sys.sp_addextendedproperty @name=N''' + @pPropertyName +
        ''', @value=N''' + @pPropertyValue + ''', @level0type=N''SCHEMA'', @level0name=' + @pSchema +
        CASE WHEN @pTableName IS NULL THEN '' ELSE ', @level1type=N''TABLE'', @level1name=' + @pTableName END + 
        CASE WHEN @pColumnName IS NULL THEN '' ELSE ', @level2type=N''COLUMN'', @level2name=' + @pColumnName END;

    EXEC (@command);";

        /// <summary>
        /// Sql command template for removing extended properties
        /// 
        /// 0 = Property Name
        /// 1 = Object (Table) Name
        /// 2 = Column Name
        /// </summary>
        private const string SQL_TEMPLATE_REMOVE = @"
    DECLARE @command VARCHAR(2000);
    
    SET @pPropertyName = REPLACE(REPLACE(@pPropertyName, '''', ''''''), ']', ']]');
    SET @pObjectName = REPLACE(REPLACE(@pObjectName, '''', ''''''), ']', ']]');
    SET @pColumnName = REPLACE(REPLACE(@pColumnName, '''', ''''''), ']', ']]');

    SET @command = 'sys.sp_dropextendedproperty @name=N''' + @pPropertyName + 
        ''', @level0type=N''SCHEMA'', @level0name=dbo' +
        CASE WHEN @pObjectName IS NULL THEN '' ELSE ', @level1type=N''TABLE'', @level1name=' + @pObjectName END +
        CASE WHEN @pColumnName IS NULL THEN '' ELSE ', @level2type=N''COLUMN'', @level2name=' + @pColumnName END;
        
    --PRINT @command;

    EXEC (@command);";

        /// <summary>
        /// Sql command template to get the extended properties by object_id
        /// It unions the column properties after getting just the table properties. 
        /// This was done so that there is only a single database interaction to 
        /// get properties for table.
        /// </summary>
        private const string SQL_TEMPLATE_RETRIEVE_BY_OBJECT_ID = @"
    SELECT 
        o.object_id AS ObjectId
        , s.name AS SchemaName
        , o.name AS ObjectName
        , 0 AS ColumnId
        , '' AS ColumnName
        , ep.name AS PropertyName
        , ep.value AS PropertyValue
    FROM 
        sys.objects o
        INNER JOIN sys.extended_properties ep On ep.major_id = o.[object_id]   
        INNER JOIN sys.schemas s ON s.[schema_id] = o.[schema_id]     
    WHERE
        o.[object_id] = @pObjectId
        AND ep.class IN (1, 2, 3)
        AND ep.minor_id = 0
        
    UNION ALL

    SELECT 
        o.[object_id] AS ObjectId   
        , s.name AS SchemaName             
        , o.name AS ObjectName
        , c.column_id AS ColumnId
        , c.name AS ColumnName
        , ep.name AS PropertyName
        , ep.value AS PropertyValue      
    FROM
        sys.objects o 
        INNER JOIN sys.columns c ON c.[object_id] = o.[object_id]
        INNER JOIN sys.schemas s ON s.[schema_id] = o.[schema_id]  
        INNER JOIN sys.extended_properties ep ON ep.major_id = o.[object_id]
                                             AND ep.minor_id = c.column_id
    WHERE
        o.[object_id] = @pObjectId
        AND ep.class IN (1,2,3);";

        #endregion

        public ExtendedPropertyDal(IExtendedPropertyModelMapper propertyModelMapper, IDalHelper dalHelper)
        {
            _modelMapper = propertyModelMapper;
            _dalHelper = dalHelper;
        }

        /// <summary>
        /// Method to add an extended property to a sql server database object
        /// </summary>
        /// <param name="model">Model containing the data needed to create the property</param>
        /// <param name="connectionString">Connection string for connecting to the desired database</param>
        /// <returns></returns>
        public DalResponseModel<bool> AddProperty(ExtendedPropertyModel model, string connectionString)
        {

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@pPropertyName", model.Name),
                        new SqlParameter("@pPropertyValue", model.Text),
                        new SqlParameter("@pSchema", model.SchemaName),
                        new SqlParameter("@pTableName", model.TableName),
                        BuildColumnParam(model.ColumnName)
                    };
                    
                    var result = _dalHelper.Insert(new DalHelperModel
                    {
                        CommandText = SQL_TEMPLATE_ADD,
                        Connection = conn,
                        Parameters = parameters
                    });

                    return new DalResponseModel<bool>
                    {
                        HasError = false,
                        Result = result > 0
                    };
                }
            }
            catch (Exception ex)
            {
                return new DalResponseModel<bool>
                {
                    Exception = ex,
                    HasError = true,
                    Result = default(bool)
                };
            }
        }

        /// <summary>
        /// Method to delete an extended property from a sql server database object
        /// </summary>
        /// <param name="model">Model containing the data needed for the delete</param>
        /// <param name="connectionString">Connection string for connecting to the desired database</param>
        /// <returns></returns>
        public DalResponseModel DeleteProperty(ExtendedPropertyModel model, string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@pPropertyName", model.Name),
                        new SqlParameter("@pObjectName", model.TableName),
                        BuildColumnParam(model.ColumnName)
                    }; 

                    _dalHelper.Delete(new DalHelperModel
                    {
                        CommandText = SQL_TEMPLATE_REMOVE,
                        Connection = conn,
                        Parameters = parameters
                    });

                    return new DalResponseModel { HasError = false };
                }
            }
            catch (Exception ex)
            {
                return new DalResponseModel
                {
                    Exception = ex,
                    HasError = true
                };
            }
        }

        /// <summary>
        /// Method for retrieving the extended properties associated with a database object
        /// </summary>
        /// <param name="objectId">Object id to lookup the extended properties for</param>
        /// <param name="connectionString">Connection String for connecting to the desired database</param>
        /// <returns></returns>
        public DalResponseModel<IList<ExtendedPropertyModel>> RetrieveByTableId(long objectId, string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var result = _dalHelper.RetrieveList(new DalHelperModel<ExtendedPropertyModel>
                    {
                        CommandText = SQL_TEMPLATE_RETRIEVE_BY_OBJECT_ID,
                        Connection = conn,
                        Parameters = new List<SqlParameter> { new SqlParameter("@pObjectId", objectId.ToString())},
                        Mapper = _modelMapper.Map
                    });

                    return new DalResponseModel<IList<ExtendedPropertyModel>>
                    {
                        HasError = false,
                        Result = result
                    };
                }
            }
            catch(Exception ex)
            {
                return new DalResponseModel<IList<ExtendedPropertyModel>>
                {
                    Exception = ex,
                    HasError = true,
                    Result = null
                };
            }
        }

        /// <summary>
        /// Helper to build the @pColumnName parameter that must be present in the 
        /// SqlCommand's parameter set but can be null
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private SqlParameter BuildColumnParam(string val)
        {
            var columnParam = new SqlParameter("@pColumnName", SqlDbType.VarChar);
            if (!String.IsNullOrWhiteSpace(val))
            {
                columnParam.Value = val;
            }
            else
            {
                columnParam.Value = DBNull.Value;
            }
            return columnParam;
        }
    }
}
