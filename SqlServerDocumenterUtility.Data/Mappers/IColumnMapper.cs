using SqlServerDocumenterUtility.Models;
using System.Data;

namespace SqlServerDocumenterUtility.Data.Mappers
{
    public interface IColumnMapper
    {
        ColumnModel Map(IDataRecord record);
    }
}
