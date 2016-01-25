using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerDocumenterUtility.Data.Mappers;
using Rhino.Mocks;
using System.Data;
using SqlServerDocumenterUtility.Models;

namespace SqlServerDocumenterUtility.Tests.Data.Mappers
{
    [TestClass]
    public class TableMapperTests
    {
        private ITableMapper _mapper;

        [TestInitialize]
        public void Init()
        {
            _mapper = new TableMapper();
        }

        [TestMethod]
        public void Map_Valid()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(1);
            mockReader.Stub(x => x["ObjectName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaId"]).Return(1);
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");

            var tableModel = _mapper.Map(mockReader);

            Assert.IsNotNull(tableModel);
            Assert.IsInstanceOfType(tableModel, typeof(TableModel));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Map_Invalid_ObjectId()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(DBNull.Value);
            mockReader.Stub(x => x["ObjectName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaId"]).Return(1);
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");

            var tableModel = _mapper.Map(mockReader);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Map_Invalid_SchemaId()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(1);
            mockReader.Stub(x => x["ObjectName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaId"]).Return(DBNull.Value);
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");

            var tableModel = _mapper.Map(mockReader);
        }

    }
}
