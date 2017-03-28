using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
    public class EmptyValidationTest
    {
        [TestMethod]
        public void Check_EmptyString()
        {
            var v = new Validator();
            v.CheckValue("1", "One").NotEmpty().NotNull();
            XAssert.IsFalse(v.HasErrors);
            v.CheckValue<string>(null, "Null").IsNull();
            XAssert.IsFalse(v.HasErrors);

            var check = v.CheckValue("", "Empty").NotEmpty();
            XAssert.IsTrue(check.HasError);
            XAssert.AreEqual(check.Error["Validator"], "NotEmpty");

            check = v.CheckValue<string>(null, "Null").NotNull();
            XAssert.IsTrue(check.HasError);
            XAssert.AreEqual(check.Error["Validator"], "NotNull");

            check = v.CheckValue("", "NotNull").IsNull();
            XAssert.IsTrue(check.HasError);
            XAssert.AreEqual(check.Error["Validator"], "IsNull");
        }
    }
}
