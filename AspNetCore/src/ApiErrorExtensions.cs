using System.Collections.Generic;

namespace Qoden.Validation.AspNetCore
{
    public static class ApiErrorExtensions
    {
        public static void ApiErrorCode(this Error error, string code)
        {
            error.Add(ApiError.ErrorCodeKey, code);
        }

        public static void StatusCode(this Error error, string code)
        {
            error.Add(ApiError.StatusCodeKey, code);
        }

        public static string ApiErrorCode(this Error error)
        {
            if (error.ContainsKey(ApiError.ErrorCodeKey))
                return error[ApiError.ErrorCodeKey] as string;
            return null;
        }

        public static string StatusCodeKey(this Error error)
        {
            if (error.ContainsKey(ApiError.StatusCodeKey))
                return error[ApiError.StatusCodeKey] as string;
            return null;
        }

        public static bool IsValidationError(this Error e)
        {
            return e.ApiErrorCode() == null && !string.IsNullOrEmpty(e.Key);
        }

        public static ApiError ToApiError(this Error error)
        {
            string code = "";
            if (error.TryGetValue(ApiError.ErrorCodeKey, out var apiErrorCode))
            {
                code = apiErrorCode.ToString().ToSnakeCase();
            }

            var apiError = new ApiError(code, error.Message);
            foreach (var kv in error)
            {
                if (string.IsNullOrEmpty(kv.Key)) continue;
                if (kv.Value == null) continue;
                if (kv.Key == "Validator") continue;
                if (kv.Key == ApiError.ErrorCodeKey) continue;
                if (kv.Key == "Exception") continue;
                if (kv.Key == "Key" && string.IsNullOrEmpty(kv.Value.ToString())) continue;

                if (apiError.Data == null)
                {
                    apiError.Data = new Dictionary<string, object>();
                }

                apiError.Data[kv.Key.ToSnakeCase()] = kv.Value;
            }

            return apiError;
        }
    }
}