using System.Collections.Generic;
using NUnit.Framework;
using XAssert = NUnit.Framework.Assert;

namespace Qoden.Validation.Test
{
	[TestFixture]
	public class LengthValidationTest
	{
		[Test]
		public void LengthValidation()
		{
			var v = new Validator();
			var value = new List<int> { 1, 2, 3 };
			v.CheckValue(value, "Array").MinLength(3).MaxLength(3);
			XAssert.False(v.HasErrors);

			var check = v.CheckValue(value, "Array");
			check.MinLength(4);
			XAssert.AreEqual(check.Error["Min"], 4);
			XAssert.AreEqual(check.Error["Value"], 3);
			XAssert.AreEqual(check.Error["Validator"], "MinLength");

			check = v.CheckValue(value, "Array");
			check.MaxLength(2);
			XAssert.AreEqual(check.Error["Max"], 2);
			XAssert.AreEqual(check.Error["Value"], 3);
			XAssert.AreEqual(check.Error["Validator"], "MaxLength");
		}
	}
}
