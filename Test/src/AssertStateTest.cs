using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
	public class AssertStateTest
	{
        [TestMethod]
        public void AssertState()
        {
            var ex = XAssert.ThrowsException<InvalidOperationException>(() =>
            {
                Assert.State((string)null, "child").NotNull();
            });
        }
	}
}
