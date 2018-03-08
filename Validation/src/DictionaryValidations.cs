using System;
using System.Collections;

namespace Qoden.Validation
{
    public static class DictionaryValidations
    {
        public static Check<T> ContainsKey<T>(this Check<T> check, object key,
                                              string message = "{Key} should have key {Value}",
                                              Action<Error> onError = null) where T : IDictionary
        {
            if (check.Value == null || !check.Value.Contains(key))
            {
                check.FailValidator(new Error(message) {{"Value", key}}, onError);
            }

            return check;
        }
    }
}