using System;

namespace SqlServerDocumenterUtility.Models.Validation
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
            if ((typeof(T) == typeof(string) && (val == null || String.IsNullOrWhiteSpace(val.ToString())))
                || val == null)
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
}