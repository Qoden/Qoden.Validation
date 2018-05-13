using System;
using System.Text.RegularExpressions;

namespace Qoden.Validation
{
    public static class PasswordValidations
    {
        public static Regex Password = new Regex("^\\S+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsPassword(string str)
        {
            return str != null && Password.IsMatch(str);
        }

        public const string IsPasswordErrorMessage = "{Key} should not contain whitespace symbols";

        public static Check<string> IsPassword(this Check<string> check, string message = IsPasswordErrorMessage,
            Action<Error> onError = null)
        {
            if (!IsPassword(check.Value))
            {
                var error = new Error(message)
                {
                    {"Value", check.Value}
                };
                check.FailValidator(error, onError);
            }
            return check;
        }
    }
}