using Nancy.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy;
using SqlServerDocumenterUtility.NancyApi.Modules;
using SqlServerDocumenterUtility.Data.Dals;
using Rhino.Mocks;
using SqlServerDocumenterUtility.Models;
using System.Collections.Generic;
using System;

namespace SqlServerDocumenterUtility.Tests.Nancy
{
    [TestClass]
    public class DocumenterModuleTests
    {

        [TestMethod]
        public void Should_return_not_found_when_no_route()
        {
            //Arrange
            var stubTableDal = MockRepository.GenerateStub<ITableDal>();
            var stubColumnDal = MockRepository.GenerateStub<IColumnDal>();
            var browser = new Browser(cfg =>
            {
                cfg.Module<DocumenterModule>();
                cfg.Dependencies<ITableDal>(stubTableDal);
                cfg.Dependencies<IColumnDal>(stubColumnDal);
            });

            //Act
            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void Should_return_ok_when_route_exists()
        {
            //Arrange
            var stubTableDal = MockRepository.GenerateStub<ITableDal>();
            var stubColumnDal = MockRepository.GenerateStub<IColumnDal>();
            var browser = new Browser(cfg =>
            {
                cfg.Module<DocumenterModule>();
                cfg.Dependencies<ITableDal>(stubTableDal);
                cfg.Dependencies<IColumnDal>(stubColumnDal);
            });

            //Act
            var result = browser.Get("nancy/", with =>
            {
                with.HttpRequest();
            });

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Hello World!", result.Body.AsString());
        }

        [TestMethod]
        public void Should_get_tables()
        {
            //Arrange
            var stubColumnDal = MockRepository.GenerateStub<IColumnDal>();
            var mockTableDal = MockRepository.GenerateMock<ITableDal>();
            mockTableDal.Stub(x => x.GetTables(
                Arg<string>.Is.Anything
            )).Return(new DalResponseModel<IList<TableModel>>
            {
                HasError = false,
                Result = new List<TableModel>
                {
                    new TableModel()
                }
            });

            var browser = new Browser(with =>
            {
                with.Module<DocumenterModule>();
                with.Dependencies<ITableDal>(mockTableDal);
                with.Dependencies<IColumnDal>(stubColumnDal);
            });

            //Act
            var result = browser.Get("nancy/tables", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.Header("connectionString", "fake_connection");
            });

            //Assert
            var tables = result.Body.DeserializeJson<IList<TableModel>>();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(tables.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Should_fail_get_tables_without_header()
        {
            //Arrange
            var stubColumnDal = MockRepository.GenerateStub<IColumnDal>();
            var mockTableDal = MockRepository.GenerateMock<ITableDal>();
            mockTableDal.Stub(x => x.GetTables(
                Arg<string>.Is.Anything
            )).Return(new DalResponseModel<IList<TableModel>>
            {
                HasError = false,
                Result = new List<TableModel>
                {
                    new TableModel()
                }
            });

            var browser = new Browser(with =>
            {
                with.Module<DocumenterModule>();
                with.Dependencies<ITableDal>(mockTableDal);
                with.Dependencies<IColumnDal>(stubColumnDal);
            });

            //Assert
            var result = browser.Get("nancy/tables", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
            });            
        }

        [TestMethod]
        public void Should_get_databases()
        {
            //Arrange
            var stubTableDal = MockRepository.GenerateStub<ITableDal>();
            var stubColumnDal = MockRepository.GenerateStub<IColumnDal>();
            var browser = new Browser(cfg =>
            {
                cfg.Module<DocumenterModule>();
                cfg.Dependencies<ITableDal>(stubTableDal);
                cfg.Dependencies<IColumnDal>(stubColumnDal);
            });

            //Act
            var result = browser.Get("nancy/databases", with =>
            {
                with.HttpRequest();
                with.Header("Accept", "application/json");
            });

            //Assert
            var databases = result.Body.DeserializeJson < IList<DatabaseModel>>();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(databases.Count > 0);
        }

    }
}
