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
    public class ExtendedPropertyDalTests
    {
        private string _connectionString;

        [TestInitialize]
        public void Init()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
        }

        [TestMethod]
        public void RetrieveByTableId()
        {
            //Arrange
            var mockHelper = BuildMockDalHelper();
            var mapperStub = MockRepository.GenerateStub<IExtendedPropertyModelMapper>();
            var extendedPropertyDal = new ExtendedPropertyDal(mapperStub, mockHelper);

            //Act
            var dalResponse = extendedPropertyDal.RetrieveByTableId(0, _connectionString);

            //Assert
            Assert.IsFalse(dalResponse.HasError);
            Assert.IsNotNull(dalResponse.Result);
        }

        [TestMethod]
        public void DeleteProperty()
        {
            //Arrange
            var mockHelper = BuildMockDalHelper();
            var modelStub = MockRepository.GenerateStub<ExtendedPropertyModel>();
            var mapperStub = MockRepository.GenerateStub<IExtendedPropertyModelMapper>();
            var extendedPropertyDal = new ExtendedPropertyDal(mapperStub, mockHelper);

            //Act
            var dalResponse = extendedPropertyDal.DeleteProperty(modelStub, _connectionString);

            //Assert
            Assert.IsFalse(dalResponse.HasError);
        }

        [TestMethod]
        public void AddProperty()
        {
            //Arrange
            var mockHelper = BuildMockDalHelper();
            var modelStub = MockRepository.GenerateStub<ExtendedPropertyModel>();
            var mapperStub = MockRepository.GenerateStub<IExtendedPropertyModelMapper>();
            var extendedPropertyDal = new ExtendedPropertyDal(mapperStub, mockHelper);

            //Act
            var dalResponse = extendedPropertyDal.AddProperty(modelStub, _connectionString);

            //Assert
            Assert.IsFalse(dalResponse.HasError);
            Assert.IsNotNull(dalResponse.Result);
            Assert.IsTrue(dalResponse.Result);
        }


        private IDalHelper BuildMockDalHelper()
        {
            var mockHelper = MockRepository.GenerateMock<IDalHelper>();

            mockHelper.Stub(x => x.Delete(Arg<DalHelperModel>.Is.Anything));
            mockHelper.Stub(x => x.Insert(Arg<DalHelperModel>.Is.Anything)).Return(1);
            mockHelper.Stub(x => x.RetrieveList(
                    Arg<DalHelperModel<ExtendedPropertyModel>>.Is.Anything
                )).Return(new List<ExtendedPropertyModel>
                {
                    new ExtendedPropertyModel()
                });

            return mockHelper;
        }
    }
}
