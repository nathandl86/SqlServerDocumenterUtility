using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SqlServerDocumenterUtility.Data;
using SqlServerDocumenterUtility.Data.Dals;
using SqlServerDocumenterUtility.Data.Mappers;
using SqlServerDocumenterUtility.Models;
using System.Collections.Generic;
using System.Configuration;

namespace SqlServerDocumenterUtility.Tests.Data
{
    [TestClass]
    public class ColumnDalTests
    {
        private string _connectionString;

        [TestInitialize]
        public void Init()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
        }

        [TestMethod]
        public void GetColumns()
        {
            //Arrange
            var mockHelper = MockRepository.GenerateMock<IDalHelper>();
            mockHelper.Stub(x => x.RetrieveList(
                Arg<DalHelperModel<ColumnModel>>.Is.Anything
            )).Return(new List<ColumnModel>
            {
                new ColumnModel()
            });
            var mapperStub = MockRepository.GenerateStub<IColumnMapper>();
            var columnDal = new ColumnDal(mapperStub, mockHelper);

            //Act 
            var dalResponse = columnDal.GetColumns(0, _connectionString);

            //Assert
            Assert.IsFalse(dalResponse.HasError);
            Assert.IsNotNull(dalResponse.Result);
        }
    }
}
