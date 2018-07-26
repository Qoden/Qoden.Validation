using System;
using System.Linq;

namespace Qoden.Validation
{
    public static class EqualityValidation
    {
        public const string EqualityMessage = "{Key} is not equal to {Expected}";

        public static Check<T> EqualsTo<T>(this Check<T> check, T value, string message = EqualityMessage, Action<Error> onError = null)
        {
            if (!Equals(check.Value, value))
            {
                check.FailValidator(new Error(message) {
                    {"Expected", value },
                    {"Value", check.Value}
                }, onError);
            }
            return check;
        }

        public const string NullMessage = "{Key} must be null";

        public static Check<T> IsNull<T>(this Check<T> check, string message = NullMessage, Action<Error> onError = null)
            where T : class
        {
            if (!ReferenceEquals(check.Value, null))
            {
                check.FailValidator(new Error(message) { { "Value", check.Value } }, onError);
            }
            return check;
        }

        public const string NotNullMessage = "{Key} cannot be null";

        public static Check<T> NotNull<T>(this Check<T> check, string message = NotNullMessage, Action<Error> onError = null)
        {
            if (ReferenceEquals(check.Value, null))
            {
                check.FailValidator(new Error(message) { { "Value", null } }, onError);
            }
            return check;
        }
        
        public const string NotDefaultMessage = "{Key} cannot be equivalent to default value";

        public static Check<T> NotDefault<T>(this Check<T> check, string message = NotDefaultMessage, Action<Error> onError = null)
        {
            if (check.Value.Equals(default(T)))
            {
                check.FailValidator(new Error(message) {{ "Value", default(T) }}, onError);
            }
            return check;
        }
        
        public static Check<bool> IsTrue(this Check<bool> check, string message = EqualityMessage, Action<Error> onError = null)
        {
            if (!check.Value)
            {
                check.FailValidator(new Error(message){
                    {"Value", check.Value},
                    {"Expected", true}
                }, onError);
            }
            return check;
        }

        public static Check<bool> IsFalse(this Check<bool> check, string message = EqualityMessage, Action<Error> onError = null)
        {
            if (check.Value)
            {
                check.FailValidator(new Error(message){
                    {"Value", check.Value},
                    {"Expected", false}
                }, onError);
            }
            return check;
        }

        public const string NotEqualsMessage = "{Key} should not be equals to {NotExpected}";

        public static Check<T> NotEqualsTo<T>(this Check<T> check, T value, string message = NotEqualsMessage, Action<Error> onError = null)
        {
            if (Equals(check.Value, value))
            {
                check.FailValidator(new Error(message)
                {
                    {"Value", check.Value},
                    {"NotExpected", value}
                }, onError);
            }
            return check;
        }

        public const string InMessage = "{Key} should be on of {Candidates}";
        public static Check<T> In<T>(this Check<T> check, T[] candidates, string message = InMessage, Action<Error> onError = null)
        {
            if (!candidates.Contains(check.Value))
            {
                var candidatesStr = GenerateCandidatesStr(candidates);
                check.FailValidator(new Error(message)
                {
                    {"Candidates", candidatesStr}
                }, onError);
            }
            return check;
        }

        public const string NotInMessage = "{Key} should not be on of {InvalidCandidates}";
        public static Check<T> NotIn<T>(this Check<T> check, T[] candidates, string message = NotInMessage, Action<Error> onError = null)
        {
            if (candidates.Contains(check.Value))
            {
                var candidatesStr = GenerateCandidatesStr(candidates);
                check.FailValidator(new Error(message)
                {
                    {"InvalidCandidates", candidatesStr}
                }, onError);
            }
            return check;
        }

        private static string GenerateCandidatesStr<T>(T[] candidates)
        {
            var candidatesStr = string.Join(",", candidates.AsEnumerable().Take(10));
            if (candidates.Length > 10) candidatesStr += ", ...";
            return candidatesStr;
        }
    }
}

