using System;
using System.Text.RegularExpressions;

namespace Qoden.Validation
{
    public static class FileNameValidations
    {
        public static Regex FileName = new Regex("^(\\p{L}|[0-9 _-])+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsFileName(string str)
        {
            if (str == null) return false;
            return FileName.IsMatch(str) && str.Trim().Length > 0 && str.Trim().Length <= 200;
        }

        public const string IsFileNameErrorMessage = "{Key} should not contain /:\\*?\"<>|";

        public static Check<string> IsFileName(this Check<string> check, string message = IsFileNameErrorMessage,
            Action<Error> onError = null)
        {
            if (!IsFileName(check.Value))
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