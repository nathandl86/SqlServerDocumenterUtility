using System.Collections.Generic;
using SqlServerDocumenterUtility.Models;

namespace SqlServerDocumenterUtility.Data.Dals
{
    public interface IExtendedPropertyDal
    {
        DalResponseModel<bool> AddProperty(ExtendedPropertyModel model, string connectionString);
        DalResponseModel DeleteProperty(ExtendedPropertyModel model, string connectionString);
        DalResponseModel<IList<ExtendedPropertyModel>> RetrieveByTableId(long objectId, string connectionString);
    }
}