using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthRoutes.Core.Services
{
    public class ApiResult<T>
    {
        public string Message { get; set; }
        public bool Success { get; set; }

        public T Result { get; set; }

        public static ApiResult<T> AsHappyDandy(T result)
        {
            return new ApiResult<T>() { Success = true, Result = result };
        }

        public static ApiResult<T> AsDeathAndSorrow(string message)
        {
            return new ApiResult<T>() { Success = false, Message = message };
        }

        public ApiResult ToNonGeneric()
        {
            return new ApiResult() { Success = this.Success, Message = this.Message };
        }
    }

    public class ApiResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }

        public static ApiResult AsHappyDandy()
        {
            return new ApiResult() { Success = true };
        }

        public static ApiResult AsDeathAndSorrow(string message)
        {
            return new ApiResult() { Success = false, Message = message };
        }
    }
}
