using Microsoft.VisualStudio.TestTools.UnitTesting;
using XAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Qoden.Validation.Test
{
	[TestClass]
	public class BasicFunctionsTest
	{
		[TestMethod]
		public void SanityCheck()
		{
			var v = new Validator();
			var check = v.CheckValue<string>(null, "SomeValue");
			check.NotNull("Value {Key} should not be null");
			XAssert.IsFalse(v.IsValid);
			XAssert.IsTrue(v.HasErrorsForKey("SomeValue"));
			XAssert.IsTrue(v.HasErrors);
			var error = v.ErrorForKey("SomeValue");
			XAssert.AreEqual("Value SomeValue should not be null", error.Message);
		}

		[TestMethod]
		public void ErrorInfoPostprocessing()
		{
			var v = new Validator();
			v.CheckValue<string>(null, "someKey").OnError(e => { e.Add("Code", "Some_Error_Code"); })
				.NotNull();

			var error = v.ErrorForKey("someKey");
			XAssert.IsTrue(error.ContainsKey("Code"));
			XAssert.AreEqual(error["Code"], "Some_Error_Code");
		}

		[TestMethod]
		public void ErrorHasProperMessage()
		{
			var v = new Validator();
			v.CheckValue(1, "ValueName").GreaterOrEqualTo(10);
			var error = v.ErrorForKey("ValueName");
			StringAssert.Contains(error.Message, "ValueName");
			StringAssert.Contains(error.Message, "1");
			StringAssert.Contains(error.Message, "10");
		}

		[TestMethod]
		public void ErrorHasProperFlags()
		{
			var v = new Validator();
			var check = v.CheckValue(1, "ValueName");
			XAssert.IsTrue(check.IsValid);
			XAssert.IsFalse(check.HasError);
			XAssert.IsFalse(v.HasErrors);
			check = check.GreaterOrEqualTo(10);
			XAssert.IsFalse(check.IsValid);
			XAssert.IsTrue(check.HasError);
			XAssert.IsTrue(v.HasErrors);
			XAssert.IsNotNull(v.ErrorsForKey("ValueName"));
		}

		[TestMethod]
		public void PostProcessorsCanCustomizeError()
		{
			var v = new Validator();

			var check = v.CheckValue(true, "err1")
				.OnError(e => e.Add("Code", "SomeCodeErr1"))
				.IsFalse();
			XAssert.AreEqual(check.Error["Code"], "SomeCodeErr1");
		}
	}
}