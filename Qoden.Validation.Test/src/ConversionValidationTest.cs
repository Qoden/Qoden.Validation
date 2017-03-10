using System;
using NUnit.Framework;
using XAssert = NUnit.Framework.Assert;

namespace Qoden.Validation.Test
{
	[TestFixture]
    public class ConversionValidationTest
    {
        [Test]
        public void Check_ConvertTo()
        {
            var v = new Validator();
            var initial = v.CheckValue("1", "One");
            initial.OnErrorAction = error => { };
            var converted = initial.ConvertTo<int>();
            XAssert.False(v.HasErrors);
            XAssert.AreEqual(initial.Key, converted.Key);
            XAssert.AreEqual(initial.OnErrorAction, converted.OnErrorAction);
            XAssert.AreEqual(initial.Validator, converted.Validator);

            var check = v.CheckValue("abc", "ABC").ConvertTo<int>();
            XAssert.True(check.HasError);
            XAssert.True(check.Error["Exception"] is Exception);
            XAssert.AreEqual(check.Error["Value"], "abc");
            XAssert.AreEqual(check.Error["Validator"], "ConvertTo");
        }
    }
}
