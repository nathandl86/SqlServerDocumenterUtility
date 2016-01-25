
using SqlServerDocumenterUtility.Models.Exceptions;

namespace SqlServerDocumenterUtility.Models.Validation
{
    /// <summary>
    /// Class containing helper methods for validating data layer responses
    /// </summary>
    public static class HttpAssert
    {
        /// <summary>
        /// Method to ensure that the response model returned by the data layer
        /// is free of errors.
        /// </summary>
        /// <param name="model"></param>
        public static void Success(DalResponseModel model)
        {
            if (model.HasError)
            {
                throw model.Exception;
            }
        }

        /// <summary>
        /// Method to ensure that the result in the data layer response model is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        public static void NotNull<T>(DalResponseModel<T> model, string msg) where T : class
        {
            if (model.Result == null)
            {
                throw new NotFoundException(msg);
            }
        }
    }
}