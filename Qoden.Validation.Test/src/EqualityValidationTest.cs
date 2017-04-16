using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
    public class EqualityValidationTest : ValidationsTestBase
    {
        [TestMethod]
        public void Check_IsTrue_IsFalse()
        {
            Validator.CheckValue(true, "One").IsTrue();
            XAssert.IsFalse(Validator.HasErrors);
            Validator.CheckValue(false, "One").IsFalse();
            XAssert.IsFalse(Validator.HasErrors);

            var falseCheck = Validator.CheckValue(false, "One").IsTrue();
            XAssert.AreEqual(falseCheck.Error["Value"], false);
            XAssert.AreEqual(falseCheck.Error["Expected"], true);
            XAssert.AreEqual(falseCheck.Error["Validator"], "IsTrue");

            var trueCheck = Validator.CheckValue(true, "One").IsFalse();
            XAssert.AreEqual(trueCheck.Error["Value"], true);
            XAssert.AreEqual(trueCheck.Error["Expected"], false);
            XAssert.AreEqual(trueCheck.Error["Validator"], "IsFalse");
        }

        [TestMethod]
        public void Check_EqualsTo()
        {
            Validator.CheckValue("1", "One").EqualsTo("1");
            var oneCheck = Validator.CheckValue("1", "One").EqualsTo("2");
            XAssert.AreEqual(oneCheck.Error["Value"], "1");
            XAssert.AreEqual(oneCheck.Error["Expected"], "2");
            XAssert.AreEqual(oneCheck.Error["Validator"], "EqualsTo");
        }

        [TestMethod]
        public void Check_NotEquals()
        {
            var check = Validator.CheckValue("AAA").NotEqualsTo("BBB");
            XAssert.IsTrue(check.IsValid);
            check.NotEqualsTo("AAA");
            XAssert.IsFalse(check.IsValid);
            XAssert.AreEqual(check.Error["Validator"], "NotEqualsTo");
        }

        [TestMethod]
        public void Check_In()
        {
            Validator.CheckValue("AAA").In(new[] { "BBB", "AAA" });
            XAssert.IsTrue(Validator.IsValid);
            var check = Validator.CheckValue("AAA").In(new[] { "CCCC", "ZZZ" });
            XAssert.IsFalse(Validator.IsValid);
            XAssert.AreEqual(check.Error["Validator"], "In");
        }

        [TestMethod]
        public void Check_NotIn()
        {
            Validator.CheckValue("CCCC").NotIn(new[] { "BBB", "AAA" });
            XAssert.IsTrue(Validator.IsValid);
            var check = Validator.CheckValue("CCCC").NotIn(new[] { "CCCC", "ZZZ" });
            XAssert.IsFalse(Validator.IsValid);
            XAssert.AreEqual(check.Error["Validator"], "NotIn");
        }
    }
}
