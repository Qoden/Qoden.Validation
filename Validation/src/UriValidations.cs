using System;

namespace Qoden.Validation
{
    public static class UriValidations
    {
        public const string ErrorMessage = "{Key} expected to be absolute URI but was '{Value}'";

        public static Check<Uri> IsAbsoluteUri(this Check<Uri> check,
                                               string message = ErrorMessage,
                                              Action<Error> onError = null)
        {
            if (check.Value == null || !check.Value.IsAbsoluteUri)
            {
                check.FailValidator(new Error(message) { { "Value", check.Value } }, onError);
            }
            return check;
        }

        public static Check<Uri> IsAbsoluteUri(this Check<string> check,
                                               string message = ErrorMessage,
                                               Action<Error> onError = null)
        {
            try
            {
                var uri = new Uri(check.Value, UriKind.Absolute);
                return new Check<Uri>(uri, check.Key, check.Validator, check.OnErrorAction);
            }
            catch (Exception e)
            {
                check.FailValidator(new Error(message) {
                    { "Value", check.Value },
                    { "Exception", e}
                }, onError);
                return new Check<Uri>(null, check.Key, check.Validator, check.OnErrorAction);
            }
        }
    }
}