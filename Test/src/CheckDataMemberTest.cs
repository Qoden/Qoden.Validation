using System;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
    [TestClass]
    public class CheckDataMemberTest : ValidationsTestBase
    {
        public class Dto
        {
            [DataMember]
            public string Title { get; set; }
            [DataMember(Name = "description")]
            public string Description { get; set; }
            public object NonMember { get; internal set; }
        }

        [TestMethod]
        public void CheckDataMemberWithoutName()
        {
            var dto = new Dto();
            Validator.CheckDataMember(dto, x => x.Title).NotNull();
            var error = Validator.ErrorForKey("Title");
            XAssert.IsNotNull(error);
        }

        [TestMethod]
        public void CheckDataMemberWithName()
        {
            var dto = new Dto();
            Validator.CheckDataMember(dto, x => x.Description).NotNull();
            var error = Validator.ErrorForKey("description");
            XAssert.IsNotNull(error);
        }

        [TestMethod]
        public void CheckNonDataMember()
        {            
            var dto = new Dto();
            Validator.CheckDataMember(dto, x => x.NonMember).NotNull();  
            var error = Validator.ErrorForKey("NonMember");
            XAssert.IsNotNull(error);          
        }
    }
}
