using SqlServerDocumenterUtility.Models;
using System.Data;

namespace SqlServerDocumenterUtility.Data.Mappers
{
    public interface IExtendedPropertyModelMapper
    {
        ExtendedPropertyModel Map(IDataRecord record);
    }
}
