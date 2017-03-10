using System;

namespace Qoden.Validation
{
    public static class NullableValidations
    {
        public static Check<T> HasValue<T>(this Check<T?> check, string message = "{Key} should have value",
            Action<Error> onError = null)
            where T : struct
        {
            if (check.Value.HasValue)
                return new Check<T>(check.Value.Value, check.Key, check.Validator, check.OnErrorAction);
            check.FailValidator(new Error(message), onError);
            return new Check<T>(default(T), check.Key, check.Validator, check.OnErrorAction);
        }
    }
}