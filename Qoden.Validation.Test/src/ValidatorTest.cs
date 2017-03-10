using NUnit.Framework;
using XAssert = NUnit.Framework.Assert;

namespace Qoden.Validation.Test
{
    public class ValidatorTest
    {
        [Test]
        public void EmptyErrorListValid()
        {
            var errors = new Validator();
            XAssert.True(errors.IsValid);
            XAssert.False(errors.HasErrors);
        }

        [Test]
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
            XAssert.AreEqual(key1Errors, new[] { key11, key12 });
            var allErrors = errors.Errors;
            XAssert.AreEqual(allErrors, new[] { key11, key12, key21 });

            XAssert.True(errors.HasErrorsForKey("Key1"));
            XAssert.True(errors.HasErrorsForKey("Key2"));
        }

        [Test]
        public void ErrorListThrows()
        {
            var errors = new Validator();
            errors.CheckValue(false, "x").IsTrue();
            errors.CheckValue(true, "y").IsFalse();
            var ex = XAssert.Throws<MultipleErrorsException>(() => errors.Throw());
            XAssert.AreEqual(2, ex.Errors.Count);
        }
    }
}
