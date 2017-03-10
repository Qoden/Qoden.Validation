using System;
using NUnit.Framework;
using XAssert = NUnit.Framework.Assert;

namespace Qoden.Validation.Test
{
	[TestFixture]
    public class NullableValidationTest
    {
        [Test]
        public void Check_HasValue()
        {
            var nullable = new DateTime?(DateTime.Now);
            var v = new Validator();
            var initial = v.CheckValue(nullable);
            initial.OnErrorAction = error => { };
            var converted = initial.HasValue();
            XAssert.True(v.IsValid);
            XAssert.AreEqual(initial.Key, converted.Key);
            XAssert.AreEqual(initial.OnErrorAction, converted.OnErrorAction);
            XAssert.AreEqual(initial.Validator, converted.Validator);

            var check = v.CheckValue(new DateTime?()).HasValue();
            XAssert.False(v.IsValid);
            XAssert.AreEqual(check.Error["Validator"], "HasValue");
        }
    }
}
