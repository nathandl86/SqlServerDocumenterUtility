using System.Collections.Generic;
using SqlServerDocumenterUtility.Models;

namespace SqlServerDocumenterUtility.Data.Dals
{
    public interface ITableDal
    {
        DalResponseModel<IList<TableModel>> GetTables(string connectionString);
    }
}