using System;
using System.Text.RegularExpressions;

namespace Qoden.Validation
{
    public static class DigitOrLetterValidations
    {
        public static Regex DigitOrLetter = new Regex("^[0-9A-Z]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsDigitOrLetter(string str)
        {
            return str != null && DigitOrLetter.IsMatch(str);
        }

        public const string IsDigitOrLetterErrorMessage = "{Key} should contaion only letters or digits";

        public static Check<string> IsDigitOrLetter(this Check<string> check, string message = IsDigitOrLetterErrorMessage,
            Action<Error> onError = null)
        {
            if (!IsDigitOrLetter(check.Value))
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