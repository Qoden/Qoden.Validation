using NUnit.Framework;
using XAssert = NUnit.Framework.Assert;

namespace Qoden.Validation.Test
{
	[TestFixture]
    public class EqualityValidationTest
    {
        [Test]
        public void Check_IsTrue_IsFalse()
        {
            var v = new Validator();
            v.CheckValue(true, "One").IsTrue();
            XAssert.False(v.HasErrors);
            v.CheckValue(false, "One").IsFalse();
            XAssert.False(v.HasErrors);

            var falseCheck = v.CheckValue(false, "One").IsTrue();
            XAssert.AreEqual(falseCheck.Error["Value"], false);
            XAssert.AreEqual(falseCheck.Error["Expected"], true);
            XAssert.AreEqual(falseCheck.Error["Validator"], "IsTrue");

            var trueCheck = v.CheckValue(true, "One").IsFalse();
            XAssert.AreEqual(trueCheck.Error["Value"], true);
            XAssert.AreEqual(trueCheck.Error["Expected"], false);
            XAssert.AreEqual(trueCheck.Error["Validator"], "IsFalse");
        }

        [Test]
        public void Check_EqualsTo()
        {
            var v = new Validator();
            v.CheckValue("1", "One").EqualsTo("1");
            var oneCheck = v.CheckValue("1", "One").EqualsTo("2");
            XAssert.AreEqual(oneCheck.Error["Value"], "1");
            XAssert.AreEqual(oneCheck.Error["Expected"], "2");
            XAssert.AreEqual(oneCheck.Error["Validator"], "EqualsTo");
        }

        [Test]
        public void Check_NotEquals()
        {
            var v = new Validator();
            var check = v.CheckValue("AAA").NotEqualsTo("BBB");
            XAssert.True(check.IsValid);
            check.NotEqualsTo("AAA");
            XAssert.False(check.IsValid);
            XAssert.AreEqual(check.Error["Validator"], "NotEqualsTo");
        }

        [Test]
        public void Check_In()
        {
            var v = new Validator();
            v.CheckValue("AAA").In(new[] { "BBB", "AAA" });
            XAssert.True(v.IsValid);
            var check = v.CheckValue("AAA").In(new[] { "CCCC", "ZZZ" });
            XAssert.False(v.IsValid);
            XAssert.AreEqual(check.Error["Validator"], "In");

            
        }

        [Test]
        public void Check_NotIn()
        {
            var v = new Validator();
            v.CheckValue("CCCC").NotIn(new[] { "BBB", "AAA" });
            XAssert.True(v.IsValid);
            var check = v.CheckValue("CCCC").NotIn(new[] { "CCCC", "ZZZ" });
            XAssert.False(v.IsValid);
            XAssert.AreEqual(check.Error["Validator"], "NotIn");
        }
    }
}
