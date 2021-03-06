﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.SessionState;
using Mvc = System.Web.Mvc;
using SqlServerDocumenterUtility.Data.Dals;
using SqlServerDocumenterUtility.Models;
using System.Linq;
using SqlServerDocumenterUtility.Models.Validation;
using Autofac.Integration.WebApi;

namespace SqlServerDocumenterUtility.Controllers.Api
{
    /// <summary>
    /// Api Controller for lookups of non-property data.
    /// </summary>
    [RoutePrefix("api/documenter")]
    [Mvc.SessionState(SessionStateBehavior.Disabled)]
    [AutofacControllerConfiguration]
    public class DocumenterController : ApiController
    {
        private readonly ITableDal _tableDal;
        private readonly IColumnDal _columnDal;
        
        public DocumenterController(ITableDal tableDal, IColumnDal columnDal)
        {
            _tableDal = tableDal;
            _columnDal = columnDal;
        }


        /// <summary>
        /// Api Method to retrieve a collection of the databases defined in the web.config
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("databases")]
        public IHttpActionResult GetDatabases()
        {
            try
            {
                var connections = new List<DatabaseModel>();

                HttpRequires.IsTrue(ConfigurationManager.ConnectionStrings.Count > 0, "No connections");
                
                foreach(ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings)
                {
                    if (connection.Name.Contains("LocalSqlServer"))
                    {
                        continue;
                    }

                    //Since the nancy api is also using the same data base connections, "|DataDirectory|" cannot be used, because within 
                    // the nancy api project, it has a different context. Therefore, we replace it with the server path to the web app's 
                    // App_Data folder containing the database mdf's included. This only applies to database connection strings that
                    // attach to a file system path
                    var connectionString = connection.ConnectionString;
                    if (connectionString.Contains("|DataDirectory|"))
                    {
                        connectionString = connectionString.Replace("|DataDirectory|", System.Web.HttpContext.Current.Server.MapPath("\\Databases"));
                    }

                    connections.Add(new DatabaseModel {
                        Name = connection.Name,
                        ConnectionString = connectionString });
                }
                
                return Ok(connections);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Api Method to get a collection of the tables within the user's selected database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("tables")]
        public IHttpActionResult GetTables()
        {
            try
            {
                var connectionString = Request.Headers.GetValues("connectionString").FirstOrDefault();

                HttpRequires.IsNotNull(connectionString, "Invalid connection");

                var response = _tableDal.GetTables(connectionString);

                HttpAssert.Success(response);
                HttpAssert.NotNull(response, "Unable to find results for table");                
                return Ok(response.Result);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Api Method to get a collection of the columns for the user's selected table.
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("columns/{tableId}")]
        public IHttpActionResult GetColumns(long tableId)
        {
            try
            {
                var connectionString = Request.Headers.GetValues("connectionString").FirstOrDefault();

                HttpRequires.IsNotNull(connectionString, "Invalid Connection");
                HttpRequires.IsTrue(tableId > 0, "Invalid Table Id");

                var response = _columnDal.GetColumns(tableId, connectionString);

                HttpAssert.Success(response);
                HttpAssert.NotNull(response, "Unable to find column results for table");               
                return Ok(response.Result);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}

