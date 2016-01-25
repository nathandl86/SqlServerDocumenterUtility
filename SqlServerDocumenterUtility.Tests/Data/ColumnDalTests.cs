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
    public class ColumnDalTests
    {
        private ColumnDal _columnDal;
        private string _connectionString;

        [TestInitialize]
        public void Init()
        {
            _columnDal = new ColumnDal();
            _connectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
        }

        [TestMethod]
        public void GetColumns()
        {
            var mockHelper = MockRepository.GenerateMock<IDalHelper>();
            mockHelper.Stub(x => x.RetrieveList(
                Arg<DalHelperModel<ColumnModel>>.Is.Anything
            )).Return(new List<ColumnModel>
            {
                new ColumnModel()
            });

            _columnDal.DalHelper = mockHelper;

            var dalResponse = _columnDal.GetColumns(0, _connectionString);

            Assert.IsFalse(dalResponse.HasError);
            Assert.IsNotNull(dalResponse.Result);
        }
    }
}
