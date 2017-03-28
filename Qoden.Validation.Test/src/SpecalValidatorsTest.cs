using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
	public class SpecalValidatorsTest
	{
		[TestMethod]
		public void AssertContainsError()
		{
			var ex = XAssert.ThrowsException<ArgumentException>(
				() => Assert.Argument("", "key").NotEmpty());
			XAssert.IsNotNull(ex.GetQodenError());
			XAssert.IsInstanceOfType(ex.GetQodenError(), typeof(Error));
		}

		[TestMethod]
		public void InvalidOperationContainsError()
		{
			var ex = XAssert.ThrowsException<InvalidOperationException>(
				() => Assert.State("", "key").NotEmpty());			
			XAssert.IsNotNull(ex.GetQodenError());
			XAssert.IsInstanceOfType(ex.GetQodenError(), typeof(Error));
		}
	}
}
