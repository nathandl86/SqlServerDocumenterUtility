using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlServerDocumenterUtility.Models
{
    public class DalHelperModel<T> : DalHelperModel
    {        
        public Func<IDataRecord, T> Mapper { get; set; }
        public Func<DataSet, IEnumerable<T>> DatasetMapper { get; set; }
    }

    public class DalHelperModel
    {
        public string ProcName { get; set; }
        public string CommandText { get; set; }
        public SqlConnection Connection { get; set; }
        public IList<SqlParameter> Parameters { get; set; }
    }
}
