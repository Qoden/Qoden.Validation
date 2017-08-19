using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
    public class NullableValidationTest : ValidationsTestBase
    {
        [TestMethod]
        public void Check_HasValue()
        {
            var nullable = new DateTime?(DateTime.Now);
            var initial = Validator.CheckValue(nullable);
            initial.OnErrorAction = error => { };
            var converted = initial.HasValue();
            XAssert.IsTrue(Validator.IsValid);
            XAssert.AreEqual(initial.Key, converted.Key);
            XAssert.AreEqual(initial.OnErrorAction, converted.OnErrorAction);
            XAssert.AreEqual(initial.Validator, converted.Validator);

            var check = Validator.CheckValue(new DateTime?()).HasValue();
            XAssert.IsFalse(Validator.IsValid);
            XAssert.AreEqual(check.Error["Validator"], "HasValue");
        }
    }
}
