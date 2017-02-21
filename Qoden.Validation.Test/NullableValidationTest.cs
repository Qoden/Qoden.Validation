using System;
using Xunit;
using XAssert = Xunit.Assert;

namespace Qoden.Validation.Test
{
    public class NullableValidationTest
    {
        [Fact]
        public void Check_HasValue()
        {
            var nullable = new DateTime?(DateTime.Now);
            var v = new Validator();
            var initial = v.CheckValue(nullable);
            initial.OnErrorAction = error => { };
            var converted = initial.HasValue();
            XAssert.True(v.IsValid);
            XAssert.Equal(initial.Key, converted.Key);
            XAssert.Equal(initial.OnErrorAction, converted.OnErrorAction);
            XAssert.Equal(initial.Validator, converted.Validator);

            var check = v.CheckValue(new DateTime?()).HasValue();
            XAssert.False(v.IsValid);
            XAssert.Equal(check.Error["Validator"], "HasValue");
        }
    }
}
