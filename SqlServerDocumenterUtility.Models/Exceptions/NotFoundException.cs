using System;

namespace SqlServerDocumenterUtility.Models.Exceptions
{
    /// <summary>
    /// Wrapper exception to allow for not found exceptions to 
    /// be thrown, so that 404's can be returned from the api.
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException(string msg) : base(msg) { }
    }
}