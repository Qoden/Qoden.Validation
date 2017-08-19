using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
    [TestClass]
    public class DictionaryValidationsTest : ValidationsTestBase
    {
        Dictionary<string, string> dict;

        [TestInitialize]
        public void Init()
        {
            dict = new Dictionary<string, string>();
        }

        [TestMethod]
        public void EmailValidation()
        {
            dict.Add("testKey", "testValue");
            Validator.CheckValue(dict, "someEmail").ContainsKey("testKey");
            XAssert.IsTrue(Validator.IsValid);
        }

        [TestMethod]
        public void InvalidEmails()
        {
            Validator.CheckValue(dict, "someEmail").ContainsKey("testKey");
            XAssert.IsFalse(Validator.IsValid);
        }
    }
}

