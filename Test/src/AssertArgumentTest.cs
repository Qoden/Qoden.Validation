using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
    [TestClass]
    public class ArgumentValidatorTest
    {
        [TestMethod]
        public void AssertNullArgument()
        {
            Qoden.Util.HttpUtility.ParseQueryString("aaaa=cc&ddd=3");

            var ex = XAssert.ThrowsException<ArgumentNullException>(()=>
            {
                Assert.Argument((string)null, "child").NotNull();    
            });
        }

        [TestMethod]
        public void AssertArgument()
        {
            var ex = XAssert.ThrowsException<ArgumentException>(() =>
            {
                Assert.Argument(false, "child").IsTrue();
            });
        }
    }
}
