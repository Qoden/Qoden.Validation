using System;
using System.Text.RegularExpressions;

namespace Qoden.Validation
{
    public static class FileNameValidations
    {
        public static Regex FileName = new Regex("[/:\\*?\"<>|]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsFileName(string str)
        {
            return str != null && FileName.IsMatch(str);
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