using System;
using System.Runtime.CompilerServices;

namespace Qoden.Validation
{
    public static class CheckExtensions
    {
        /// <summary>
        /// Set <see cref="Check{T}.OnErrorAction"/>
        /// </summary>
        public static Check<T> OnError<T>(this Check<T> check, Action<Error> onError)
        {
            check.OnErrorAction = onError;
            return check;
        }

        public static void FailValidator<T>(this Check<T> check, Error error, Action<Error> onError = null, [CallerMemberName] string validator = null)
        {
            onError?.Invoke(error);
            error.Add("Validator", validator);
            check.Fail(error);
        }

        public static Check<T> CustomValidation<T>(this Check<T> check, Func<T, bool> validatorFunc, string message = "Object didn't pass validation", 
                                                      Action<Error> onError = null)
        {
            if (!validatorFunc(check.Value))
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