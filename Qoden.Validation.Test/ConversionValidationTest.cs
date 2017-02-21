using System;
using Xunit;
using XAssert = Xunit.Assert;

namespace Qoden.Validation.Test
{
    public class ConversionValidationTest
    {
        [Fact]
        public void Check_ConvertTo()
        {
            var v = new Validator();
            var initial = v.CheckValue("1", "One");
            initial.OnErrorAction = error => { };
            var converted = initial.ConvertTo<int>();
            XAssert.False(v.HasErrors);
            XAssert.Equal(initial.Key, converted.Key);
            XAssert.Equal(initial.OnErrorAction, converted.OnErrorAction);
            XAssert.Equal(initial.Validator, converted.Validator);

            var check = v.CheckValue("abc", "ABC").ConvertTo<int>();
            XAssert.True(check.HasError);
            XAssert.True(check.Error["Exception"] is Exception);
            XAssert.Equal(check.Error["Value"], "abc");
            XAssert.Equal(check.Error["Validator"], "ConvertTo");
        }
    }
}
