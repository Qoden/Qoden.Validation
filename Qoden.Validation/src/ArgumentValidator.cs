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

            foreach (var kv in error)
            {
                ex.Data.Add(kv.Key, kv.Value);
            }

			throw ex;
		}

		public const string ErrorKey = "Qoden.Error";
	}

	public static class ArgumentValidatorExtensions 
	{
		public static Error GetQodenError(this ArgumentException ex)
		{
			if (ex.Data.Contains(ArgumentValidator.ErrorKey)) {
				return ex.Data[ArgumentValidator.ErrorKey] as Error;	
			}
			return null;
		}
	}
}