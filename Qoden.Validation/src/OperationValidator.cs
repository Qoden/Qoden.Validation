using System;

namespace Qoden.Validation
{
    public class OperationValidator : NonCollectingValidator
    {
        public static readonly OperationValidator Instance = new OperationValidator();

        public override void Add(Error error)
        {
            var ex = new InvalidOperationException(error.Message);
			ex.Data.Add(ErrorKey, error);
			throw ex;
        }

		public const string ErrorKey = "Qoden.Error";
    }

	public static class OperationValidatorExtensions
	{
		public static Error GetQodenError(this InvalidOperationException ex)
		{
			if (ex.Data.Contains(OperationValidator.ErrorKey))
			{
				return ex.Data[OperationValidator.ErrorKey] as Error;
			}
			return null;
		}
	}
}