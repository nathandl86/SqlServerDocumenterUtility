using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerDocumenterUtility.Models;
using SqlServerDocumenterUtility.Models.Exceptions;
using SqlServerDocumenterUtility.Models.Validation;
using System;

namespace SqlServerDocumenterUtility.Tests.Models
{
    [TestClass]
    public class HttpAssertTests
    {
        [TestMethod]
        public void Success_Valid()
        {
            HttpAssert.Success(new DalResponseModel<bool> { HasError = false });
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Success_InValid()
        {
            var dalResp = new DalResponseModel<bool> {
                HasError = true,
                Exception = new NullReferenceException()
            };

            HttpAssert.Success(dalResp);
        }

        [TestMethod]
        public void NotNull_Valid()
        {
            var dalResp = new DalResponseModel<ColumnModel>
            {
                HasError = false,
                Result = new ColumnModel()
            };

            HttpAssert.NotNull(dalResp, "");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void NotNull_Invalid()
        {
            var dalResp = new DalResponseModel<ColumnModel>
            {
                Result = null,
                HasError = false
            };

            HttpAssert.NotNull(dalResp, "");
        }
    }
}
