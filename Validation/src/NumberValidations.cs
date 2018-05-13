using System;
using System.Text.RegularExpressions;

namespace Qoden.Validation
{
    public static class NumberValidations
    {
        public static Regex Number = new Regex("^[0-9]+([.,][0-9]+){0,1}[0-9]*$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsNumber(string str)
        {
            return str != null && Number.IsMatch(str);
        }

        public const string IsNumberErrorMessage = "{Key} should be an number";

        public static Check<string> IsNumber(this Check<string> check, string message = IsNumberErrorMessage,
            Action<Error> onError = null)
        {
            if (!IsNumber(check.Value))
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
