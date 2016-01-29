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
            //Arrange
            var response = new DalResponseModel<bool> { HasError = false };

            //Act
            HttpAssert.Success(response);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Success_InValid()
        {
            //Arrange
            var dalResp = new DalResponseModel<bool> {
                HasError = true,
                Exception = new NullReferenceException()
            };

            //Act
            HttpAssert.Success(dalResp);
        }

        [TestMethod]
        public void NotNull_Valid()
        {
            //Arrange
            var dalResp = new DalResponseModel<ColumnModel>
            {
                HasError = false,
                Result = new ColumnModel()
            };

            //Act
            HttpAssert.NotNull(dalResp, "");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void NotNull_Invalid()
        {
            //Arrange
            var dalResp = new DalResponseModel<ColumnModel>
            {
                Result = null,
                HasError = false
            };

            //Act
            HttpAssert.NotNull(dalResp, "");
        }
    }
}
