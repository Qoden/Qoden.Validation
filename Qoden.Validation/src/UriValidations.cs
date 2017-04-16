using System;

namespace Qoden.Validation
{
    public static class UriValidations
    {
        public static Check<Uri> IsAbsoluteUri(this Check<Uri> check,
            string message = "{Key} expected to be absolute URI but was '{Value}'")
        {
            if (check.Value != null && !check.Value.IsAbsoluteUri)
            {
                check.Fail(new Error(message) {{"Value", check.Value}});
            }
            return check;
        }
    }
}