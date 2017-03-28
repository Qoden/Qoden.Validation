using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
    public class ComparableValidationsTest
    {
        [TestMethod]
        public void Check_LessOrEqualTo()
        {
            var v = new Validator();
            v.CheckValue(1, "One")
                .LessOrEqualTo(1)
                .LessOrEqualTo(2);
            XAssert.IsFalse(v.HasErrors);

            var check = v.CheckValue(1, "One");
            check.LessOrEqualTo(0);
            XAssert.AreEqual(check.Error["Max"], 0);
            XAssert.AreEqual(check.Error["Value"], 1);
            XAssert.AreEqual(check.Error["Validator"], "LessOrEqualTo");
        }

        [TestMethod]
        public void Check_Less()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One").Less(2);
            XAssert.IsFalse(v.HasErrors);
            check.Less(1);
            XAssert.AreEqual(check.Error["Max"], 1);
            XAssert.AreEqual(check.Error["Value"], 1);
            XAssert.AreEqual(check.Error["Validator"], "Less");
        }

        [TestMethod]
        public void Check_GreaterOrEqualTo()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One")
                .GreaterOrEqualTo(1)
                .GreaterOrEqualTo(0);
            XAssert.IsFalse(v.HasErrors);

            check.GreaterOrEqualTo(2);
            XAssert.AreEqual(check.Error["Min"], 2);
            XAssert.AreEqual(check.Error["Value"], 1);
            XAssert.AreEqual(check.Error["Validator"], "GreaterOrEqualTo");
        }

        [TestMethod]
        public void Check_Greater()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One").Greater(0);
            XAssert.IsFalse(v.HasErrors);
            check.Greater(1);
            XAssert.AreEqual(check.Error["Min"], 1);
            XAssert.AreEqual(check.Error["Value"], 1);
            XAssert.AreEqual(check.Error["Validator"], "Greater");
        }

        [TestMethod]
        public void Check_EqualsTo()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One").EqualsTo(1);
            XAssert.IsFalse(v.HasErrors);
            check.EqualsTo(0);
            XAssert.AreEqual(check.Error["Expected"], 0);
            XAssert.AreEqual(check.Error["Value"], 1);
            XAssert.AreEqual(check.Error["Validator"], "EqualsTo");
        }

        [TestMethod]
        public void Check_Between()
        {
            var v = new Validator();
            var check = v.CheckValue(1, "One")
                .EqualsTo(1)
                .BetweenInclusive(-1, 1)
                .BetweenInclusive(1, 2)
                .Between(0, 2);
            XAssert.IsFalse(v.HasErrors);

            check.Between(1, 2);
            XAssert.AreEqual(check.Error["Min"], 1);
            XAssert.AreEqual(check.Error["Max"], 2);
            XAssert.AreEqual(check.Error["Value"], 1);
            XAssert.AreEqual(check.Error["Validator"], "Between");

            check.Between(0, 1);
            XAssert.AreEqual(check.Error["Min"], 0);
            XAssert.AreEqual(check.Error["Max"], 1);
            XAssert.AreEqual(check.Error["Value"], 1);
            XAssert.AreEqual(check.Error["Validator"], "Between");

            check.BetweenInclusive(-1, 0);
            XAssert.AreEqual(check.Error["Min"], -1);
            XAssert.AreEqual(check.Error["Max"], 0);
            XAssert.AreEqual(check.Error["Value"], 1);
            XAssert.AreEqual(check.Error["Validator"], "BetweenInclusive");

            check.BetweenInclusive(2, 3);
            XAssert.AreEqual(check.Error["Min"], 2);
            XAssert.AreEqual(check.Error["Max"], 3);
            XAssert.AreEqual(check.Error["Value"], 1);
            XAssert.AreEqual(check.Error["Validator"], "BetweenInclusive");
        }
    }
}