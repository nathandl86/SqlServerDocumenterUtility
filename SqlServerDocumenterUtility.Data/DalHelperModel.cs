using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlServerDocumenterUtility.Data
{
    public class HelperDto<T>
    {
        public string ProcName { get; set; }
        public SqlConnection Connection { get; set; }
        public IList<SqlParameter> Parameters { get; set; }
        public Func<IDataRecord, T> Mapper { get; set; }
        public Func<DataSet, IEnumerable<T>> DatasetMapper { get; set; }
    }
}
