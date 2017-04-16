using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
    [TestClass]
    public class UriValidationsTest
    {
        [TestMethod]
        public void UriValidation()
        {
            var v = new Validator();
            v.CheckValue(new Uri("http://google.com")).IsAbsoluteUri();
            XAssert.IsTrue(v.IsValid);
        }

        [TestMethod]
        public void RelativeUri()
        {
            var v = new Validator();
            v.CheckValue(new Uri("/path/some", UriKind.Relative)).IsAbsoluteUri();
            XAssert.IsFalse(v.IsValid);
        }
    }
}
