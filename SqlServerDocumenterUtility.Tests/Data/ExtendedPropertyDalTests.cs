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
    public class ExtendedPropertyDalTests
    {
        private ExtendedPropertyDal _extendedPropertyDal;
        private string _connectionString;

        [TestInitialize]
        public void Init()
        {
            _extendedPropertyDal = new ExtendedPropertyDal();
            _connectionString = ConfigurationManager.ConnectionStrings["test"].ConnectionString;
        }

        [TestMethod]
        public void RetrieveByTableId()
        {
            var mockHelper = BuildMockDalHelper();
            _extendedPropertyDal.DalHelper = mockHelper;

            var dalResponse = _extendedPropertyDal.RetrieveByTableId(0, _connectionString);

            Assert.IsFalse(dalResponse.HasError);
            Assert.IsNotNull(dalResponse.Result);
        }

        [TestMethod]
        public void DeleteProperty()
        {
            var mockHelper = BuildMockDalHelper();
            _extendedPropertyDal.DalHelper = mockHelper;
            var modelStub = MockRepository.GenerateStub<ExtendedPropertyModel>();
            

            var dalResponse = _extendedPropertyDal.DeleteProperty(modelStub, _connectionString);

            Assert.IsFalse(dalResponse.HasError);
        }

        [TestMethod]
        public void AddProperty()
        {
            var mockHelper = BuildMockDalHelper();
            _extendedPropertyDal.DalHelper = mockHelper;
            var modelStub = MockRepository.GenerateStub<ExtendedPropertyModel>();

            var dalResponse = _extendedPropertyDal.AddProperty(modelStub, _connectionString);

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
