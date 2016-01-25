using System;

namespace SqlServerDocumenterUtility.Models
{
    public class DalResponseModel<T> : DalResponseModel
    {
        public T Result { get; set; }
    }

    public class DalResponseModel
    {
        public bool HasError { get; set; }
        public Exception Exception { get; set; }
    }
}
