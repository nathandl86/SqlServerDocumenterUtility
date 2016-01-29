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
    public class TableDalTests
    {
        private string _connectionString;

        [TestInitialize]
        public void Init()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
        }

        [TestMethod]
        public void GetTables()
        {
            //Arrange
            var mockHelper = MockRepository.GenerateMock<IDalHelper>();
            mockHelper.Stub(x => x.RetrieveList(
                Arg<DalHelperModel<TableModel>>.Is.Anything
            )).Return(new List<TableModel>
            {
                new TableModel()
            });
            var mapperStub = MockRepository.GenerateStub<ITableMapper>();
            var tableDal = new TableDal(mapperStub, mockHelper);

            //Act
            var dalResponse = tableDal.GetTables(_connectionString);

            //Assert
            Assert.IsFalse(dalResponse.HasError);
            Assert.IsNotNull(dalResponse.Result);
        }
    }
}
