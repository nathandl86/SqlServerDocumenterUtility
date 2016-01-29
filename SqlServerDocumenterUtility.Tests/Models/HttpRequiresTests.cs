using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerDocumenterUtility.Models.Validation;
using System;

namespace SqlServerDocumenterUtility.Tests.Code
{
    [TestClass]
    public class HttpRequiresTests
    {

        [TestMethod]
        public void IsNotNull_ValidString()
        {
            //Act
            HttpRequires.IsNotNull("Test", String.Empty);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsNotNull_EmptyString()
        {
            //Act
            HttpRequires.IsNotNull("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsNotNull_NullString()
        {
            //Act
            HttpRequires.IsNotNull((String)null, "");
        }

        [TestMethod]
        public void IsNotNull_ValidObj()
        {
            //Act
            HttpRequires.IsNotNull(new object(), "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsNotNull_NullObj()
        {
            //Act
            HttpRequires.IsNotNull((object)null, "");
        }

        [TestMethod]
        public void IsNotNull_ValidNullableInt()
        {
            //Arrange
            int? i = 1;

            //Act
            HttpRequires.IsNotNull(i, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsNotNull_NullNullableInt()
        {
            //Arrange
            int? i = null;

            //Act
            HttpRequires.IsNotNull(i, "");
        }

        [TestMethod]
        public void IsTrue_Valid()
        {
            //Act
            HttpRequires.IsTrue(0 < 10, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsTrue_Invalid()
        {
            //Act
            HttpRequires.IsTrue(10 < 0, "");
        }

    }
}
