using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerDocumenterUtility.Data.Mappers;
using Rhino.Mocks;
using SqlServerDocumenterUtility.Models;
using System.Data;

namespace SqlServerDocumenterUtility.Tests.Data.Mappers
{
    [TestClass]
    public class ExtendedPropertyMapperTests
    {
        IExtendedPropertyModelMapper _mapper;

        [TestInitialize]
        public void Init()
        {
            _mapper = new ExtendedPropertyModelMapper();
        }

        [TestMethod]
        public void Map_Valid()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(1);
            mockReader.Stub(x => x["ObjectName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");
            mockReader.Stub(x => x["ColumnId"]).Return(1);
            mockReader.Stub(x => x["ColumnName"]).Return("Fubar");
            mockReader.Stub(x => x["PropertyName"]).Return("Foo");
            mockReader.Stub(x => x["PropertyValue"]).Return("Foo");

            var propertyModel = _mapper.Map(mockReader);

            Assert.IsNotNull(propertyModel);
            Assert.IsInstanceOfType(propertyModel, typeof(ExtendedPropertyModel));
        }

        [TestMethod]
        public void Map_InValid_ObjectId()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(DBNull.Value);
            mockReader.Stub(x => x["ObjectName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");
            mockReader.Stub(x => x["ColumnId"]).Return(1);
            mockReader.Stub(x => x["ColumnName"]).Return("Fubar");
            mockReader.Stub(x => x["PropertyName"]).Return("Foo");
            mockReader.Stub(x => x["PropertyValue"]).Return("Foo");

            var propertyModel = _mapper.Map(mockReader);

            Assert.IsNotNull(propertyModel);
            Assert.IsNull(propertyModel.TableId);
        }

        [TestMethod]
        public void Map_InValid_ColumnId()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(1);
            mockReader.Stub(x => x["ObjectName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");
            mockReader.Stub(x => x["ColumnId"]).Return(DBNull.Value);
            mockReader.Stub(x => x["ColumnName"]).Return("Fubar");
            mockReader.Stub(x => x["PropertyName"]).Return("Foo");
            mockReader.Stub(x => x["PropertyValue"]).Return("Foo");

            var propertyModel = _mapper.Map(mockReader);

            Assert.IsNotNull(propertyModel);
            Assert.IsNull(propertyModel.ColumnId);
        }

        [TestMethod]
        public void Map_InValid_PropertyName()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(1);
            mockReader.Stub(x => x["ObjectName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");
            mockReader.Stub(x => x["ColumnId"]).Return(1);
            mockReader.Stub(x => x["ColumnName"]).Return("Fubar");
            mockReader.Stub(x => x["PropertyName"]).Return(DBNull.Value);
            mockReader.Stub(x => x["PropertyValue"]).Return("Foo");

            var propertyModel = _mapper.Map(mockReader);

            Assert.IsNotNull(propertyModel);
            Assert.IsTrue(String.IsNullOrWhiteSpace(propertyModel.Name));
        }

        [TestMethod]
        public void Map_InValid_PropertyValue()
        {
            var mockReader = MockRepository.GenerateStub<IDataRecord>();
            mockReader.Stub(x => x["ObjectId"]).Return(1);
            mockReader.Stub(x => x["ObjectName"]).Return("Foobar");
            mockReader.Stub(x => x["SchemaName"]).Return("dbo");
            mockReader.Stub(x => x["ColumnId"]).Return(1);
            mockReader.Stub(x => x["ColumnName"]).Return("Fubar");
            mockReader.Stub(x => x["PropertyName"]).Return("Foo");
            mockReader.Stub(x => x["PropertyValue"]).Return(DBNull.Value);

            var propertyModel = _mapper.Map(mockReader);

            Assert.IsNotNull(propertyModel);
            Assert.IsTrue(String.IsNullOrWhiteSpace(propertyModel.Text));
        }

    }
}
