using System;

namespace Qoden.Validation
{
	public static class EmptyValidations
	{
		public const string NotEmptyMessage = "{Key} expected to be not null and not empty";

		public static Check<string> NotEmpty (this Check<string> check, string message = NotEmptyMessage, Action<Error> onError = null)
        {
			if (string.IsNullOrWhiteSpace (check.Value)) {
				check.FailValidator(new Error(message), onError);
			}
			return check;
		}
	}
}

