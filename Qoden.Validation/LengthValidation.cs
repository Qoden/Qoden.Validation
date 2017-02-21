using System;
using System.Collections;

namespace Qoden.Validation
{
    public static class LengthValidation
    {
        public const string MinLengthMessage = "{Key} length must be more than {Min}";
        public const string MaxLengthMessage = "{Key} length must be less than {Max}";

        public static Check<string> MinLength(this Check<string> check, int min, string message = MinLengthMessage,
            Action<Error> onError = null)
        {
            var actual = check.Value?.Length ?? 0;
            if (actual < min)
            {
                check.FailValidator(new Error(message)
                {
                    {"Value", actual},
                    {"Min", min}
                }, onError);
            }
            return check;
        }

        public static Check<T> MinLength<T>(this Check<T> check, int min, string message = MinLengthMessage,
            Action<Error> onError = null)
            where T : IList
        {
            var actual = check.Value?.Count ?? 0;
            if (actual < min)
            {
                check.FailValidator(new Error(message)
                {
                    {"Value", actual},
                    {"Min", min}
                }, onError);
            }
            return check;
        }

        public static Check<string> MaxLength(this Check<string> check, int max, string message = MaxLengthMessage,
            Action<Error> onError = null)
        {
            var actual = check.Value?.Length ?? 0;            
            if (actual > max)
            {
                check.FailValidator(new Error(message)
                {
                    {"Value", actual},
                    {"Max", max}
                }, onError);
            }
            return check;
        }

        public static Check<T> MaxLength<T>(this Check<T> check, int max, string message = MaxLengthMessage,
            Action<Error> onError = null)
            where T : IList
        {
            var actual = check.Value?.Count ?? 0;
            if (actual > max)
            {
                check.FailValidator(new Error(message)
                {
                    {"Value", actual},
                    {"Max", max}
                }, onError);
            }
            return check;
        }
    }
}