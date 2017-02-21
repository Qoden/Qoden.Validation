using Xunit;
using XAssert = Xunit.Assert;

namespace Qoden.Validation.Test
{
    public class ComparableValidationsTest
    {
        [Fact]
        public void Check_LessOrEqualTo()
        {
            var v = new Validator();
            v.CheckValue(1, "One")
                .LessOrEqualTo(1)
                .LessOrEqualTo(2);
            XAssert.False(v.HasErrors);

            var check = v.CheckValue(1, "One");
            check.LessOrEqualTo(0);
            XAssert.Equal(check.Error["Max"], 0);
            XAssert.Equal(check.Error["Value"], 1);
            XAssert.Equal(check.Error["Validator"], "LessOrEqualTo");
        }

        [Fact]
        public void Check_Less()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One").Less(2);
            XAssert.False(v.HasErrors);
            check.Less(1);
            XAssert.Equal(check.Error["Max"], 1);
            XAssert.Equal(check.Error["Value"], 1);
            XAssert.Equal(check.Error["Validator"], "Less");
        }

        [Fact]
        public void Check_GreaterOrEqualTo()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One")
                .GreaterOrEqualTo(1)
                .GreaterOrEqualTo(0);
            XAssert.False(v.HasErrors);

            check.GreaterOrEqualTo(2);
            XAssert.Equal(check.Error["Min"], 2);
            XAssert.Equal(check.Error["Value"], 1);
            XAssert.Equal(check.Error["Validator"], "GreaterOrEqualTo");
        }

        [Fact]
        public void Check_Greater()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One").Greater(0);
            XAssert.False(v.HasErrors);
            check.Greater(1);
            XAssert.Equal(check.Error["Min"], 1);
            XAssert.Equal(check.Error["Value"], 1);
            XAssert.Equal(check.Error["Validator"], "Greater");
        }

        [Fact]
        public void Check_EqualsTo()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One").EqualsTo(1);
            XAssert.False(v.HasErrors);
            check.EqualsTo(0);
            XAssert.Equal(check.Error["Expected"], 0);
            XAssert.Equal(check.Error["Value"], 1);
            XAssert.Equal(check.Error["Validator"], "EqualsTo");
        }

        [Fact]
        public void Check_Between()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One")
                .EqualsTo(1)
                .BetweenInclusive(-1, 1)
                .BetweenInclusive(1, 2)
                .Between(0, 2);
            XAssert.False(v.HasErrors);

            check.Between(1, 2);
            XAssert.Equal(check.Error["Min"], 1);
            XAssert.Equal(check.Error["Max"], 2);
            XAssert.Equal(check.Error["Value"], 1);
            XAssert.Equal(check.Error["Validator"], "Between");

            check.Between(0, 1);
            XAssert.Equal(check.Error["Min"], 0);
            XAssert.Equal(check.Error["Max"], 1);
            XAssert.Equal(check.Error["Value"], 1);
            XAssert.Equal(check.Error["Validator"], "Between");

            check.BetweenInclusive(-1, 0);
            XAssert.Equal(check.Error["Min"], -1);
            XAssert.Equal(check.Error["Max"], 0);
            XAssert.Equal(check.Error["Value"], 1);
            XAssert.Equal(check.Error["Validator"], "BetweenInclusive");

            check.BetweenInclusive(2, 3);
            XAssert.Equal(check.Error["Min"], 2);
            XAssert.Equal(check.Error["Max"], 3);
            XAssert.Equal(check.Error["Value"], 1);
            XAssert.Equal(check.Error["Validator"], "BetweenInclusive");
        }
    }
}