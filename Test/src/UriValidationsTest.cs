using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
    [TestClass]
    public class UriValidationsTest : ValidationsTestBase
    {
        [TestMethod]
        public void UriValidation()
        {
            Validator.CheckValue(new Uri("http://google.com")).IsAbsoluteUri();
            XAssert.IsTrue(Validator.IsValid);

            Validator.CheckValue("http://google.com").IsAbsoluteUri();
            XAssert.IsTrue(Validator.IsValid);

            Validator.CheckValue("/path/some").IsAbsoluteUri();
            XAssert.IsTrue(Validator.IsValid); //File Uri
        }

        [TestMethod]
        public void RelativeUri()
        {
            Validator.CheckValue(new Uri("/path/some", UriKind.Relative)).IsAbsoluteUri();
            XAssert.IsFalse(Validator.IsValid);
        }

        [TestMethod]
        public void Nulls()
        {
            Validator.CheckValue((string)null).IsAbsoluteUri();
            XAssert.IsFalse(Validator.IsValid);

            Validator.CheckValue((Uri)null).IsAbsoluteUri();
            XAssert.IsFalse(Validator.IsValid);
        }
    }
}
