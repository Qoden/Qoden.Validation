using System;
using System.Text.RegularExpressions;

namespace Qoden.Validation
{
    public static class PasswordValidations
    {
        public const string ErrorPasswordPattern = @"[\x00-\x1F]";

        public static bool IsPassword(string str)
        {
            if (str == null) return false; 
            return str.Length > 0 && !Regex.IsMatch(str, ErrorPasswordPattern);
        }

        public static bool IsPasswordOrEmpty(string str) 
        {
            if (str == null) return true;
            if (str.Length == 0) return true;
            return IsPassword(str);
        }

        public const string IsPasswordErrorMessage = "{Key} should not contain characters(ASCII 0-31) symbols and be not empty";
        public const string IsPasswordOrEmptyErrorMessage = "{Key} should not contain characters(ASCII 0-31) symbols";

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

        public static Check<string> IsPasswordOrEmpty(this Check<string> check, string message = IsPasswordOrEmptyErrorMessage,
           Action<Error> onError = null)
        {
            if (!IsPasswordOrEmpty(check.Value))
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