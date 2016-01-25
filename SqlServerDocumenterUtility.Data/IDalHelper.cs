using System.Collections.Generic;
using SqlServerDocumenterUtility.Models;

namespace SqlServerDocumenterUtility.Data
{
    public interface IDalHelper
    {
        void Delete(DalHelperModel dto);
        int? Insert(DalHelperModel dto);
        IList<T> RetrieveList<T>(DalHelperModel<T> dto);
        bool Update(DalHelperModel dto);
    }
}