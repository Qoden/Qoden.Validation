using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
    public class ConversionValidationTest : ValidationsTestBase
    {
        [TestMethod]
        public void Check_ConvertTo()
        {
            var initial = Validator.CheckValue("1", "One");
            initial.OnErrorAction = error => { };
            var converted = initial.ConvertTo<int>();
            XAssert.IsFalse(Validator.HasErrors);
            XAssert.AreEqual(initial.Key, converted.Key);
            XAssert.AreEqual(initial.OnErrorAction, converted.OnErrorAction);
            XAssert.AreEqual(initial.Validator, converted.Validator);

            var check = Validator.CheckValue("abc", "ABC").ConvertTo<int>();
            XAssert.IsTrue(check.HasError);
            XAssert.IsTrue(check.Error["Exception"] is Exception);
            XAssert.AreEqual(check.Error["Value"], "abc");
            XAssert.AreEqual(check.Error["Validator"], "ConvertTo");
        }
    }
}
