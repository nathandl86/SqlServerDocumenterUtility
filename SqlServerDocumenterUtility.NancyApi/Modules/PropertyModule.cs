using Nancy;
using Nancy.ModelBinding;
using SqlServerDocumenterUtility.Data.Dals;
using SqlServerDocumenterUtility.Models;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using SqlServerDocumenterUtility.Models.Validation;

namespace SqlServerDocumenterUtility.NancyApi.Modules
{
    /// <summary>
    /// Nancy Property Module defining API for interactions with the sql server
    /// extended properties
    /// </summary>
    public class PropertyModule : NancyModule
    {
        #region Injectibles

        private IExtendedPropertyDal _propertyDal;
        internal IExtendedPropertyDal PropertyDal
        {
            get { return _propertyDal ?? (_propertyDal = new ExtendedPropertyDal()); }
            set { _propertyDal = value; }
        }

        #endregion


        public PropertyModule() : base("nancy/property")
        {
            Get["/get/{tableId}"] = parameters => GetProperties(parameters.tableId);
            Post["/"] = _ => HttpStatusCode.MethodNotAllowed;

            Post["/delete"] = _ =>
            {
                var property = this.Bind<ExtendedPropertyModel>();
                Remove(property);
                return HttpStatusCode.OK;
            };

            Post["/add"] = _ =>
            {
                var property = this.Bind<ExtendedPropertyModel>();
                Add(property);
                return HttpStatusCode.OK;
            };

            Post["/update"] = _ =>
            {
                var property = this.Bind<ExtendedPropertyModel>();
                Update(property);
                return HttpStatusCode.OK;
            };
        }

        /// <summary>
        /// Method to get the properties for a table (includes the properties for the 
        /// table's columns)
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        private List<ExtendedPropertyModel> GetProperties(long tableId)
        {
            var connectionString = Request.Headers["connectionString"].FirstOrDefault();

            HttpRequires.IsNotNull(connectionString, "Ivnalid Connection");
            HttpRequires.IsTrue(tableId >= 0, "Invalid Table Id");

            var dalResp = PropertyDal.RetrieveByTableId(tableId, connectionString);

            HttpAssert.Success(dalResp);
            HttpAssert.NotNull(dalResp, "Unable to find property results for table");

            return dalResp.Result.ToList();
        }

        /// <summary>
        /// Method to remove an extended property from a table or column.
        /// </summary>
        /// <param name="model"></param>
        private void Remove(ExtendedPropertyModel model)
        {
            var connectionString = Request.Headers["connectionString"].FirstOrDefault();
            HttpRequires.IsNotNull(connectionString, "Ivnalid Connection");
            HttpRequires.IsNotNull(model, "Invalid Properties");

            var dalResponse = PropertyDal.DeleteProperty(model, connectionString);
            HttpAssert.Success(dalResponse);
        }

        /// <summary>
        /// Method to add an extended property to a table or table column
        /// </summary>
        /// <param name="model"></param>
        private void Add(ExtendedPropertyModel model)
        {
            var connectionString = Request.Headers["connectionString"].FirstOrDefault();
            HttpRequires.IsNotNull(connectionString, "Invalid Connection");
            HttpRequires.IsNotNull(model, "Invalid Properties");

            var response = PropertyDal.AddProperty(model, connectionString);

            HttpAssert.Success(response);
        }

        /// <summary>
        /// Method to update an extended property of a table or column
        /// </summary>
        /// <param name="model"></param>
        private void Update(ExtendedPropertyModel model)
        {
            var connectionString = Request.Headers["connectionString"].FirstOrDefault();
            HttpRequires.IsNotNull(connectionString, "Invalid Connection");
            HttpRequires.IsNotNull(model, "Invalid Properties");

            //Using the sql server system procedures I had (for add and delete). Instead of an in
            // place update, the process is to first delete the existing property, and then add
            // the new values. To avoid a situation where the delete succeeds but the add fails, 
            // we use a transaction scope.
            using (var scope = new TransactionScope())
            {
                var deleteResp = PropertyDal.DeleteProperty(model, connectionString);
                var addResp = PropertyDal.AddProperty(model, connectionString);
                HttpAssert.Success(deleteResp);
                HttpAssert.Success(addResp);
                scope.Complete();
            }
        }
    }
}