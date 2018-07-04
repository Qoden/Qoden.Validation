using System;

namespace Qoden.Validation
{
	public class ArgumentValidator : NonCollectingValidator
	{
		public static readonly ArgumentValidator Instance = new ArgumentValidator(false);
		public static readonly ArgumentValidator SkipNullsInstance = new ArgumentValidator(true);

		public ArgumentValidator(bool skipNulls) : base(skipNulls)
		{
		}

		public override void Add(Error error)
		{
			Exception ex = null;
			if (error.ContainsKey("Value") && error["Value"] == null)
				ex = new ArgumentNullException(error.Message, error.Key);
			else
				ex = new ArgumentException(error.Message, error.Key);
			throw ex;
		}
	}
	
}