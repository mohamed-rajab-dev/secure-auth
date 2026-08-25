using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SecureAuth.Application.Common.Result
{
    public record Result
    {
        public bool Success { get; init; }

        public string Message { get; init; } = string.Empty;

        public Dictionary<string, List<string>>? Errors { get; init; }


        public static Result FailureResult(string message)
        {
            return new Result
            {
                Success = false,
                Message = message
            };
        }

        public static Result FailureResult(string message, string key, params string[] errorMessages)
        {

            var errors = new Dictionary<string, List<string>>
            {
                [key] = errorMessages.ToList()
            };

            return new Result
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

        public static Result FailureResult(string message, Dictionary<string, List<string>> errors)
        {
            return new Result
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

        public static Result SuccessResult(string message)
        {
            return new Result
            {
                Success = true,
                Message = message
            };
        }
    }

    public record Result<T> : Result
    {
        public T? Data { get; init; }

        public static Result<T> SuccessResult(string message, T? data = default)
        {
            return new Result<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public new static Result<T> FailureResult(string message)
        {
            return new Result<T>
            {
                Success = false,
                Message = message
            };
        }

        public new static Result<T> FailureResult(
            string message,
            string key,
            params string[] errorMessages)
        {
            return new Result<T>
            {
                Success = false,
                Message = message,
                Errors = new Dictionary<string, List<string>>
                {
                    [key] = errorMessages.ToList()
                }
            };
        }

        public new static Result<T> FailureResult(
            string message,
            Dictionary<string, List<string>> errors)
        {
            return new Result<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
