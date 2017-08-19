using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
    public class BasicFunctionsTest : ValidationsTestBase
	{
		[TestMethod]
		public void SanityCheck()
		{
			var check = Validator.CheckValue<string>(null, "SomeValue");
			check.NotNull("Value {Key} should not be null");
			XAssert.IsFalse(Validator.IsValid);
			XAssert.IsTrue(Validator.HasErrorsForKey("SomeValue"));
			XAssert.IsTrue(Validator.HasErrors);
			var error = Validator.ErrorForKey("SomeValue");
			XAssert.AreEqual("Value SomeValue should not be null", error.Message);
		}

		[TestMethod]
		public void ErrorInfoPostprocessing()
		{
			Validator.CheckValue<string>(null, "someKey").OnError(e => { e.Add("Code", "Some_Error_Code"); })
				.NotNull();

			var error = Validator.ErrorForKey("someKey");
			XAssert.IsTrue(error.ContainsKey("Code"));
			XAssert.AreEqual(error["Code"], "Some_Error_Code");
		}

		[TestMethod]
		public void ErrorHasProperMessage()
		{
			Validator.CheckValue(1, "ValueName").GreaterOrEqualTo(10);
			var error = Validator.ErrorForKey("ValueName");
			StringAssert.Contains(error.Message, "ValueName");
			StringAssert.Contains(error.Message, "1");
			StringAssert.Contains(error.Message, "10");
		}

		[TestMethod]
		public void ErrorHasProperFlags()
		{            
			var check = Validator.CheckValue(1, "ValueName");
			XAssert.IsTrue(check.IsValid);
			XAssert.IsFalse(check.HasError);
			XAssert.IsFalse(Validator.HasErrors);
			check = check.GreaterOrEqualTo(10);
			XAssert.IsFalse(check.IsValid);
			XAssert.IsTrue(check.HasError);
			XAssert.IsTrue(Validator.HasErrors);
			XAssert.IsNotNull(Validator.ErrorsForKey("ValueName"));
		}

		[TestMethod]
		public void PostProcessorsCanCustomizeError()
		{
			var check = Validator.CheckValue(true, "err1")
				.OnError(e => e.Add("Code", "SomeCodeErr1"))
				.IsFalse();
			XAssert.AreEqual(check.Error["Code"], "SomeCodeErr1");
		}
	}
}