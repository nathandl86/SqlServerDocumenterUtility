using System;
namespace SqlServerDocumenterUtility.Exceptions
{
    /// <summary>
    /// Wrapper exception to allow for not found exceptions to 
    /// be thrown, so that 404's can be returned from the api.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string msg): base(msg) { }
    }
}