using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
    public class ValidatorTest
    {
        [TestMethod]
        public void EmptyErrorListValid()
        {
            var errors = new Validator();
            XAssert.IsTrue(errors.IsValid);
            XAssert.IsFalse(errors.HasErrors);
        }

        [TestMethod]
        public void ErrorListCanBeInspected()
        {
            var errors = new Validator();
            var key11 = new Error("First rrror for Key1") { Key = "Key1" };
            errors.Add(key11);
            var key12 = new Error("Second error for Key2") { Key = "Key1" };
            errors.Add(key12);
            var key21 = new Error("Erorr for Key2") { Key = "Key2" };
            errors.Add(key21);

            var key1Errors = errors.ErrorsForKey("Key1");

			CollectionAssert.AreEqual(new[] { key11, key12 }, key1Errors.ToArray());
            var allErrors = errors.Errors;
			CollectionAssert.AreEqual(new[] { key11, key12, key21 }, allErrors.ToArray());

            XAssert.IsTrue(errors.HasErrorsForKey("Key1"));
            XAssert.IsTrue(errors.HasErrorsForKey("Key2"));
        }

        [TestMethod]
        public void ErrorListThrows()
        {
            var errors = new Validator();
            errors.CheckValue(false, "x").IsTrue();
            errors.CheckValue(true, "y").IsFalse();
            var ex = XAssert.ThrowsException<MultipleErrorsException>(() => errors.Throw());
            XAssert.AreEqual(2, ex.Errors.Count);
        }
    }
}
