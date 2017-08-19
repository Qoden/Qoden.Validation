using System.Collections;

namespace Qoden.Validation
{
    public static class DictionaryValidations
    {
        public static Check<T> ContainsKey<T>(this Check<T> check, object key,
            string message = "{Key} should have key {Value}") where T : IDictionary
        {
            if (check.Value == null || !check.Value.Contains(key))
            {
                check.Fail(new Error(message) {{"Value", key}});
            }
            return check;
        }
    }
}