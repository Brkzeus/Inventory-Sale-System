using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventorySystem_Core.DTOs
{
    public class APIResponse<T> where T : class, new()
    {
        [JsonPropertyOrder(2)]
        public bool Success { get; set; }
        [JsonPropertyOrder(3)]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyOrder(1)]
        public T? Data { get; set; }
        [JsonPropertyOrder(4)]
        public List<string>? Errors { get; set; }

        // Helper method for successful responses
        public static APIResponse<T> SuccessResponse(T data, string message = "Success")
        {
            return new APIResponse<T>
            {
                Data = data,
                Success = true,
                Message = message,
                Errors = null
            };
        }

        public static APIResponse<T> SuccessResponse(string message)
        {
            return new APIResponse<T>
            {
                Data = default,
                Success = true,
                Message = message,
                Errors = null
            };
        }

        // Helper method for failed responses
        public static APIResponse<T> FailureResponse(string message, List<string>? errors = null)
        {
            return new APIResponse<T>
            {
                Success = false,
                Message = message,
                Data = default, // Automatically handles null for DummyClass or any T
                Errors = errors
            };
        }

    }
}
