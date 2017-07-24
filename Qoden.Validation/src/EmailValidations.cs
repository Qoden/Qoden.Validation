using System;
using System.Text.RegularExpressions;

namespace Qoden.Validation
{
    public static class EmailValidations
    {
        public static Regex Email = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,}$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsEmail(string str)
        {
            return str != null && Email.IsMatch(str);
        }

        public const string IsEmailErrorMessage = "{Key} should be an email";

        public static Check<string> IsEmail(this Check<string> check, string message = IsEmailErrorMessage,
            Action<Error> onError = null)
        {
            if (!IsEmail(check.Value))
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