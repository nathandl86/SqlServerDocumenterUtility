using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerDocumenterUtility.Controllers;
using System.Web;
using Rhino.Mocks;
using System.Web.Routing;

namespace SqlServerDocumenterUtility.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController _controller;

        [TestInitialize]
        public void Init()
        {
            _controller = new HomeController();
        }

        private const string titleTemplate = "SQL Server Utility | ";

        [TestMethod]
        public void Index()
        {
            ViewResult result = _controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(titleTemplate + "Default", result.ViewBag.Title);
        }

        [TestMethod]
        public void About()
        {
            var result = _controller.About() as PartialViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(titleTemplate + "About", result.ViewBag.Title);
        }

        [TestMethod]
        public void Ipsum()
        {
            var result = _controller.Ipsum() as PartialViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(titleTemplate + "Ipsum", result.ViewBag.Title);
        }

        [TestMethod]
        public void Readme()
        {
            var mockResponse = MockRepository.GenerateMock<HttpResponseBase>();
            var mockContext = MockRepository.GenerateMock<HttpContextBase>();
            mockContext.Stub(x => x.Response).Return(mockResponse);

            var context = new ControllerContext(mockContext, new RouteData(), _controller);
            _controller.ControllerContext = context;

            var result = _controller.Readme() as FilePathResult;
            Assert.IsNotNull(result);
        }
    }
}
