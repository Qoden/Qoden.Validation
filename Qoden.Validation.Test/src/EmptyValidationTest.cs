using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
    public class EmptyValidationTest : ValidationsTestBase
    {
        [TestMethod]
        public void Check_EmptyString()
        {
            Validator.CheckValue("1", "One").NotEmpty().NotNull();
            XAssert.IsFalse(Validator.HasErrors);
            Validator.CheckValue<string>(null, "Null").IsNull();
            XAssert.IsFalse(Validator.HasErrors);

            var check = Validator.CheckValue("", "Empty").NotEmpty();
            XAssert.IsTrue(check.HasError);
            XAssert.AreEqual(check.Error["Validator"], "NotEmpty");

            check = Validator.CheckValue<string>(null, "Null").NotNull();
            XAssert.IsTrue(check.HasError);
            XAssert.AreEqual(check.Error["Validator"], "NotNull");

            check = Validator.CheckValue("", "NotNull").IsNull();
            XAssert.IsTrue(check.HasError);
            XAssert.AreEqual(check.Error["Validator"], "IsNull");
        }
    }
}
