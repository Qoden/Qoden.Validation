using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
    [TestClass]
    public class EmailValidationsTest
    {
        [TestMethod]
        public void EmailValidation()
        {
            var v = new Validator();
            v.CheckValue("test@mailinator.com", "someEmail").IsEmail();
            XAssert.IsTrue(v.IsValid);
        }

        [TestMethod]
        public void InvalidEmails()
        {
            var v = new Validator();
            v.CheckValue("mailinator.com", "someEmail").IsEmail();
            XAssert.IsFalse(v.IsValid);

            v.CheckValue("@mailinator.com", "someEmail").IsEmail();
            XAssert.IsFalse(v.IsValid);

            v.CheckValue("sadsf@mailinator", "someEmail").IsEmail();
            XAssert.IsFalse(v.IsValid);
        }
    }
}
