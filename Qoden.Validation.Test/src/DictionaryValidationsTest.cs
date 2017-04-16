using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
    [TestClass]
    public class DictionaryValidationsTest
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
            var v = new Validator();
            dict.Add("testKey", "testValue");
            v.CheckValue(dict, "someEmail").ContainsKey("testKey");
            XAssert.IsTrue(v.IsValid);
        }

        [TestMethod]
        public void InvalidEmails()
        {
            var v = new Validator();
            v.CheckValue(dict, "someEmail").ContainsKey("testKey");
            XAssert.IsFalse(v.IsValid);
        }
    }
}

