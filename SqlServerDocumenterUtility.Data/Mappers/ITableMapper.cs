using System.Data;
using SqlServerDocumenterUtility.Models;

namespace SqlServerDocumenterUtility.Data.Mappers
{
    public interface ITableMapper
    {
        TableModel Map(IDataRecord record);
    }
}