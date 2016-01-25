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
            HttpRequires.IsNotNull("Test", String.Empty);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsNotNull_EmptyString()
        {
            HttpRequires.IsNotNull("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsNotNull_NullString()
        {
            HttpRequires.IsNotNull((String)null, "");
        }

        [TestMethod]
        public void IsNotNull_ValidObj()
        {
            HttpRequires.IsNotNull(new object(), "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsNotNull_NullObj()
        {
            HttpRequires.IsNotNull((object)null, "");
        }

        [TestMethod]
        public void IsNotNull_ValidNullableInt()
        {
            int? i = 1;
            HttpRequires.IsNotNull(i, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsNotNull_NullNullableInt()
        {
            int? i = null;
            HttpRequires.IsNotNull(i, "");
        }

        [TestMethod]
        public void IsTrue_Valid()
        {
            HttpRequires.IsTrue(0 < 10, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsTrue_Invalid()
        {
            HttpRequires.IsTrue(10 < 0, "");
        }

    }
}
