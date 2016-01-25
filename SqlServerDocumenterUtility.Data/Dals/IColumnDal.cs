using SqlServerDocumenterUtility.Models;
using System.Collections.Generic;

namespace SqlServerDocumenterUtility.Data.Dals
{
    public interface IColumnDal
    {
        DalResponseModel<IList<ColumnModel>> GetColumns(long objectId, string connectionString);
    }
}
