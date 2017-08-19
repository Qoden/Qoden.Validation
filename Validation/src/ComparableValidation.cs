using System;

namespace Qoden.Validation
{
    public static class ComparableValidation
    {
        public const string NumberMessage = "{Key} is not a number";

        public const string GreaterOrEqualToMessage = "{Key} ({Value}) expected to be more than or equal to {Min}";

        public static Check<T> GreaterOrEqualTo<T>(this Check<T> check, T min, string message = GreaterOrEqualToMessage, Action<Error> onError = null)
            where T : struct, IComparable
        {
            if (min.CompareTo(check.Value) > 0)
            {
                check.FailValidator(MakeError(check.Value, min, null, message), onError);
            }
            return check;
        }

        public const string GreaterMessage = "{Key} ({Value}) expected to be more than {Min}";
        public static Check<T> Greater<T>(this Check<T> check, T min, string message = GreaterMessage, Action<Error> onError = null)
            where T : struct, IComparable
        {
            if (min.CompareTo(check.Value) >= 0)
            {
                check.FailValidator(MakeError(check.Value, min, null, message), onError);
            }
            return check;
        }

        public const string LessOrEqualToMessage = "{Key} ({Value}) expected to be less than {Max}";

        public static Check<T> LessOrEqualTo<T>(this Check<T> check, T max, string message = LessOrEqualToMessage, Action<Error> onError = null)
            where T : struct, IComparable
        {
            if (max.CompareTo(check.Value) < 0)
            {
                check.FailValidator(MakeError(check.Value, null, max, message), onError);
            }
            return check;
        }

        public const string LessMessage = "{Key} ({Value}) expected to be less than {Max}";

        public static Check<T> Less<T>(this Check<T> check, T max, string message = LessMessage, Action<Error> onError = null)
            where T : struct, IComparable
        {
            if (max.CompareTo(check.Value) <= 0)
            {
                check.FailValidator(MakeError(check.Value, null, max, message), onError);
            }
            return check;
        }

        public const string BetweenMessage = "{Key} ({Value}) expected to be between {Min} and {Max}";

        public static Check<T> Between<T>(this Check<T> check, T min, T max, string message = BetweenMessage, Action<Error> onError = null)
            where T : struct, IComparable
        {
            if (min.CompareTo(check.Value) >= 0 || max.CompareTo(check.Value) <= 0)
            {
                check.FailValidator(MakeError(check.Value, min, max, message), onError);
            }
            return check;
        }

        public const string BetweenInclusiveMessage = "{Key} ({Value}) expected to be between {Min} and {Max}";

        public static Check<T> BetweenInclusive<T>(this Check<T> check, T min, T max, string message = BetweenInclusiveMessage, Action<Error> onError = null)
            where T : struct, IComparable
        {
            if (min.CompareTo(check.Value) > 0 || max.CompareTo(check.Value) < 0)
            {
                check.FailValidator(MakeError(check.Value, min, max, message), onError);
            }
            return check;
        }

        private static Error MakeError<T>(T value, T? min, T? max, string message)
            where T : struct, IComparable
        {
            var error = new Error(message)
            {
                {"Value", value}
            };
            if (min.HasValue) error.Add("Min", min.Value);
            if (max.HasValue) error.Add("Max", max.Value);
            return error;
        }
    }
}