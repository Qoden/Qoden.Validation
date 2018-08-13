using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Qoden.Validation.AspNetCore
{
    [DataContract]
    public class ApiError
    {
        public const string ErrorCodeKey = "ApiErrorCode";
        public const string StatusCodeKey = "StatusCode";
        public static readonly string StatusCodeKeySnake = StatusCodeKey.ToSnakeCase();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError" /> class.
        /// </summary>
        /// <param name="code">Code (required) (default to &quot;unknown_error&quot;)</param>
        /// <param name="message">Message (required)</param>
        /// <param name="statusCode"></param>
        public ApiError(string code, string message, int statusCode = 400)
        {
            Code = code ?? throw new ArgumentException("Code is a required property for Error and cannot be null");
            Message = message ?? throw new ArgumentException(
                          "Message is a required property for Error and cannot be null");
            StatusCode = statusCode;
        }

        [JsonExtensionData]
        public IDictionary<string, object> Data { get; set; }

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        public int StatusCode { get; }
    }

    [DataContract]
    public class ErrorResponse
    {
        /// <summary>
        /// Default constructor for deserialization
        /// </summary>
        public ErrorResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse" /> class.
        /// </summary>
        /// <param name="errors">Errors (required).</param>
        public ErrorResponse(List<ApiError> errors)
        {
            Errors = errors ?? throw new ArgumentException(
                         "Errors is a required property for ErrorResponse and cannot be null");
        }

        /// <summary>
        /// Gets or Sets Errors
        /// </summary>
        [DataMember(Name = "errors")]
        public List<ApiError> Errors { get; set; }
    }
}