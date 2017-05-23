using System;
using Microsoft.AspNetCore.Http;

namespace Qoden.Validation.AspNetCore
{
    public static class Validations
    {
        public const string NotFoundMessage = "{Type} not found";

        public static Check<T> Found<T>(this Check<T> check,
                                        string message = NotFoundMessage,
                                        Action<Error> onError = null) where T : class
        {
            if (check.Value == null)
            {
                var error = new Error(message)
                {
                    {"Type", typeof(T).Name},
                    {"StatusCode", StatusCodes.Status404NotFound}
                };
                check.FailValidator(error, onError);
            }
            return check;
        }
    }
}
