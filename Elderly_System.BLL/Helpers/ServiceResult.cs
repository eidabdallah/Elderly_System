using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderlySystem.BLL.Helpers
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        public static ServiceResult SuccessMessage(string message)
        {
            return new ServiceResult
            {
                Success = true,
                Message = message
            };
        }
        public static ServiceResult SuccessWithData(object data, string message)
        {
            return new ServiceResult
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
        public static ServiceResult Failure(string message)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message
            };
        }
    }
}
