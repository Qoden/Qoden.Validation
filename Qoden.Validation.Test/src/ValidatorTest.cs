using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
    public class ValidatorTest : ValidationsTestBase
    {
        [TestMethod]
        public void EmptyErrorListValid()
        {
            XAssert.IsTrue(Validator.IsValid);
            XAssert.IsFalse(Validator.HasErrors);
        }

        [TestMethod]
        public void ErrorListCanBeInspected()
        {
            var key11 = new Error("First rrror for Key1") { Key = "Key1" };
            Validator.Add(key11);
            var key12 = new Error("Second error for Key2") { Key = "Key1" };
            Validator.Add(key12);
            var key21 = new Error("Erorr for Key2") { Key = "Key2" };
            Validator.Add(key21);

            var key1Errors = Validator.ErrorsForKey("Key1");

			CollectionAssert.AreEqual(new[] { key11, key12 }, key1Errors.ToArray());
            var allErrors = Validator.Errors;
			CollectionAssert.AreEqual(new[] { key11, key12, key21 }, allErrors.ToArray());

            XAssert.IsTrue(Validator.HasErrorsForKey("Key1"));
            XAssert.IsTrue(Validator.HasErrorsForKey("Key2"));
        }

        [TestMethod]
        public void ErrorListThrows()
        {
            Validator.CheckValue(false, "x").IsTrue();
            Validator.CheckValue(true, "y").IsFalse();
            var ex = XAssert.ThrowsException<MultipleErrorsException>(() => Validator.Throw());
            XAssert.AreEqual(2, ex.Errors.Count);
        }
    }
}
