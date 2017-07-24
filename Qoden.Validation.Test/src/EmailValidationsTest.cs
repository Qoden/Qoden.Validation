using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
    [TestClass]
    public class EmailValidationsTest : ValidationsTestBase
    {
        [TestMethod]
        public void EmailValidation()
        {
            Validator.CheckValue("test@mailinator.com", "someEmail").IsEmail();
            XAssert.IsTrue(Validator.IsValid);
        }

        [TestMethod]
        public void InvalidEmails()
        {
            Validator.CheckValue("mailinator.com", "someEmail").IsEmail();
            XAssert.IsFalse(Validator.IsValid);

            Validator.CheckValue("@mailinator.com", "someEmail").IsEmail();
            XAssert.IsFalse(Validator.IsValid);

            Validator.CheckValue("sadsf@mailinator", "someEmail").IsEmail();
            XAssert.IsFalse(Validator.IsValid);
        }

        [TestMethod]
        public void NullEmail()
        {
            Validator.CheckValue((string)null).IsEmail();
            XAssert.IsFalse(Validator.IsValid);
        }
    }
}
