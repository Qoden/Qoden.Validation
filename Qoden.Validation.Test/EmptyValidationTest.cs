using Xunit;
using XAssert = Xunit.Assert;

namespace Qoden.Validation.Test
{
    public class EmptyValidationTest
    {
        [Fact]
        public void Check_EmptyString()
        {
            var v = new Validator();
            v.CheckValue("1", "One").NotEmpty().NotNull();
            XAssert.False(v.HasErrors);
            v.CheckValue<string>(null, "Null").IsNull();
            XAssert.False(v.HasErrors);

            var check = v.CheckValue("", "Empty").NotEmpty();
            XAssert.True(check.HasError);
            XAssert.Equal(check.Error["Validator"], "NotEmpty");

            check = v.CheckValue<string>(null, "Null").NotNull();
            XAssert.True(check.HasError);
            XAssert.Equal(check.Error["Validator"], "NotNull");

            check = v.CheckValue("", "NotNull").IsNull();
            XAssert.True(check.HasError);
            XAssert.Equal(check.Error["Validator"], "IsNull");
        }
    }
}
