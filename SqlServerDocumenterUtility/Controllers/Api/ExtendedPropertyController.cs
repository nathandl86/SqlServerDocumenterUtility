
using SqlServerDocumenterUtility.Data.Dals;
using SqlServerDocumenterUtility.Models;
using SqlServerDocumenterUtility.Models.Exceptions;
using SqlServerDocumenterUtility.Models.Validation;
using System;
using System.Linq;
using System.Transactions;
using System.Web.Http;
using System.Web.SessionState;
using Mvc = System.Web.Mvc;

namespace SqlServerDocumenterUtility.Controllers.Api
{
    /// <summary>
    /// Api Controller for interactions against sql server table extended properties
    /// </summary>
    [RoutePrefix("api/property")]
    [Mvc.SessionState(SessionStateBehavior.Disabled)]
    public class ExtendedPropertyController : ApiController
    {
        #region Injectibles

        private IExtendedPropertyDal _propertyDal;
        internal IExtendedPropertyDal PropertyDal
        {
            get { return _propertyDal ?? (_propertyDal = new ExtendedPropertyDal()); }
            set { _propertyDal = value; }
        }

        #endregion

        /// <summary>
        /// Api Method for removing an extended property
        /// </summary>
        /// <param name="propertyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public IHttpActionResult DeleteProperty([FromBody] ExtendedPropertyModel propertyModel)
        {
            try
            {
                var connectionString = Request.Headers.GetValues("connectionString").FirstOrDefault();

                HttpRequires.IsNotNull(connectionString, "Ivnalid Connection");
                HttpRequires.IsNotNull(propertyModel, "Invalid Properties");
                ValidatePropertyModel(propertyModel);

                var response = PropertyDal.DeleteProperty(propertyModel, connectionString);

                HttpAssert.Success(response);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Api Method for adding a new extended property
        /// </summary>
        /// <param name="propertyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddProperty([FromBody] ExtendedPropertyModel propertyModel)
        {
            try
            {
                var connectionString = Request.Headers.GetValues("connectionString").FirstOrDefault();

                HttpRequires.IsNotNull(connectionString, "Invalid Connection");
                HttpRequires.IsNotNull(propertyModel, "Invalid Properties");
                ValidatePropertyModel(propertyModel);

                var response = PropertyDal.AddProperty(propertyModel, connectionString);

                HttpAssert.Success(response);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Api Method for updating an extended property
        /// </summary>
        /// <param name="propertyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateProperty([FromBody] ExtendedPropertyModel propertyModel)
        {
            try
            {
                var connectionString = Request.Headers.GetValues("connectionString").FirstOrDefault();

                HttpRequires.IsNotNull(connectionString, "Invalid Connection");
                HttpRequires.IsNotNull(propertyModel, "Invalid Properties");
                ValidatePropertyModel(propertyModel);

                DalResponseModel deletionResponse, addResponse;

                //Using the sql server system procedures I had (for add and delete). Instead of an in
                // place update, the process is to first delete the existing property, and then add
                // the new values. To avoid a situation where the delete succeeds but the add fails, 
                // we use a transaction scope.
                using (var scope = new TransactionScope())
                {
                    deletionResponse = PropertyDal.DeleteProperty(propertyModel, connectionString);
                    addResponse = PropertyDal.AddProperty(propertyModel, connectionString);
                    HttpAssert.Success(deletionResponse);
                    HttpAssert.Success(addResponse);
                    scope.Complete();
                }
                
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Api Method to retrieve the extended properties for a table (includes column properties)
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/{tableId}")]
        public IHttpActionResult GetTableProperties(long tableId)
        {
            try
            {
                var connectionString = Request.Headers.GetValues("connectionString").FirstOrDefault();

                HttpRequires.IsNotNull(connectionString, "Ivnalid Connection");
                HttpRequires.IsTrue(tableId >= 0, "Invalid Table Id");

                var response = PropertyDal.RetrieveByTableId(tableId, connectionString);

                HttpAssert.Success(response);
                HttpAssert.NotNull(response, "Unable to find property results for table");
                return Ok(response.Result);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        /// <summary>
        /// Helper method to validate the property model before persistance.
        /// </summary>
        /// <param name="model"></param>
        private void ValidatePropertyModel(ExtendedPropertyModel model)
        {
            if (String.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentException("Extended Propery Model requires a Name");
            else if (String.IsNullOrWhiteSpace(model.TableName))
            {
                throw new ArgumentException("Extended Property Model requires either a table name be specified");
            }
        }
    }
}
