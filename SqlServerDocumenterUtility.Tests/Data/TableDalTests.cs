using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SqlServerDocumenterUtility.Data;
using SqlServerDocumenterUtility.Data.Dals;
using SqlServerDocumenterUtility.Models;
using System.Collections.Generic;
using System.Configuration;

namespace SqlServerDocumenterUtility.Tests.Data
{
    [TestClass]
    public class TableDalTests
    {
        private TableDal _tableDal;
        private string _connectionString;

        [TestInitialize]
        public void Init()
        {
            _tableDal = new TableDal();
            _connectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
        }

        [TestMethod]
        public void GetTables()
        {
            var mockHelper = MockRepository.GenerateMock<IDalHelper>();
            mockHelper.Stub(x => x.RetrieveList(
                Arg<DalHelperModel<TableModel>>.Is.Anything
            )).Return(new List<TableModel>
            {
                new TableModel()
            });

            _tableDal.DalHelper = mockHelper;

            var dalResponse = _tableDal.GetTables(_connectionString);

            Assert.IsFalse(dalResponse.HasError);
            Assert.IsNotNull(dalResponse.Result);
        }
    }
}
