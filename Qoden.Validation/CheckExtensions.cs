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
    }
}