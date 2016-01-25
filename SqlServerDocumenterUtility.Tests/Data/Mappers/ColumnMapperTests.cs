using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SqlServerDocumenterUtility.Data.Mappers;
using SqlServerDocumenterUtility.Models;
using System;
using System.Data;

namespace SqlServerDocumenterUtility.Tests.Data.Mappers
{
    [TestClass]
    public class ColumnMapperTests
    {
        private IColumnMapper _mapper;

        [TestInitialize]
        public void Init()
        {
            _mapper = new ColumnMapper();
        }


        [TestMethod]
        public void Map_Valid()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(1);
            mockReader.Stub(x => x["TableName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaId"]).Return(1);
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");
            mockReader.Stub(x => x["ColumnId"]).Return(1);
            mockReader.Stub(x => x["ColumnName"]).Return("Fubar");

            var columnModel = _mapper.Map(mockReader);

            Assert.IsNotNull(columnModel);
            Assert.IsInstanceOfType(columnModel, typeof(ColumnModel));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Map_Invalid_ObjectId()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(DBNull.Value);
            mockReader.Stub(x => x["TableName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaId"]).Return(1);
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");
            mockReader.Stub(x => x["ColumnId"]).Return(1);
            mockReader.Stub(x => x["ColumnName"]).Return("Fubar");

            var columnModel = _mapper.Map(mockReader);

            Assert.IsNotNull(columnModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Map_Invalid_ColumnId()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(1);
            mockReader.Stub(x => x["TableName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaId"]).Return(1);
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");
            mockReader.Stub(x => x["ColumnId"]).Return(DBNull.Value);
            mockReader.Stub(x => x["ColumnName"]).Return("Fubar");

            var columnModel = _mapper.Map(mockReader);

            Assert.IsNotNull(columnModel);
        }
    }
}
