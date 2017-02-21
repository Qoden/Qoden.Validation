using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Qoden.Validation.Test
{
    public class LengthValidationTest
    {
        [Fact]
        public void LengthValidation()
        {
            var v = new Validator();
            var value = new List<int> { 1, 2, 3 };
            v.CheckValue(value, "Array").MinLength(3).MaxLength(3);
            Xunit.Assert.False(v.HasErrors);

            var check = v.CheckValue(value, "Array");
            check.MinLength(4);
            Xunit.Assert.Equal(check.Error["Min"], 4);
            Xunit.Assert.Equal(check.Error["Value"], 3);
            Xunit.Assert.Equal(check.Error["Validator"], "MinLength");

            check = v.CheckValue(value, "Array");
            check.MaxLength(2);
            Xunit.Assert.Equal(check.Error["Max"], 2);
            Xunit.Assert.Equal(check.Error["Value"], 3);
            Xunit.Assert.Equal(check.Error["Validator"], "MaxLength");
        }
    }
}
