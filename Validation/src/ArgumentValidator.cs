using System;

namespace Qoden.Validation
{
	public class ArgumentValidator : NonCollectingValidator
	{
		public static readonly ArgumentValidator Instance = new ArgumentValidator();

		public override void Add(Error error)
		{
			Exception ex = null;
			if (error.ContainsKey("Value") && error["Value"] == null)
				ex = new ArgumentNullException(error.Key);
			else
				ex = new ArgumentException(error.Message, error.Key);
			throw ex;
		}
	}
	
}