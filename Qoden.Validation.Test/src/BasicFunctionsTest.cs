using NUnit.Framework;
using XAssert = NUnit.Framework.Assert;

namespace Qoden.Validation.Test
{
	[TestFixture]
	public class BasicFunctionsTest
	{
		[Test]
		public void SanityCheck()
		{
			var v = new Validator();
			var check = v.CheckValue<string>(null, "SomeValue");
			check.NotNull("Value {Key} should not be null");
			XAssert.False(v.IsValid);
			XAssert.True(v.HasErrorsForKey("SomeValue"));
			XAssert.True(v.HasErrors);
			var error = v.ErrorForKey("SomeValue");
			XAssert.AreEqual("Value SomeValue should not be null", error.Message);
		}

		[Test]
		public void ErrorInfoPostprocessing()
		{
			var v = new Validator();
			v.CheckValue<string>(null, "someKey").OnError(e => { e.Add("Code", "Some_Error_Code"); })
				.NotNull();

			var error = v.ErrorForKey("someKey");
			XAssert.True(error.ContainsKey("Code"));
			XAssert.AreEqual(error["Code"], "Some_Error_Code");
		}

		[Test]
		public void ErrorHasProperMessage()
		{

			var v = new Validator();
			v.CheckValue(1, "ValueName").GreaterOrEqualTo(10);
			var error = v.ErrorForKey("ValueName");
			StringAssert.Contains("ValueName", error.Message);
			StringAssert.Contains("1", error.Message);
			StringAssert.Contains("10", error.Message);
		}

		[Test]
		public void ErrorHasProperFlags()
		{
			var v = new Validator();
			var check = v.CheckValue(1, "ValueName");
			XAssert.True(check.IsValid);
			XAssert.False(check.HasError);
			XAssert.False(v.HasErrors);
			check = check.GreaterOrEqualTo(10);
			XAssert.False(check.IsValid);
			XAssert.True(check.HasError);
			XAssert.True(v.HasErrors);
			XAssert.NotNull(v.ErrorsForKey("ValueName"));
		}

		[Test]
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