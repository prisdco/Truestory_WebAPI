using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truestory.Domain.Models
{
    public class ResultViewModel
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public string Timestamp { get; }
        public bool IsFailure => !IsSuccess;

        protected ResultViewModel(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error;
        }

        public static ResultViewModel Fail(string message)
        {
            return new ResultViewModel(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result<T> Fail<T>(T value, string message)
        {
            return new Result<T>(value, false, message);
        }


        public static ResultViewModel Ok()
        {
            return new ResultViewModel(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static ResultViewModel Combine(params ResultViewModel[] results)
        {
            foreach (var result in results)
            {
                if (result.IsFailure)
                    return result;
            }
            return Ok();
        }

        public static ResultViewModel Combine<T>(params ResultViewModel[] results)
        {
            foreach (var result in results)
            {
                if (result.IsFailure)
                    return Fail<T>(result.Error);
            }
            return ResultViewModel.Ok();
        }
    }

  
}
