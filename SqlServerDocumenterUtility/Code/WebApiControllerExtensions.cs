using SqlServerDocumenterUtility.Exceptions;
using SqlServerDocumenterUtility.Models;
using System;

namespace SqlServerDocumenterUtility.Code
{
    /// <summary>
    /// Class containing helper methods to validate request values before
    /// attempting to interact with the data layer.
    /// </summary>
    public static class HttpRequires
    {
        /// <summary>
        /// Evaluates the value passed in for if it is null or not. For types of
        /// "string", it is evaluated as null.
        /// </summary>
        /// <typeparam name="T">Tyep for evaluating for null</typeparam>
        /// <param name="val"></param>
        /// <param name="msg">Message to be thrown in the ArgumentException on failure</param>
        public static void IsNotNull<T>(T val, string msg)
        {
            if ((typeof(T) == typeof(string) && String.IsNullOrWhiteSpace(val.ToString()))
                && val == null)
            {
                throw new ArgumentException(msg);
            }
        }

        /// <summary>
        /// Evaluates if the value passed in is null
        /// </summary>
        /// <param name="eval"></param>
        /// <param name="msg">Message to be thrown in the ArgumentException on failure</param>
        public static void IsTrue(bool eval, string msg)
        {
            if (!eval)
            {
                throw new ArgumentException(msg);
            }
        }
    }

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
            if(model.Result == null)
            {
                throw new NotFoundException(msg);
            }
        }
    }
}