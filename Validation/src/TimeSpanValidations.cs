using System;

namespace Qoden.Validation
{
    public static class TimeSpanValidations
    {
        public static bool IsTimeSpan(string str)
        {
            return str != null && TimeSpan.TryParse(str, out _);
        }

        public const string IsTimeSpanErrorMessage = "{Key} should be an timespan";

        public static Check<string> IsTimeSpan(this Check<string> check, string message = IsTimeSpanErrorMessage,
            Action<Error> onError = null)
        {
            if (!IsTimeSpan(check.Value))
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