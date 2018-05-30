using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
    [TestClass]
    public class NumberValidationsTest : ValidationsTestBase
    {
        [TestMethod]
        public void NumberValidation()
        {
			Validator.CheckValue("234.33", "someNumber").IsNumber();
            XAssert.IsTrue(Validator.IsValid);
        }

        [TestMethod]
        public void InvalidNumbers()
        {
			Validator.CheckValue("s34d", "someNumber").IsNumber();
            XAssert.IsFalse(Validator.IsValid);

			Validator.CheckValue("234.", "someNumber").IsNumber();
            XAssert.IsFalse(Validator.IsValid);

			Validator.CheckValue(".45", "someNumber").IsNumber();
            XAssert.IsFalse(Validator.IsValid);
        }

        [TestMethod]
        public void NullNumber()
        {
			Validator.CheckValue((string)null).IsNumber();
            XAssert.IsFalse(Validator.IsValid);
        }
    }
}
