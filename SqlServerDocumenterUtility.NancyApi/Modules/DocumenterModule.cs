
using Nancy;
using SqlServerDocumenterUtility.Data.Dals;
using SqlServerDocumenterUtility.Models;
using SqlServerDocumenterUtility.Models.Validation;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SqlServerDocumenterUtility.NancyApi.Modules
{
    /// <summary>
    /// Nancy Property Module defining API for interactions to get lookup data
    /// extended properties
    /// </summary>
    public class DocumenterModule : NancyModule
    {
        #region Injectibles

        private ITableDal _tableDal;
        internal ITableDal TableDal
        {
            private get { return _tableDal ?? (_tableDal = new TableDal()); }
            set { _tableDal = value; }
        }

        private IColumnDal _columnDal;
        internal IColumnDal ColumnDal
        {
            private get { return _columnDal ?? (_columnDal = new ColumnDal()); }
            set { _columnDal = value; }
        }

        #endregion

        public DocumenterModule() : base("/nancy")
        {
            Get["/"] = _ => "Hello World!";

            Get["/databases"] = _ => GetDatabases();
            Get["/tables"] = _ => GetTables();
            Get["/columns/{tableId}"] = parameters => GetColumns(parameters.tableId);
        }

        /// <summary>
        /// Method to get the database connections setup to be accessible using the nancy api. These are 
        /// evaluated for matches with the connections found using Web API. Where they match, the user
        /// is able to select to use the Nancy Api instead of Web API
        /// </summary>
        /// <returns></returns>
        private List<DatabaseModel> GetDatabases()
        {
            HttpRequires.IsTrue(ConfigurationManager.ConnectionStrings.Count > 0, "No connections");
            var connections = new List<DatabaseModel>();
            foreach (ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings)
            {
                connections.Add(new DatabaseModel
                {
                    Name = connection.Name,
                    ConnectionString = connection.ConnectionString
                });
            }
            return connections;
        }

        /// <summary>
        /// Method to get a collection of tables in the database selected by the user.
        /// </summary>
        /// <returns></returns>
        private List<TableModel> GetTables()
        {
            var connectionString = Request.Headers["connectionString"].FirstOrDefault();
            connectionString = connectionString.Replace("|DataDirectory|", "..\\..\\SqlServerDocumenterUtility\\App_Data");

            HttpRequires.IsNotNull(connectionString, "Invalid connection");

            var dalResp = TableDal.GetTables(connectionString);

            HttpAssert.Success(dalResp);
            HttpAssert.NotNull(dalResp, "Unable to find results for table");

            return dalResp.Result.ToList();
        }

        /// <summary>
        /// Method to get a collection of columns for the user's selected table.
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        private List<ColumnModel> GetColumns(long tableId)
        {
            var connectionString = Request.Headers["connectionString"].FirstOrDefault();

            HttpRequires.IsNotNull(connectionString, "Invalid Connection");
            HttpRequires.IsTrue(tableId > 0, "Invalid Table Id");

            var dalResp = ColumnDal.GetColumns(tableId, connectionString);

            HttpAssert.Success(dalResp);
            HttpAssert.NotNull(dalResp, "Unable to find column results for table");
            return dalResp.Result.ToList();
        }
    }
}