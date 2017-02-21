using Xunit;
using XAssert = Xunit.Assert;

namespace Qoden.Validation.Test
{
    public class EqualityValidationTest
    {
        [Fact]
        public void Check_IsTrue_IsFalse()
        {
            var v = new Validator();
            v.CheckValue(true, "One").IsTrue();
            XAssert.False(v.HasErrors);
            v.CheckValue(false, "One").IsFalse();
            XAssert.False(v.HasErrors);

            var falseCheck = v.CheckValue(false, "One").IsTrue();
            XAssert.Equal(falseCheck.Error["Value"], false);
            XAssert.Equal(falseCheck.Error["Expected"], true);
            XAssert.Equal(falseCheck.Error["Validator"], "IsTrue");

            var trueCheck = v.CheckValue(true, "One").IsFalse();
            XAssert.Equal(trueCheck.Error["Value"], true);
            XAssert.Equal(trueCheck.Error["Expected"], false);
            XAssert.Equal(trueCheck.Error["Validator"], "IsFalse");
        }

        [Fact]
        public void Check_EqualsTo()
        {
            var v = new Validator();
            v.CheckValue("1", "One").EqualsTo("1");
            var oneCheck = v.CheckValue("1", "One").EqualsTo("2");
            XAssert.Equal(oneCheck.Error["Value"], "1");
            XAssert.Equal(oneCheck.Error["Expected"], "2");
            XAssert.Equal(oneCheck.Error["Validator"], "EqualsTo");
        }

        [Fact]
        public void Check_NotEquals()
        {
            var v = new Validator();
            var check = v.CheckValue("AAA").NotEqualsTo("BBB");
            XAssert.True(check.IsValid);
            check.NotEqualsTo("AAA");
            XAssert.False(check.IsValid);
            XAssert.Equal(check.Error["Validator"], "NotEqualsTo");
        }

        [Fact]
        public void Check_In()
        {
            var v = new Validator();
            v.CheckValue("AAA").In(new[] { "BBB", "AAA" });
            XAssert.True(v.IsValid);
            var check = v.CheckValue("AAA").In(new[] { "CCCC", "ZZZ" });
            XAssert.False(v.IsValid);
            XAssert.Equal(check.Error["Validator"], "In");

            
        }

        [Fact]
        public void Check_NotIn()
        {
            var v = new Validator();
            v.CheckValue("CCCC").NotIn(new[] { "BBB", "AAA" });
            XAssert.True(v.IsValid);
            var check = v.CheckValue("CCCC").NotIn(new[] { "CCCC", "ZZZ" });
            XAssert.False(v.IsValid);
            XAssert.Equal(check.Error["Validator"], "NotIn");
        }
    }
}
