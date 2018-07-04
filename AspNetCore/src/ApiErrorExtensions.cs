using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Qoden.Validation.AspNetCore
{
    public static class ApiErrorExtensions
    {
        public static void ApiErrorCode(this Error error, string code)
        {
            error.Add(ApiError.ErrorCodeKey, code);
        }

        public static string ApiErrorCode(this Error error)
        {
            if (error.ContainsKey(ApiError.ErrorCodeKey))
                return error[ApiError.ErrorCodeKey] as string;
            return null;
        }
        
        public static void StatusCode(this Error error, int code)
        {
            error.Add(ApiError.StatusCodeKey, code);
        }
        
        public static int StatusCode(this Error error)
        {
            if (error.TryGetValue(ApiError.StatusCodeKey, out var statusCode))
            {
                return Convert.ToInt32(statusCode);
            }
            return 400;
        }

        public static ApiError ToApiError(this Error error)
        {
            string code = "";
            if (error.TryGetValue(ApiError.ErrorCodeKey, out var apiErrorCode))
            {
                code = apiErrorCode.ToString().ToSnakeCase();
            }

            int statusCode = 400;
            if (error.TryGetValue(ApiError.StatusCodeKey, out var statusCodeObj))
            {
                try
                {
                    statusCode = Convert.ToInt32(statusCodeObj);
                }
                catch (Exception)
                {
                    //ignore
                }
            }

            var apiError = new ApiError(code, error.Message, statusCode);
            foreach (var kv in error)
            {
                if (string.IsNullOrEmpty(kv.Key)) continue;
                if (kv.Value == null) continue;
                if (kv.Key == "Validator") continue;
                if (kv.Key == ApiError.ErrorCodeKey) continue;
                if (kv.Key == "Exception") continue;
                if (kv.Key == "StatusCode") continue;
                if (kv.Key == "Key" && string.IsNullOrEmpty(kv.Value.ToString())) continue;

                if (apiError.Data == null)
                {
                    apiError.Data = new Dictionary<string, object>();
                }

                apiError.Data[kv.Key.ToSnakeCase()] = kv.Value;
            }

            return apiError;
        }
        
        public static Error ToError(this ApiError apiError)
        {
            var error = new Error();

            if (!string.IsNullOrWhiteSpace(apiError.Code))
                error.Add(ApiError.ErrorCodeKey, apiError.Code);
            if (apiError.StatusCode != 0)
                error.Add(ApiError.StatusCodeKey, apiError.StatusCode);
            if (!string.IsNullOrWhiteSpace(apiError.Message))
                error.MessageFormat = apiError.Message;
            if (apiError.Data != null)
            {
                foreach (var data in apiError.Data)
                    error.Add(data.Key, data.Value);
            }

            return error;
        }

        public static bool IsErrorCode(this Error error, string code)
        {
            return error.TryGetValue(ApiError.ErrorCodeKey, out var errorCode) &&
                   errorCode.ToString() == code.ToSnakeCase();
        }
    }
}